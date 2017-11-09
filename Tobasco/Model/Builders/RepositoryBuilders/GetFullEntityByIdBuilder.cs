using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.RepositoryBuilders
{
    public class GetFullEntityByIdBuilder : GetFullEntityByIdHelper
    {
        public GetFullEntityByIdBuilder(EntityHandler entity, Repository repository) : base(entity, repository)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(RepositoryResources.RepositoryGetFullObjectById);
            template.Fill(GetParatmers());
            return template.GetText;
        }

    }
}
