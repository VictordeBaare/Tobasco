using System;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DeleteBuilder : DatabaseHelper
    {
        public DeleteBuilder(Entity entity, Database database, EntityInformation information) : base(entity, database, information)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(GetTemplate);
            template.Fill(InsertTemplateParameters());
            return template.GetText;
        }

        private TemplateParameter InsertTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpParameter, string.Join("," + Environment.NewLine, GetSqlParameters()));
            parameters.Add(SqlConstants.StpParameterName, string.Join("," + Environment.NewLine, GetSqlValueParameterNames()));
            parameters.Add(SqlConstants.StpPropertyNames, string.Join("," + Environment.NewLine, GetSqlParameterNames()));
            return parameters;
        }

        protected virtual string GetTemplate => SqlResources.SqlDeleteStp;
    }
}
