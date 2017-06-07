using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Extensions;

namespace Tobasco.FileBuilder
{
    public class Constructor
    {
        public List<FieldWithParameter> ParameterWithField { get; } = new List<FieldWithParameter>();

        public List<TypeWithName> Parameters { get; } = new List<TypeWithName>();

        public List<TypeWithName> Fields { get; } = new List<TypeWithName>(); 

        public List<string> CustomImplementation { get; } = new List<string>();

        public string Build(string name)
        {
            var builder = new StringBuilder();
            foreach (TypeWithName typeWithName in Fields)
            {
                builder.AppendLineWithTabs($"private {typeWithName.Type} {typeWithName.Name};", 2);
            }
            foreach (var parameterWithField in ParameterWithField)
            {
                builder.AppendLineWithTabs($"private readonly {parameterWithField.Field.Type} {parameterWithField.Field.Name};", 2);
            }
            builder.AppendLineWithTabs($"public {name}({ConstructorParameters(ParameterWithField.Select(x => x.Parameter).Concat(Parameters))})", 2);
            builder.AppendLineWithTabs("{", 2);
            foreach (var parameterWithField in ParameterWithField)
            {
                builder.AppendLineWithTabs($"{parameterWithField.Field.Name} = {parameterWithField.Parameter.Name};", 3);                
            }
            foreach (var customImpl in CustomImplementation)
            {
                builder.AppendLineWithTabs($"{customImpl};", 3);
            }
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        public bool ShouldBeBuild()
        {
            return ParameterWithField.Any() || Parameters.Any() || CustomImplementation.Any();
        }

        private string ConstructorParameters(IEnumerable<TypeWithName> parameters)
        {
            return parameters.Select(x => $"{x.Type} {x.Name}").Join(", ");
        }
    }
}
