using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Extensions;

namespace Tobasco.Model.Builders.Base
{
    public abstract class MapperBuilderBase
    {
        protected readonly EntityHandler EntityHandler;

        protected MapperBuilderBase(EntityHandler entity)
        {
            EntityHandler = entity;
        }

        public virtual string MapperName => $"{EntityHandler.Entity.Name}Mapper";

        public virtual string SelectMapperParameter => $"{EntityHandler.Entity.Name.FirstCharToLower()}Mapper";

        public virtual string SelectMapperInterface => $"I{EntityHandler.Entity.Name}Mapper";

        public abstract IEnumerable<FileBuilder.OutputFile> Build(Mapper mapper);
    }
}
