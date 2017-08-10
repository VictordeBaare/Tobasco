using System;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.Properties;

namespace Tobasco.FileBuilder
{
    public class ClassFile : OutputFile
    {
        public override FileType Type => FileType.Class;

        public Constructor Constructor { get; } = new Constructor();
        
        public bool IsAbstract { get; set; }

        public override string BuildContent()
        {
            Template.SetTemplate(Resources.ClassFile);
            TemplateParameters.Add(FileConstants.Namespaces, Namespaces.Select(x => $"using {x};"));
            TemplateParameters.Add(FileConstants.OwnNamespace, OwnNamespace);
            TemplateParameters.Add(FileConstants.Type, Type.GetDescription());
            TemplateParameters.Add(FileConstants.Abstract, IsAbstract ? "abstract" : "");
            TemplateParameters.Add(FileConstants.ClassName, Name);
            TemplateParameters.Add(FileConstants.Extension, NameExtension);
            TemplateParameters.Add(FileConstants.BaseClass, BaseClass);
            TemplateParameters.Add(FileConstants.Constructor, Constructor.Build(Name));
            TemplateParameters.Add(FileConstants.Attributes, ClassAttributes);
            TemplateParameters.Add(FileConstants.Properties, Properties);
            TemplateParameters.Add(FileConstants.Methods, Methods);
            Template.Fill(TemplateParameters);

            return Template.GetText;
        }
    }
}
