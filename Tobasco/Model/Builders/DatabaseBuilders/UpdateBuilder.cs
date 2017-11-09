using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
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
            template.SetTemplate(SqlResources.SqlUpdateStp);
            template.Fill(UpdateTemplateParameters());

            return template.GetText;
        }

        private TemplateParameter UpdateTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpParameter, GetSqlParameters());
            parameters.Add(SqlConstants.StpParameterName, GetSqlParameterNames("@"));
            parameters.Add(SqlConstants.StpPropertyNames, GetSqlParameterNames(""));
            parameters.Add(SqlConstants.UpdateSetTableParemeters, GetSqlUpdateParameters());

            return parameters;
        }  
    }
}
