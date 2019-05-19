using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TobascoV2.Context;
using TobascoV2.Scaffolding;

namespace TobascoV2.Builder
{
    public class DatabaseBuilder : IBuilder
    {
        private static XmlSerializer _entityserializer = new XmlSerializer(typeof(XmlEntity));

        public void Build(IDictionary<string, string> commandParameters)
        {
            throw new NotImplementedException();
        }

        private void ScaffoldXmls(IEnumerable<string> entities, ITobascoContext tobascoContext, string appRoot)
        {
            foreach (var entity in entities)
            {
                using (var reader = new StreamReader(entity))
                {
                    var item = (XmlEntity)_entityserializer.Deserialize(reader);

                    var scaffolder = new DapperTableScaffolder();
                    scaffolder.Scaffold(item, tobascoContext, appRoot);
                    var scaffolderStp = new DapperStoredProcedureScaffolder();
                    scaffolderStp.Scaffold(item, tobascoContext, appRoot);
                }
            }
        }
    }
}
