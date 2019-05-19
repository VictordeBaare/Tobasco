using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using TobascoV2.Builder.Helpers;
using TobascoV2.Context;
using TobascoV2.Extensions;
using TobascoV2.Scaffolding;

namespace TobascoV2.Builder
{
    public class EntityBuilder : IBuilder
    {
        private static XmlSerializer _entityserializer = new XmlSerializer(typeof(XmlEntity));

        public void Build(IDictionary<string, string> commandParameters)
        {
            Assembly assembly = BuilderParameterHelper.GetAssembly(commandParameters);
            ITobascoContext context = BuilderParameterHelper.GetContext(assembly, commandParameters);
            IEnumerable<string> entities = BuilderParameterHelper.GetEntities(commandParameters);
            var appRoot = assembly?.GetAppRoot();

            ScaffoldXmls(entities, context, appRoot);
        }

        private void ScaffoldXmls(IEnumerable<string> entities, ITobascoContext tobascoContext, string appRoot)
        {
            foreach(var entity in entities)
            {
                using (var reader = new StreamReader(entity))
                {
                    var entityScaffolder = new EntityScaffolder();
                    entityScaffolder.Scaffold((XmlEntity)_entityserializer.Deserialize(reader), tobascoContext, appRoot);
                }
            }
        }
    }
}
