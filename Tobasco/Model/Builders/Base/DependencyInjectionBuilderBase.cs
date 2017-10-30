using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.FileBuilder;

namespace Tobasco.Model.Builders.Base
{
    public abstract class DependencyInjectionBuilderBase
    {
        protected readonly IEnumerable<EntityHandler> EntityHandlers;

        protected DependencyInjectionBuilderBase(IEnumerable<EntityHandler> entityHandlers)
        {
            EntityHandlers = entityHandlers;
        }

        protected string ResolveBindingExtension(Module module, ClassFile classfile)
        {
            switch (module.Scope)
            {
                case Enums.ScopeType.InRequestScope:
                    {
                        classfile.Namespaces.Add("Ninject.Web.Common");
                        return ".InRequestScope()";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public abstract FileBuilder.OutputFile Build(Module module);
    }
}
