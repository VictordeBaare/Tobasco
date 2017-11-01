using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class GetByReferenceIdBuilder : GetByIdHelper
    {
        public GetByReferenceIdBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build(Property reference)
        {
            var template = new Template();
            template.SetTemplate(SqlResources.SqlGetByReferenceId);
            template.Fill(GetReferenceParameters(reference));
            return template.GetText;
        }

        private TemplateParameter GetReferenceParameters(Property reference)
        {
            var parameters = GetParameters();
            parameters.Add(SqlConstants.ReferenceName, reference.Name);
            return parameters;
        }
    }
}
