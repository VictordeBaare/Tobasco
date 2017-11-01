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
    public class InsertBuilder : DatabaseHelper
    {
        public InsertBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? SqlResources.SqlInsertStpWithUid : SqlResources.SqlInsertStp);
            template.Fill(InsertTemplateParameters());

            return template.GetText;
        }

        private TemplateParameter InsertTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpParameter, GetSqlParameters());
            parameters.Add(SqlConstants.StpParameterName, GetSqlParameterNames("@"));
            parameters.Add(SqlConstants.StpPropertyNames, GetSqlParameterNames(""));
            return parameters;
        }

    }
}
