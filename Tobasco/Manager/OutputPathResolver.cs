using System.Collections.Generic;

namespace Tobasco.Manager
{
    /// <summary>
    /// Resolves the output path for a given project and folder name.
    /// Performance is improved by caching the results of slow DTE calls in a local cache.
    /// </summary>
    internal class OutputPathResolver
    {
        private readonly VsManager _vsManager;
        private readonly Dictionary<string, string> lookup = new Dictionary<string, string>();

        public OutputPathResolver(VsManager vsManager)
        {
            _vsManager = vsManager;
        }

        public string GetOutputPath(string projectName, string folderName, string defaultPath)
        {
            var key = ToKey(projectName, folderName);
            if (lookup.ContainsKey(key))
            {
                return lookup[key];
            }
            var value = _vsManager.GetOutputPath(projectName, folderName, defaultPath);
            lookup[key] = value;
            return value;
        }

        private string ToKey(string projectName, string folderName)
        {
            return $"{projectName}:{folderName}";
        }
    }
}