using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.RepositoryBuilders
{
    public class GetFullEntityByUidBuilder : GetFullEntityByUidHelper
    {
        public GetFullEntityByUidBuilder(EntityHandler entity, Repository repository) : base(entity, repository)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(RepositoryResources.RepositoryGetFullObjectByUId);
            template.Fill(GetParatmers());
            return template.GetText;
        }
    }
}
