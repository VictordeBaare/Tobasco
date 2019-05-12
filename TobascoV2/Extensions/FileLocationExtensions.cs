using TobascoV2.Context;
using TobascoV2.Context.Base;

namespace TobascoV2.Extensions
{
    public static class FileLocationExtensions
    {
        public static string GetPath(this FileLocation fileLocation)
        {
            if (string.IsNullOrEmpty(fileLocation.Folder))
            {
                if (string.IsNullOrEmpty(fileLocation.Project))
                {
                    return string.Empty;
                }
                return fileLocation.Project;
            }
            else
            {
                return $"{fileLocation.Project}//{fileLocation.Folder}";
            }
        }

        public static string GetNamespace(this FileLocation fileLocation)
        {
            if (string.IsNullOrEmpty(fileLocation.Folder))
            {
                if (string.IsNullOrEmpty(fileLocation.Project))
                {
                    return string.Empty;
                }
                return fileLocation.Project;
            }
            else
            {
                return $"{fileLocation.Project}.{fileLocation.Folder}";
            }
        }
    }
}
