using System.Collections.Generic;
using System.Linq;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.Manager;
using Tobasco.Model.Builders.RepositoryBuilders;

namespace Tobasco.Model.Builders.Base
{
    public abstract class RepositoryBuilderBase : RepositoryHelper
    {
        public RepositoryBuilderBase(EntityHandler entity, Repository repository) : base(entity, repository)
        {
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}