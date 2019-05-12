using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TobascoV2.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetAppRoot(this Assembly assembly)
        {
            var pad = Path.GetDirectoryName(assembly.GetName().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            return Directory.GetParent(appPathMatcher.Match(pad).Value).FullName;
        }
    }
}
