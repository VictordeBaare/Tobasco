using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tobasco.FileBuilder;
using Tobasco.Manager;

namespace Tobasco.Generation
{
    internal class FileProcessor
    {
        private string _placeholderKey = "{0}-{1}";
        private readonly DTE _dte;
        private readonly string _templateFile;
        private readonly string _defaultPath;
        private readonly ProjectItem _templateProjectItem;
        private Dictionary<string, ProjectItem> _placeholders;
        private Dictionary<string, HashSet<string>> _processedFileNames;

        internal FileProcessor(DTE dte, string templateFile, ProjectItem templateProjectItem)
        {
            _dte = dte;
            _templateFile = templateFile;
            _defaultPath = Path.GetDirectoryName(_templateFile);
            _templateProjectItem = templateProjectItem;
            _placeholders = new Dictionary<string, ProjectItem>();
            _processedFileNames = new Dictionary<string, HashSet<string>>();
        }

        internal void ProcessClassFile(OutputFile outputFile)
        {
            var outputPath = VsManager.GetOutputPath(_dte, outputFile, _defaultPath);
            outputFile.FileName = Path.Combine(outputPath, outputFile.FullName);

            ProjectItem placeHolder = GetPlaceholder(outputFile);

            if (!FileExist(outputFile) || IsFileContentDifferent(outputFile))
            {
                ProcessFile(outputFile, placeHolder);
            }
            RegisterProcessedFile(outputFile);
        }
        
        internal void RemoveUnusedTemplateFiles()
        {
            var solutionItems = VsManager.GetAllSolutionItems(_dte);
            var activeTemplateFullNames = _placeholders.Values.Select(x => VsManager.GetProjectItemFullPath(x));
            var allHelperTemplateFullNames = solutionItems.Where(p => p.Name == VsManager.GetTemplatePlaceholderName(_templateProjectItem))
                .Select(VsManager.GetProjectItemFullPath);

            var delta = allHelperTemplateFullNames.Except(activeTemplateFullNames);

            var dirtyHelperTemplates = solutionItems.Where(p => delta.Contains(VsManager.GetProjectItemFullPath(p)));

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

        internal void CleanTemplateFiles()
        {
            foreach (KeyValuePair<string, ProjectItem> item in _placeholders)
            {
                var processedFileNames = _processedFileNames[item.Key];
                foreach(ProjectItem projectItem in item.Value.ProjectItems)
                {
                    if (!processedFileNames.Contains(projectItem.Name))
                    {
                        projectItem.Delete();
                    }
                }
            }
        }

        private void RegisterProcessedFile(OutputFile outputFile)
        {
            var key = GetKey(outputFile);
            if (_processedFileNames.ContainsKey(key))
            {
                _processedFileNames[key].Add(outputFile.FullName);
            }
            else
            {
                _processedFileNames.Add(key, new HashSet<string>() { outputFile.FullName });
            }
        }

        private void ProcessFile(OutputFile outputFile, ProjectItem placeholder)
        {
            CheckoutFileIfRequired(outputFile.FileName);
            File.WriteAllText(outputFile.FileName, outputFile.BuildContent(), Encoding.UTF8);
            placeholder.ProjectItems.AddFromFile(outputFile.FileName);
        }

        private void CheckoutFileIfRequired(string fileName)
        {
            if (_dte.SourceControl != null && _dte.SourceControl.IsItemUnderSCC(fileName) && !_dte.SourceControl.IsItemCheckedOut(fileName))
            {
                _dte.SourceControl.CheckOutItem(fileName);
            }            
        }

        private ProjectItem GetPlaceholder(OutputFile outputFile)
        {
            var key = GetKey(outputFile);
            if (_placeholders.ContainsKey(key))
            {
                return _placeholders[key];
            }
            else
            {
                ProjectItem pi = VsManager.GetTemplateProjectItem(_templateProjectItem.DTE, outputFile, _templateProjectItem);
                _placeholders.Add(key, pi);
                return pi;
            }
        }

        private string GetKey(OutputFile outputFile)
        {
            return string.Format(_placeholderKey, outputFile.ProjectName, outputFile.FolderName);
        }

        private bool IsFileContentDifferent(OutputFile file)
        {
            return File.ReadAllText(file.FileName) != file.BuildContent();
        }

        private bool FileExist(OutputFile file)
        {
            return File.Exists(file.FileName);
        }
    }
}
