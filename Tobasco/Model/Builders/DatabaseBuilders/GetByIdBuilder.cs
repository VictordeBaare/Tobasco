using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class GetByIdBuilder : GetByIdHelper
    {
        public GetByIdBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        protected override string Template => SqlResources.GetFullObjectByEntity;

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Template);
            template.Fill(GetParameters());
            return template.CleanText;
        }
    }
}
