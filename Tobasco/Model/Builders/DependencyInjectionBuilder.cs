using System.Collections.Generic;
using System.Text;
using Tobasco.Extensions;
using Tobasco.FileBuilder;

namespace Tobasco.Model.Builders
{
    public class DependencyInjectionBuilder
    {
        private readonly IEnumerable<EntityHandler> _entityHandlers;

        public DependencyInjectionBuilder(IEnumerable<EntityHandler> entityHandlers)
        {
            _entityHandlers = entityHandlers;
        }

        public FileBuilder.OutputFile Build(Module module)
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
            foreach (var handler in _entityHandlers)
            {
                var repositoryBuilder = handler.GetRepositoryBuilder;
                builder.AppendLineWithTabs($"Bind<{repositoryBuilder.GetRepositoryInterfaceName}>().To<{repositoryBuilder.GetRepositoryName}>();", 3);
                classfile.Namespaces.Add(handler.GetRepository.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                classfile.Namespaces.Add(handler.GetRepository.InterfaceLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
            }
            builder.AppendLineWithTabs("}", 2);
            classfile.Methods.Add(builder.ToString());            
        }        
    }
}