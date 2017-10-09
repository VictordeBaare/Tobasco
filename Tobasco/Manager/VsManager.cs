using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tobasco.Manager
{
    internal static class VsManager
    {
        internal static IEnumerable<ProjectItem> GetOutputFilesAsProjectItems(DTE dte, IEnumerable<FileBuilder.OutputFile> outputFiles)
        {
            var fileNames = (from o in outputFiles
                             select Path.GetFileName(o.Name)).ToArray();

            return GetAllSolutionItems(dte).Where(f => fileNames.Contains(f.Name));
        }

        internal static IEnumerable<ProjectItem> GetAllSolutionItems(DTE dte)
        {
            List<ProjectItem> itemList = new List<ProjectItem>();

            foreach (Project item in GetAllProjects(dte))
            {
                if (item?.ProjectItems == null) continue;

                itemList.AddRange(GetAllProjectItemsRecursive(item.ProjectItems));
            }

            return itemList;
        }

        internal static Project GetProject(DTE dte, string projectName)
        {
            return GetAllProjects(dte).First(p => p.Name == projectName);
        }

        internal static IEnumerable<Project> GetAllProjects(DTE dte)
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

        internal static IEnumerable<ProjectItem> GetAllProjectItemsRecursive(ProjectItems projectItems)
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

        internal static string ExecuteVsCommand(DTE dte, ProjectItem item, params string[] command)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            string error = string.Empty;

            try
            {
                Window window = item.Open();
                window.Activate();

                foreach (var cmd in command)
                {
                    if (string.IsNullOrWhiteSpace(cmd) == true)
                    {
                        continue;
                    }

                    DTE2 dte2 = dte as DTE2;
                    dte2.ExecuteCommand(cmd, string.Empty);
                }

                item.Save();
                window.Visible = false;
                // window.Close(); // Ends VS, but not the tab :(
            }
            catch (Exception ex)
            {
                error = string.Format("Error processing file {0} {1}", item.Name, ex.Message);
            }

            return error;
        }

        internal static string GetOutputPath(DTE dte, FileBuilder.OutputFile file, string defaultPath)
        {
            if (string.IsNullOrEmpty(file.ProjectName) == true && string.IsNullOrEmpty(file.FolderName) == true)
            {
                return defaultPath;
            }

            Project prj = null;
            ProjectItem item = null;

            if (string.IsNullOrEmpty(file.ProjectName) == false)
            {
                prj = GetProject(dte, file.ProjectName);
            }

            if (string.IsNullOrEmpty(file.FolderName) == true && prj != null)
            {
                return Path.GetDirectoryName(prj.FullName);
            }
            else if (prj != null && string.IsNullOrEmpty(file.FolderName) == false)
            {
                item = GetAllProjectItemsRecursive(prj.ProjectItems).FirstOrDefault(i => i.Name == file.FolderName);
                if (item == null)
                {
                    throw new ArgumentNullException($"No folder was found with the name {file.FolderName} within the project {file.ProjectName}");
                }
            }
            else if (String.IsNullOrEmpty(file.FolderName) == false)
            {
                item = GetAllProjectItemsRecursive(dte.ActiveDocument.ProjectItem.ContainingProject.ProjectItems).FirstOrDefault(i => i.Name == file.FolderName);
            }

            if (item != null)
            {
                return GetProjectItemFullPath(item);
            }

            return defaultPath;
        }

        internal static string GetTemplatePlaceholderName(ProjectItem item)
        {
            return $"{Path.GetFileNameWithoutExtension(item.Name)}.txt4";
        }

        internal static string GetProjectItemFullPath(ProjectItem item)
        {
            if (item != null && item.Properties != null && item.Properties.Item("FullPath") != null)
            {
                return item.Properties.Item("FullPath").Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        internal static void SetPropertyValue(ProjectItem item, string propertyName, object value)
        {
            Property property = item.Properties.Item(propertyName);
            if (property == null)
            {
                throw new ArgumentException(string.Format("The property {0} was not found.", propertyName));
            }
            else
            {
                property.Value = value;
            }
        }

        internal static ProjectItem GetTemplateProjectItem(DTE dte, FileBuilder.OutputFile file, ProjectItem defaultItem)
        {
            if (string.IsNullOrEmpty(file.ProjectName) && string.IsNullOrEmpty(file.FolderName))
            {
                return defaultItem;
            }

            string templatePlaceholder = GetTemplatePlaceholderName(defaultItem);
            string itemPath = Path.GetDirectoryName(file.FileName);
            string fullName = Path.Combine(itemPath, templatePlaceholder);
            Project prj = null;
            ProjectItem item = null;

            if (string.IsNullOrEmpty(file.ProjectName) == false)
            {
                prj = GetProject(dte, file.ProjectName);
            }

            if (string.IsNullOrEmpty(file.FolderName) == true && prj != null)
            {
                return FindProjectItem(prj.ProjectItems, fullName);
            }
            else if (prj != null && string.IsNullOrEmpty(file.FolderName) == false)
            {
                item = GetAllProjectItemsRecursive(prj.ProjectItems).FirstOrDefault(i => i.Name == file.FolderName);

            }
            else if (string.IsNullOrEmpty(file.FolderName) == false)
            {
                item = GetAllProjectItemsRecursive(dte.ActiveDocument.ProjectItem.ContainingProject.ProjectItems).First(i => i.Name == file.FolderName);
            }

            if (item != null)
            {
                return FindProjectItem(item.ProjectItems, fullName);
            }

            return defaultItem;
        }

        private static ProjectItem FindProjectItem(ProjectItems items, string fullName)
        {
            ProjectItem item = (from i in items.Cast<ProjectItem>()
                                where i.Name == Path.GetFileName(fullName)
                                select i).FirstOrDefault();
            if (item == null)
            {
                File.CreateText(fullName);
                item = items.AddFromFile(fullName);
            }

            return item;
        }
    }
}
