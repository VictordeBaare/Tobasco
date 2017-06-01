using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;

namespace Tobasco
{
    public class FileProcessor
    {
        private readonly ProjectItem templateProjectItem;
        private readonly Action<string> checkOutAction;
        private readonly Action<IEnumerable<FileBuilder.OutputFile>> projectSyncAction;
        private readonly DTE dte;
        private readonly List<string> templatePlaceholderList = new List<string>();
        private readonly DynamicTextTransformation2 _textTransformation;

        public static FileProcessor Create(object textTransformation)
        {
            DynamicTextTransformation2 transformation = DynamicTextTransformation2.Create(textTransformation);
            IDynamicHost2 host = transformation.Host;
            return new FileProcessor(transformation);
        }

        public Encoding Encoding { get; set; }

        public void BeginProcessing(string path)
        {
            XmlLoader loader = new XmlLoader(_textTransformation);
            var handler = loader.Load(path);
            Process(handler.GetOutputFiles(_textTransformation));
        }

        private string GetFileTypeExtension(FileBuilder.OutputFile file)
        {
            switch (file.Type)
            {
                case FileType.Class:
                case FileType.Interface:
                    return ".cs";
                case FileType.Table:
                case FileType.Stp:
                    return ".sql";
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public virtual IEnumerable<FileBuilder.OutputFile> Process(IEnumerable<FileBuilder.OutputFile> files)
        {
            var filesToProcess = new List<FileBuilder.OutputFile>();

            foreach (var file in files)
            {
                var outputPath = VSHelper.GetOutputPath(dte, file, Path.GetDirectoryName(_textTransformation.Host.TemplateFile));
                file.FileName = Path.Combine(outputPath, file.Name) + "_Generated" + GetFileTypeExtension(file);
                
                var isNewOrAdjusted = IsNewFile(file);

                if (isNewOrAdjusted || !file.FileName.EndsWith(".sql"))
                {
                    filesToProcess.Add(file);
                }
                else
                {
                    _textTransformation.WriteLine($"// File {file.FileName} shall not be processed.");
                }
            }

            projectSyncAction.EndInvoke(projectSyncAction.BeginInvoke(filesToProcess, null, null));
            CleanUpTemplatePlaceholders();
            var items = VSHelper.GetOutputFilesAsProjectItems(dte, filesToProcess);
            WriteVsProperties(items, filesToProcess);

            return filesToProcess;
        }

        private void WriteVsProperties(IEnumerable<ProjectItem> items, IEnumerable<FileBuilder.OutputFile> outputFiles)
        {
            foreach (var file in outputFiles)
            {
                var item = items.Where(p => p.Name == Path.GetFileName(file.FileName)).FirstOrDefault();
                if (item == null) continue;

                if (string.IsNullOrEmpty(file.FileProperties.CustomTool) == false)
                {
                    VSHelper.SetPropertyValue(item, "CustomTool", file.FileProperties.CustomTool);
                }

                if (string.IsNullOrEmpty(file.FileProperties.BuildActionString) == false)
                {
                    VSHelper.SetPropertyValue(item, "ItemType", file.FileProperties.BuildActionString);
                }
            }
        }

        private FileProcessor(object textTransformation)
        {
            if (textTransformation == null)
            {
                throw new ArgumentNullException("textTransformation");
            }
            _textTransformation = DynamicTextTransformation2.Create(textTransformation);

            var hostServiceProvider = _textTransformation.Host.AsIServiceProvider();
            if (hostServiceProvider == null)
            {
                throw new ArgumentNullException("Could not obtain hostServiceProvider");
            }

            dte = (DTE)hostServiceProvider.GetService(typeof(DTE));
            if (dte == null)
            {
                throw new ArgumentNullException("Could not obtain DTE from host");
            }

            templateProjectItem = dte.Solution.FindProjectItem(_textTransformation.Host.TemplateFile);
            Encoding = Encoding.UTF8;
            checkOutAction = fileName => dte.SourceControl.CheckOutItem(fileName);
            projectSyncAction = keepFileNames => ProjectSync(templateProjectItem, keepFileNames);
        }

        private void CleanUpTemplatePlaceholders()
        {
            string[] activeTemplateFullNames = templatePlaceholderList.ToArray();
            string[] allHelperTemplateFullNames = VSHelper.GetAllSolutionItems(dte)
                .Where(p => p.Name == VSHelper.GetTemplatePlaceholderName(templateProjectItem))
                .Select(VSHelper.GetProjectItemFullPath)
                .ToArray();

            var delta = allHelperTemplateFullNames.Except(activeTemplateFullNames).ToArray();

            var dirtyHelperTemplates = VSHelper.GetAllSolutionItems(dte)
                .Where(p => delta.Contains(VSHelper.GetProjectItemFullPath(p)));

            foreach (ProjectItem item in dirtyHelperTemplates)
            {
                if (item.ProjectItems != null)
                {
                    foreach (ProjectItem subItem in item.ProjectItems)
                    {
                        subItem.Remove();
                    }
                }

                item.Remove();
            }
        }

        protected virtual bool IsNewFile(FileBuilder.OutputFile file)
        {
            if (IsFileContentDifferent(file))
            {
                _textTransformation.WriteLine($"// FileContent is different for: {file.FileName}");
                CheckoutFileIfRequired(file.FileName);
                File.WriteAllText(file.FileName, file.BuildContent(), Encoding);
                return true;
            }
            return false;
        }

        private void CheckoutFileIfRequired(string fileName)
        {
            if (dte.SourceControl == null
                || !dte.SourceControl.IsItemUnderSCC(fileName)
                    || dte.SourceControl.IsItemCheckedOut(fileName))
            {
                return;
            }

            // run on worker thread to prevent T4 calling back into VS
            checkOutAction.EndInvoke(checkOutAction.BeginInvoke(fileName, null, null));
        }

        protected bool IsFileContentDifferent(FileBuilder.OutputFile file)
        {
            return !(File.Exists(file.FileName) && File.ReadAllText(file.FileName) == file.BuildContent());
        }

        private void ProjectSync(ProjectItem templateProjectItem, IEnumerable<FileBuilder.OutputFile> keepFileNames)
        {
            var groupedFileNames = from f in keepFileNames
                                   group f by new { f.ProjectName, f.FolderName }
                                    into l
                                   select new
                                   {
                                       ProjectName = l.Key.ProjectName,
                                       FolderName = l.Key.FolderName,
                                       FirstItem = l.First(),
                                       OutputFiles = l
                                   };

            templatePlaceholderList.Clear();

            foreach (var item in groupedFileNames)
            {
                ProjectItem pi = VSHelper.GetTemplateProjectItem(templateProjectItem.DTE, item.FirstItem, templateProjectItem);
                ProjectSyncPart(pi, item.OutputFiles, _textTransformation);

                if (pi.Name.EndsWith("txt4"))
                    templatePlaceholderList.Add(VSHelper.GetProjectItemFullPath(pi));
            }

            // clean up
            bool hasDefaultItems = groupedFileNames.Where(f => string.IsNullOrEmpty(f.ProjectName) && string.IsNullOrEmpty(f.FolderName)).Any();
            if (hasDefaultItems == false)
            {
                ProjectSyncPart(templateProjectItem, new List<FileBuilder.OutputFile>(), _textTransformation);
            }
        }

        private static void ProjectSyncPart(ProjectItem templateProjectItem, IEnumerable<FileBuilder.OutputFile> keepFileNames, DynamicTextTransformation2 textTransformation)
        {
            var keepFileNameSet = new HashSet<FileBuilder.OutputFile>(keepFileNames);
            var projectFiles = new Dictionary<string, ProjectItem>();
            var originalOutput = Path.GetFileNameWithoutExtension(templateProjectItem.FileNames[0]);

            foreach (ProjectItem projectItem in templateProjectItem.ProjectItems)
            {
                projectFiles.Add(projectItem.FileNames[0], projectItem);
            }

            // Remove unused items from the project
            foreach (var pair in projectFiles)
            {
                bool isNotFound = keepFileNames.Where(f => f.FileName == pair.Key).Count() == 0;
                if (isNotFound == true
                    && !(Path.GetFileNameWithoutExtension(pair.Key) + ".").StartsWith(originalOutput + "."))
                {
                    pair.Value.Delete();
                }
            }

            // Add missing files to the project
            foreach (var fileName in keepFileNameSet)
            {
                textTransformation.WriteLine($"// check if file exists: {fileName.FileName}");
                if (!projectFiles.ContainsKey(fileName.FileName))
                {
                    textTransformation.WriteLine($"// Add {fileName.FileName}");
                    templateProjectItem.ProjectItems.AddFromFile(fileName.FileName);
                }
                else
                {
                    textTransformation.WriteLine($"// {fileName.FileName} already exists");
                }
            }
        }

        public static IEnumerable<ProjectItem> GetAllProjectItemsRecursive(ProjectItems projectItems)
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem.ProjectItems == null) continue;

                foreach (ProjectItem subItem in GetAllProjectItemsRecursive(projectItem.ProjectItems))
                {
                    yield return subItem;
                }


                yield return projectItem;
            }
        }

        public static IEnumerable<Project> GetAllProjects(DTE dte)
        {
            List<Project> projectList = new List<Project>();

            var folders = dte.Solution.Projects.Cast<Project>().Where(p => p.Kind == ProjectKinds.vsProjectKindSolutionFolder);

            foreach (Project folder in folders)
            {
                if (folder.ProjectItems == null) continue;

                foreach (ProjectItem item in folder.ProjectItems)
                {
                    if (item.Object is Project)
                        projectList.Add(item.Object as Project);
                }
            }

            var projects = dte.Solution.Projects.Cast<Project>().Where(p => p.Kind != ProjectKinds.vsProjectKindSolutionFolder);

            if (projects.Any())
                projectList.AddRange(projects);

            return projectList;
        }
    }
}
