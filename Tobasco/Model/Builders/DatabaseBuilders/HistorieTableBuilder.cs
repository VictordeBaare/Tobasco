using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class HistorieTableBuilder : TableHelper
    {
        public HistorieTableBuilder(Entity entity, Database database, EntityInformation information) : base(entity, database, information)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? GetTemplateUid : GetTemplate);
            template.Fill(GetParameters());
            return template.GetText;
        }

        protected virtual string GetTemplate => SqlResources.SqlHistorieTable;
        protected virtual string GetTemplateUid => SqlResources.SqlHistorieTableWithUid;
    }
}
