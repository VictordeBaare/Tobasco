using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model.Builders.Base
{
    public abstract class DependencyInjectionBuilderBase
    {
        protected readonly IEnumerable<EntityHandler> EntityHandlers;

        protected DependencyInjectionBuilderBase(IEnumerable<EntityHandler> entityHandlers)
        {
            EntityHandlers = entityHandlers;
        }

        public abstract FileBuilder.OutputFile Build(Module module);
    }
}
