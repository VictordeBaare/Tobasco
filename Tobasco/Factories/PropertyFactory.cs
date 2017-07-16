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

        protected virtual Dictionary<string, string> GetParameters(ClassProperty prop)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add(PropertyTemplateConstants.ValueType, prop.GetValueType);
            parameters.Add(PropertyTemplateConstants.PropertyName, prop.Property.Name);

            return parameters;
        }

        protected Template Template =>  _template ?? (_template = new Template());
    }
}
