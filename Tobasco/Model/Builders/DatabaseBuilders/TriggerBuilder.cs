﻿using System.Text;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class TriggerBuilder : DatabaseHelper
    {
        public TriggerBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var builder = new StringBuilder();
            builder.AppendLine(BuildUpdateTrigger());
            builder.AppendLine("GO");
            builder.AppendLine(BuildDeleteTrigger());
            return builder.ToString();
        }

        private string BuildDeleteTrigger()
        {
            var template = new Template();
            template.SetTemplate(SqlResources.SqlTriggerDelete);
            template.Fill(GetParameters());
            return template.GetText;
        }

        private string BuildUpdateTrigger()
        {
            var template = new Template();
            template.SetTemplate(SqlResources.SqlTriggerUpdate);
            template.Fill(GetParameters());
            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpParameter, GetSqlParameters());
            parameters.Add(SqlConstants.StpParameterName, GetSqlParameterNames("@"));
            parameters.Add(SqlConstants.StpPropertyNames, GetSqlParameterNames(""));
            parameters.Add(SqlConstants.StpDeletetedPropertyNames, GetSqlParameterNames("Deleted."));
            return parameters;
        }
    }
}