using System;
using System.Collections.Generic;
using System.Text;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Manager;
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
            classFile.ClassAttributes.Add("[System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Maintainability\", \"CA1506: AvoidExcessiveClassCoupling\", Justification = \"Generated file\")]");
            classFile.Namespaces.AddRange(new[] { "Ninject", "Ninject.Modules" });
            classFile.OwnNamespace = module.FileLocation.GetNamespace;
            LoadMethod(classFile);

            return classFile;
        }

        private void LoadMethod(ClassFile classfile)
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs("[System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Maintainability\", \"CA1506: AvoidExcessiveClassCoupling\", Justification = \"Generated file\")]", 0);
            builder.AppendLineWithTabs("public override void Load()", 2);
            builder.AppendLineWithTabs("{", 2);
            foreach (var handler in EntityHandlers)
            {
                var repositoryBuilder = handler.GetRepositoryBuilder;
                if (repositoryBuilder != null && handler.GetRepository != null && handler.GetRepository.Generate)
                {
                    builder.AppendLineWithTabs($"Bind<{repositoryBuilder.GetRepositoryInterfaceName}>().To<{repositoryBuilder.GetRepositoryName}>();", 3);
                    builder.AppendLineWithTabs($"Bind<IGenericRepository<{handler.Entity.Name}>>().To<GenericRepository<{handler.Entity.Name}>>();", 3);
                    classfile.Namespaces.Add(handler.GetRepository.FileLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                    classfile.Namespaces.Add(handler.GetEntityLocationOnId(handler.GetRepository.EntityId).FileLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                    classfile.Namespaces.Add(handler.GetRepository.InterfaceLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                }
            }
            builder.AppendLineWithTabs("}", 2);
            classfile.Methods.Add(builder.ToString());            
        }        
    }
}