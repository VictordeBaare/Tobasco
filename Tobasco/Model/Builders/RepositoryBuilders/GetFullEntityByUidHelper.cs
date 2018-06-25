using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.RepositoryBuilders
{
    public class GetFullEntityByUidHelper : RepositoryHelper
    {
        public GetFullEntityByUidHelper(EntityHandler entity, Repository repository) : base(entity, repository)
        {
        }

        protected virtual TemplateParameter GetParatmers()
        {
            var parameters = new TemplateParameter();
            parameters.Add(RepositoryBuilderConstants.EntityName, GetEntityName);
            parameters.Add(RepositoryBuilderConstants.GetByUIdStp, $"[dbo].[{Entity.Entity.Name}_GetFullByUId]");
            return parameters;
        }
    }
}
