using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;
using Tobasco.Extensions;

namespace Tobasco.FileBuilder
{
    public class InterfaceFile : OutputFile
    {
        public override FileType Type => FileType.Interface;

        public override string BuildContent()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var nspace in Namespaces)
            {
                builder.AppendLineWithTabs($"using {nspace};", 0);
            }
            builder.Append(Environment.NewLine);
            builder.AppendLineWithTabs($"{OwnNamespace}", 0);
            builder.AppendLineWithTabs("{", 0);
            builder.AppendLineWithTabs($"public partial {Type.GetDescription()} {Name}{NameExtension}", 1);
            builder.AppendLineWithTabs("{", 1);
            foreach (var prop in Properties)
            {
                builder.AppendLineWithTabs(prop, 2);
            }
            foreach (var method in Methods)
            {
                builder.AppendLineWithTabs(method, 2);
            }
            builder.AppendLineWithTabs("}", 1);
            builder.AppendLineWithTabs("}", 0);

            return builder.ToString();
        }
    }
}
