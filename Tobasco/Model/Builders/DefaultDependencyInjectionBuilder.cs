using System.Collections.Generic;
using System.Text;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;

namespace Tobasco.Model.Builders
{
    public class DefaultDependencyInjectionBuilder : DependencyInjectionBuilderBase
    {

        public DefaultDependencyInjectionBuilder(IEnumerable<EntityHandler> entityHandlers) : base(entityHandlers)
        {
            
        }

        public override FileBuilder.OutputFile Build(Module module)
        {
            var classFile = FileManager.StartNewClassFile(module.Name + "Module", module.FileLocation.Project, module.FileLocation.Folder);
            classFile.BaseClass = ": NinjectModule";
            classFile.Namespaces.AddRange(new[] { "Ninject", "Ninject.Modules" });
            classFile.OwnNamespace = module.FileLocation.GetNamespace;
            LoadMethod(classFile);

            return classFile;
        }

        private void LoadMethod(ClassFile classfile)
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("public override void Load()", 0);
            builder.AppendLineWithTabs("{", 2);
            foreach (var handler in EntityHandlers)
            {
                var repositoryBuilder = handler.GetRepositoryBuilder;
                builder.AppendLineWithTabs($"Bind<{repositoryBuilder.GetRepositoryInterfaceName}>().To<{repositoryBuilder.GetRepositoryName}>();", 3);
                classfile.Namespaces.Add(handler.GetRepository.FileLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                classfile.Namespaces.Add(handler.GetRepository.InterfaceLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
            }
            builder.AppendLineWithTabs("}", 2);
            classfile.Methods.Add(builder.ToString());            
        }        
    }
}