using System;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class HistorieViewBuilder : DatabaseHelper
    {
        public HistorieViewBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? GetTemplateUid : GetTemplate);
            template.Fill(TemplateParameters());
            return template.GetText;
        }

        private TemplateParameter TemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpPropertyNames, string.Join("," + Environment.NewLine, GetSqlParameterNames()) + ",");
            return parameters;
        }

        protected virtual string GetTemplate => SqlResources.SqlHistorieView;
        protected virtual string GetTemplateUid => SqlResources.SqlHistorieViewWithUid;
    }
}
