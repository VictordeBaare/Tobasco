using TobascoV2.Context.Base;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding.Helpers
{
    public static class FileLocationHelper
    {
        public static string GetFileLocation(FileLocation fileLocation, FileLocation contextFileLocation)
        {
            var entityPath = fileLocation.GetPath();
            if (string.IsNullOrEmpty(entityPath))
            {
                return contextFileLocation.GetPath();
            }
            return entityPath;
        }
    }
}
