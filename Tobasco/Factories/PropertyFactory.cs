using System.Text;
using Tobasco.Enums;
using Tobasco.Model.Properties;
using Tobasco.Templates;
using System.Collections.Generic;
using Tobasco.Properties;

namespace Tobasco.Factories
{
    public class PropertyFactory
    {
        private Template _template;

        public virtual string GetProperty(ClassProperty prop, bool generateRules)
        {
            Template.SetTemplate(GetPropertyTemplate(prop.Property.DataType.Datatype));
            Template.Fill(GetParameters(prop));
            return Template.GetText;
        }

        protected virtual string GetPropertyTemplate(Datatype datatype)
        {
            switch (datatype)
            {
                case Datatype.ChildCollection:
                    return Resources.PropertyChildCollection;
                case Datatype.ReadOnlyGuid:
                    return Resources.PropertyReadonlyGuid;
                default:
                    return Resources.PropertyDefault;
            }
        }

        protected virtual TemplateParameter GetParameters(ClassProperty prop)
        {
            var parameters = new TemplateParameter();

            parameters.Add(PropertyTemplateConstants.ValueType, prop.GetValueType);
            parameters.Add(PropertyTemplateConstants.PropertyName, prop.Property.Name);
            parameters.Add(PropertyTemplateConstants.PropertyParameterName, prop.Property.Name.ToLower());
            parameters.Add(PropertyTemplateConstants.BusinessRules, prop.CalcRules);
            parameters.Add(PropertyTemplateConstants.Description, prop.GetDescription);

            return parameters;
        }

        protected Template Template =>  _template ?? (_template = new Template());
    }
}
