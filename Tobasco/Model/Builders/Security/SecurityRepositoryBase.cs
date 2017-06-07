using System.Collections.Generic;
using System.Linq;
using Tobasco.Enums;
using Tobasco.Extensions;

namespace Tobasco.Model.Builders.Security
{
    public abstract class SecurityRepositoryBase
    {
        protected readonly EntityHandler Entity;
        protected readonly Dac Dac;

        protected SecurityRepositoryBase(EntityHandler entity, Dac dac)
        {
            Entity = entity;
            Dac = dac;
        }

        public virtual string GetEntityName => Entity.Entity.Name;

        public virtual string GetRepositoryName => $"{GetEntityName}Repository";

        public virtual string GetRepositoryInterfaceName => $"I{GetEntityName}Repository";

        protected abstract IEnumerable<Property> GetChildProperties { get; }

        protected abstract IEnumerable<Property> GetChildCollectionProperties { get; }

        protected abstract IEnumerable<string> GetRepositoryNamespaces { get; }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}