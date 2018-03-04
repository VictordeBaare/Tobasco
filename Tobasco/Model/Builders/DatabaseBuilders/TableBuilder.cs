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
            template.SetTemplate(Entity.GenerateReadonlyGuid ? GetTemplateUid : GetTemplate);
            template.Fill(GetParameters());
            return template.GetText;
        }

        protected virtual string GetTemplate => SqlResources.SqlTable;
        protected virtual string GetTemplateUid => SqlResources.SqlTableWithUid;
    }
}
