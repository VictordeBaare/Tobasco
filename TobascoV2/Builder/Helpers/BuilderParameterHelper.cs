using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Constants;
using TobascoV2.Context;

namespace TobascoV2.Builder.Helpers
{
    internal static class BuilderParameterHelper
    {
        internal static Assembly GetAssembly(IDictionary<string, string> commandParameters)
        {
            if (commandParameters.TryGetValue(BuilderConstants.EntityAssembly, out string assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }
            throw new ArgumentNullException(nameof(commandParameters), "Assembly parameter could not be found");
        }

        internal static ITobascoContext GetContext(Assembly assembly, IDictionary<string, string> commandParameters)
        {
            if (commandParameters.TryGetValue(BuilderConstants.EntityContext, out string contextName))
            {
                Type type = assembly.GetType(contextName);
                return (ITobascoContext)Activator.CreateInstance(type);
            }
            throw new ArgumentNullException(nameof(commandParameters), "Context parameter could not be found");
        }

        internal static IEnumerable<string> GetEntities(IDictionary<string, string> commandParameters)
        {
            if (commandParameters.TryGetValue(BuilderConstants.EntityPath, out string entityPath))
            {
                return Directory.GetFiles(entityPath);
            }
            throw new ArgumentNullException(nameof(commandParameters), "Entities path parameter could not be found");
        }
    }
}
