using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Extensions;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.FileBuilder
{
    public class Constructor
    {
        public void AddFieldWithParameter(Field field, TypeWithName parameter)
        {
            Fields.Add(field);
            Parameters.Add(parameter);
            ParameterWithField.Add(new FieldWithParameter { Field = field, Parameter = parameter });
        }        

        public List<FieldWithParameter> ParameterWithField { get; } = new List<FieldWithParameter>();

        public List<TypeWithName> Parameters { get; } = new List<TypeWithName>();

        public List<Field> Fields { get; } = new List<Field>(); 

        public List<string> CustomImplementation { get; } = new List<string>();

        private TemplateParameter GetParameters(string name)
        {
            var parameters = new TemplateParameter();

            parameters.Add(ConstructorConstants.ConstructorFields, Fields.Select(x => x.Build()));
            parameters.Add(ConstructorConstants.ConstructorParameters, ConstructorParameters(Parameters));
            parameters.Add(ConstructorConstants.FieldWithParameter, ParameterWithField.Select(x => x.Build()));
            parameters.Add(ConstructorConstants.CustomImplementation, CustomImplementation);
            parameters.Add(ConstructorConstants.Name, name);
            
            return parameters;
        }

        public string Build(string name)
        {
            if (ShouldBeBuild())
            {
                var template = new Template();
                template.SetTemplate(Resources.Constructor);
                template.Fill(GetParameters(name));

                return template.GetText;
            }
            return string.Empty;
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
