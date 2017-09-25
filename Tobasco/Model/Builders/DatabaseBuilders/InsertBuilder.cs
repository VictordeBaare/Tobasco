using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            template.SetTemplate(Entity.GenerateReadonlyGuid ? Resources.SqlInsertStpWithUid : Resources.SqlInsertStp);
            template.Fill(InsertTemplateParameters());

            return template.GetText;
        }

        private TemplateParameter InsertTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(Resources.TableName, Name);
            parameters.Add(Resources.StpParameter, GetSqlParameters());
            parameters.Add(Resources.StpParameterName, GetSqlParameterNames("@"));
            parameters.Add(Resources.StpPropertyNames, GetSqlParameterNames(""));
            return parameters;
        }

    }
}
