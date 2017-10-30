using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Properties;
using Tobasco.Templates;

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
            classFile.Namespaces.AddRange(module.Namespaces.Select(x => x.Value));
            classFile.OwnNamespace = module.FileLocation.GetNamespace;
            LoadMethod(classFile);
            AddNinjectBinder(classFile, module);
            return classFile;
        }

        private void LoadMethod(ClassFile classfile)
        {
            var template = new Template();
            template.SetTemplate(Resources.DependencyInjection);

            template.Fill(GetParameters(classfile));
            
            classfile.Methods.Add(template.GetText);            
        }

        private void AddNinjectBinder(ClassFile classfile, Module module)
        {
            var template = new Template();
            template.SetTemplate(Resources.NinjectBinder);
            var parameters = new TemplateParameter();
            parameters.Add("BindingScope", ResolveBindingExtension(module, classfile));
            template.Fill(parameters);
            classfile.Methods.Add(template.GetText);
        }

        private TemplateParameter GetParameters(ClassFile classfile)
        {
            var parameters = new TemplateParameter();
            var bindings = new List<string>();

            foreach (var handler in EntityHandlers)
            {
                var repositoryBuilder = handler.GetRepositoryBuilder;
                if (repositoryBuilder != null && handler.GetRepository != null && handler.GetRepository.Generate)
                {
                    bindings.Add(BuildBinding(repositoryBuilder.GetRepositoryInterfaceName, repositoryBuilder.GetRepositoryName));
                    bindings.Add(BuildBinding($"IGenericRepository<{handler.Entity.Name}>", $"GenericRepository<{handler.Entity.Name}>"));
                    classfile.Namespaces.Add(handler.GetRepository.FileLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                    classfile.Namespaces.Add(handler.GetEntityLocationOnId(handler.GetRepository.EntityId).FileLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                    classfile.Namespaces.Add(handler.GetRepository.InterfaceLocation.GetProjectLocation, s => !classfile.Namespaces.Contains(s));
                }
            }

            parameters.Add(DependencyInjectionConstants.Bindings, string.Join(Environment.NewLine, bindings));
            return parameters;
        }

        protected virtual string BuildBinding(string interfaceName, string className)
        {
            return $"NinjectBinder<{interfaceName},{className}>();";
        }
    }
}