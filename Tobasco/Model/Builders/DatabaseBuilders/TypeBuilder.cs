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
    public class TypeBuilder : DatabaseHelper
    {
        public TypeBuilder(Entity entity, Database database, EntityInformation information) : base(entity, database, information)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(SqlResources.SqlMergeType);
            template.Fill(GetParameters());

            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Entity.Name);
            parameters.Add(SqlConstants.TableProperties, GetTableProperties());

            return parameters;
        }
    }
}
