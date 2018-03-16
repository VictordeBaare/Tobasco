using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.Manager;
using Tobasco.FileBuilder;

namespace Tobasco
{
    public class FileProcessor
    {
        private readonly ProjectItem _templateProjectItem;
        private readonly DTE _dte;
        private readonly List<string> _templatePlaceholderList = new List<string>();
        private readonly string _templateFile;


        public bool UseAutoFormatting { get; set; } = false;

        public static FileProcessor Create(object textTransformation)
        {
            DynamicTextTransformation2 transformation = DynamicTextTransformation2.Create(textTransformation);
            return new FileProcessor(transformation);
        }

        public void BeginProcessing(string path)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    OutputPaneManager.WriteToOutputPane("Load xmls.");
                    XmlLoader loader = new XmlLoader();
                    loader.Load(path);
                    OutputPaneManager.WriteToOutputPane("Start generating.");

                    OutputPaneManager.WriteToOutputPane("Get output files.");
                    var outputFiles = FileOutputManager.ResolveSingleOutputFiles();
                    outputFiles.AddRange(FileOutputManager.ResolveEntityFiles());
                    ProgressBarManager.SetTotal((uint)outputFiles.Count(), "");
                    OutputPaneManager.WriteToOutputPane($"{outputFiles.Count()} files found. ");

                    OutputPaneManager.WriteToOutputPane("Process output files");
                    Process(outputFiles);
                }
                catch (Exception ex)
                {
                    OutputPaneManager.WriteToOutputPane($"An error occured during generating. Message: {ex.Message} Stacktrace {ex.StackTrace}");
                    OutputPaneManager.WriteToOutputPane($"{ex.ToString()}");
                }

            }).ContinueWith(x =>
                {
                    OutputPaneManager.WriteToOutputPane("Finished generating");
                    ProgressBarManager.Done();
                }
            );
        }

        private string GetFileTypeExtension(OutputFile file)
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

        public virtual IEnumerable<OutputFile> Process(IEnumerable<OutputFile> files)
        {
            var filesToProcess = new List<OutputFile>();

            foreach (var file in files)
            {
                var outputPath = VsManager.GetOutputPath(_dte, file, Path.GetDirectoryName(_templateFile));
                file.FileName = Path.Combine(outputPath, file.Name) + "_Generated" + GetFileTypeExtension(file);
                
                var isNewOrAdjusted = IsNewFile(file);

                if (isNewOrAdjusted || !file.FileName.EndsWith(".sql"))
                {
                    OutputPaneManager.WriteToOutputPane($"File {file.Name} shall be processed");
                    filesToProcess.Add(file);
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"File {file.Name} shall not be processed.");
                }
            }

            ProjectSync(_templateProjectItem, filesToProcess);
            CleanUpTemplatePlaceholders();
            var items = VsManager.GetOutputFilesAsProjectItems(_dte, filesToProcess);
            WriteVsProperties(items, filesToProcess);

            return filesToProcess;
        }


        private void FormatDocument(ProjectItems items)
        {
            OutputPaneManager.WriteToOutputPane("Start formatting");

            foreach (ProjectItem item in items)
            {
                if (item.Name.EndsWith(".cs") && !item.IsOpen)
                {
                    var b = item.Open(EnvDTE.Constants.vsViewKindDebugging);
                    b.Activate();
                    item.Document.DTE.ExecuteCommand("Edit.FormatDocument");
                    b.Close(vsSaveChanges.vsSaveChangesYes);
                }
            }
        }

        private void WriteVsProperties(IEnumerable<ProjectItem> items, IEnumerable<OutputFile> outputFiles)
        {
            foreach (var file in outputFiles)
            {
                var item = items.FirstOrDefault(p => p.Name == Path.GetFileName(file.FileName));
                if (item == null) continue;

                if (string.IsNullOrEmpty(file.FileProperties.CustomTool) == false)
                {
                    VsManager.SetPropertyValue(item, "CustomTool", file.FileProperties.CustomTool);
                }

                if (string.IsNullOrEmpty(file.FileProperties.BuildActionString) == false)
                {
                    VsManager.SetPropertyValue(item, "ItemType", file.FileProperties.BuildActionString);
                }
            }
        }



        private FileProcessor(object textTransformation)
        {
            if (textTransformation == null)
            {
                throw new ArgumentNullException("textTransformation");
            }
            var dynamictextTransformation = DynamicTextTransformation2.Create(textTransformation);
            _templateFile = dynamictextTransformation.Host.TemplateFile;

            var hostServiceProvider = dynamictextTransformation.Host.AsIServiceProvider();
            if (hostServiceProvider == null)
            {
                throw new ArgumentNullException("Could not obtain hostServiceProvider");
            }

            _dte = (DTE)hostServiceProvider.GetService(typeof(DTE));
            if (_dte == null)
            {
                throw new ArgumentNullException("Could not obtain DTE from host");
            }
            var dteServiceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)_dte);
            OutputPaneManager.Activate(dteServiceProvider);
            ProgressBarManager.Activate(dteServiceProvider);
            _templateProjectItem = _dte.Solution.FindProjectItem(_templateFile);

        }

        private void CleanUpTemplatePlaceholders()
        {
            string[] activeTemplateFullNames = _templatePlaceholderList.ToArray();
            string[] allHelperTemplateFullNames = VsManager.GetAllSolutionItems(_dte)
                .Where(p => p.Name == VsManager.GetTemplatePlaceholderName(_templateProjectItem))
                .Select(VsManager.GetProjectItemFullPath)
                .ToArray();

            var delta = allHelperTemplateFullNames.Except(activeTemplateFullNames).ToArray();

            var dirtyHelperTemplates = VsManager.GetAllSolutionItems(_dte)
                .Where(p => delta.Contains(VsManager.GetProjectItemFullPath(p)));

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

        protected virtual bool IsNewFile(OutputFile file)
        {
            if (!FileExist(file))
            {
                OutputPaneManager.WriteToOutputPane($"File does not exist: {file.FileName}");
                CheckoutFileIfRequired(file.FileName);
                File.WriteAllText(file.FileName, file.BuildContent(), Encoding.UTF8);
                return true;
            }

            if (IsFileContentDifferent(file))
            {
                OutputPaneManager.WriteToOutputPane($"FileContent is different for: {file.FileName}");
                CheckoutFileIfRequired(file.FileName);
                File.WriteAllText(file.FileName, file.BuildContent(), Encoding.UTF8);
                return true;
            }
            return false;
        }

        private void CheckoutFileIfRequired(string fileName)
        {
            if (_dte.SourceControl == null
                || !_dte.SourceControl.IsItemUnderSCC(fileName)
                    || _dte.SourceControl.IsItemCheckedOut(fileName))
            {
                return;
            }

            _dte.SourceControl.CheckOutItem(fileName);
        }

        protected bool IsFileContentDifferent(OutputFile file)
        {
            return File.ReadAllText(file.FileName) != file.BuildContent();
        }

        protected bool FileExist(OutputFile file)
        {
            return File.Exists(file.FileName);
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

            _templatePlaceholderList.Clear();

            foreach (var item in groupedFileNames)
            {
                ProjectItem pi = VsManager.GetTemplateProjectItem(templateProjectItem.DTE, item.FirstItem, templateProjectItem);
                ProjectSyncPart(pi, item.OutputFiles);
                if (UseAutoFormatting)
                {
                    FormatDocument(pi.ProjectItems);
                }
                if (pi.Name.EndsWith("txt4"))
                    _templatePlaceholderList.Add(VsManager.GetProjectItemFullPath(pi));
            }

            // clean up
            bool hasDefaultItems = groupedFileNames.Any(f => string.IsNullOrEmpty(f.ProjectName) && string.IsNullOrEmpty(f.FolderName));
            if (hasDefaultItems == false)
            {
                ProjectSyncPart(templateProjectItem, new List<OutputFile>());
            }
        }

        private static void ProjectSyncPart(ProjectItem templateProjectItem, IEnumerable<OutputFile> keepFileNames)
        {
            var keepFileNameSet = new HashSet<OutputFile>(keepFileNames);
            var projectFiles = new Dictionary<string, ProjectItem>();
            var originalOutput = Path.GetFileNameWithoutExtension(templateProjectItem.FileNames[0]);

            foreach (ProjectItem projectItem in templateProjectItem.ProjectItems)
            {
                projectFiles.Add(projectItem.FileNames[0], projectItem);
            }

            ProgressBarManager.Reset();
            ProgressBarManager.SetTotal((uint)projectFiles.Count, "Removing files");
            RemoveUnusedItems(projectFiles, originalOutput, keepFileNames);

            ProgressBarManager.Reset();
            ProgressBarManager.SetTotal((uint)keepFileNameSet.Count, "Adding files");
            AddMissingItems(keepFileNameSet, projectFiles, templateProjectItem);
        }

        private static void RemoveUnusedItems(Dictionary<string, ProjectItem> projectFiles, string originalOutput, IEnumerable<OutputFile> keepFileNames)
        {
            foreach (var pair in projectFiles)
            {
                bool isNotFound = !keepFileNames.Any(f => f.FileName == pair.Key);
                if (isNotFound == true
                    && !(Path.GetFileNameWithoutExtension(pair.Key) + ".").StartsWith(originalOutput + "."))
                {
                    pair.Value.Delete();
                    OutputPaneManager.WriteToOutputPane($"Deleting {pair.Key}");
                }
                ProgressBarManager.SetProgress();
            }
        }

        private static void AddMissingItems(HashSet<OutputFile> keepFileNameSet, Dictionary<string, ProjectItem> projectFiles, ProjectItem templateProjectItem)
        {
            foreach (var fileName in keepFileNameSet)
            {
                if (!projectFiles.ContainsKey(fileName.FileName))
                {
                    OutputPaneManager.WriteToOutputPane($"Add {fileName.Name}");
                    templateProjectItem.ProjectItems.AddFromFile(fileName.FileName);
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"{fileName.Name} already exists");
                }
                ProgressBarManager.SetProgress();
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
