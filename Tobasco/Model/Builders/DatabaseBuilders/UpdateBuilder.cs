using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class UpdateBuilder : DatabaseHelper
    {
        public UpdateBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Resources.SqlUpdateStp);
            template.Fill(UpdateTemplateParameters());

            return template.GetText;
        }

        private TemplateParameter UpdateTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(Resources.TableName, Name);
            parameters.Add(Resources.StpParameter, GetSqlParameters());
            parameters.Add(Resources.StpParameterName, GetSqlParameterNames("@"));
            parameters.Add(Resources.StpPropertyNames, GetSqlParameterNames(""));
            parameters.Add(Resources.UpdateSetTableParemeters, GetSqlUpdateParameters());

            return parameters;
        }

        private List<string> GetSqlUpdateParameters()
        {
            var list = new List<string>();

            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                list.Add($"{Name}.{selectSqlProperty.SelectSqlParameterNaam} = @{selectSqlProperty.SelectSqlParameterNaam},");
            }

            return list;
        }

    }
}
