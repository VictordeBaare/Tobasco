using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TobascoV2.Constants;
using TobascoV2.Context;

namespace TobascoV2.Builder
{
    public class EntityBuilder : IBuilder
    {
        public void Build(IDictionary<string, string> keyValuePairs)
        {
            Assembly assembly = null;
            TobascoContext context = null;
            string entities = string.Empty;
            var appRoot = string.Empty;

            if (keyValuePairs.TryGetValue(BuilderConstants.EntityAssembly, out string assemblyPath))
            {
                assembly = Assembly.LoadFrom(assemblyPath);
                var pad = Path.GetDirectoryName(assembly.GetName().CodeBase);
                Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                appRoot = Directory.GetParent(appPathMatcher.Match(pad).Value).FullName;
            }

            if (keyValuePairs.TryGetValue(BuilderConstants.EntityContext, out string contextName))
            {
                Type type = assembly.GetType(contextName);
                context = (TobascoContext)Activator.CreateInstance(type);
            }            

            if(keyValuePairs.TryGetValue(BuilderConstants.EntityPath, out string entityPath))
            {
                entities = entityPath;
            }
            
            
        }
    }
}
