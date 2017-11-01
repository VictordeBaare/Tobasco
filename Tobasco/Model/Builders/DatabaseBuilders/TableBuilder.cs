using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class TableBuilder : TableHelper
    {
        public TableBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? SqlResources.SqlTableWithUid : SqlResources.SqlTable);
            template.Fill(GetParameters());
            return template.GetText;
        }        
    }
}
