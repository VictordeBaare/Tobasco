using Tobasco.FileBuilder;

namespace Tobasco.Model.Builders.Base
{
    public abstract class DependencyInjectionBuilderBase
    {
        protected DependencyInjectionBuilderBase()
        {

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

        public abstract OutputFile Build(Module module);
    }
}
