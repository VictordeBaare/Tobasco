using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.Properties;

namespace Tobasco.FileBuilder
{
    public class InterfaceFile : OutputFile
    {
        public override FileType Type => FileType.Interface;

        public override string BuildContent()
        {
            Template.SetTemplate(Resources.InterfaceFile);
            TemplateParameters.Add(FileConstants.Namespaces, Namespaces.Select(x => $"using {x};"));
            TemplateParameters.Add(FileConstants.OwnNamespace, OwnNamespace);
            TemplateParameters.Add(FileConstants.Type, Type.GetDescription());
            TemplateParameters.Add(FileConstants.InterfaceName, Name);
            TemplateParameters.Add(FileConstants.Extension, NameExtension);
            TemplateParameters.Add(FileConstants.Properties, Properties);
            TemplateParameters.Add(FileConstants.Methods, Methods);
            TemplateParameters.Add(FileConstants.Attributes, ClassAttributes);
            Template.Fill(TemplateParameters);

            return Template.GetText;
        }
    }
}
