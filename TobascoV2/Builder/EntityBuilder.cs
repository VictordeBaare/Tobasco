using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TobascoV2.Constants;
using TobascoV2.Context;
using TobascoV2.Scaffolding;

namespace TobascoV2.Builder
{
    public class EntityBuilder : IBuilder
    {
        private static XmlSerializer _entityserializer = new XmlSerializer(typeof(EntityContext));

        public void Build(IDictionary<string, string> keyValuePairs)
        {
            Assembly assembly = null;
            TobascoContext context = null;
            List<string> entities = new List<string>();
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
                entities.AddRange(Directory.GetFiles(entityPath));
            }

            ScaffoldXmls(entities, context, appRoot);
        }

        private void ScaffoldXmls(List<string> entities, TobascoContext tobascoContext, string appRoot)
        {
            foreach(var entity in entities)
            {
                using (var reader = new StreamReader(entity))
                {
                    var entityScaffolder = new EntityScaffolder();
                    entityScaffolder.Scaffold((EntityContext)_entityserializer.Deserialize(reader), tobascoContext, appRoot);
                }
            }
        }
    }
}
