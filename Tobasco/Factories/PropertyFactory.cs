using System.Text;
using Tobasco.Enums;
using Tobasco.Model.Properties;

namespace Tobasco.Factories
{
    public class PropertyFactory
    {
        private StringBuilder _propertyBuilder;

        public virtual string GetProperty(ClassProperty prop, bool generateRules)
        {
            switch (prop.Property.DataType.Datatype)
            {
                case Datatype.ChildCollection:
                    PropertyBuilder.Append("public List<" + prop.GetValueType + "> " + prop.Property.Name + " { get; } = new List<" + prop.GetValueType + ">();");
                    break;
                case Datatype.ReadOnlyGuid:
                    PropertyBuilder.Append("public " + prop.GetValueType + " " + prop.Property.Name + " { get; private set; }");
                    break;
                default:
                    PropertyBuilder.Append("public " + prop.GetValueType + " " + prop.Property.Name + " { get; set; }");
                    break;
            }
            return PropertyBuilder.ToString();
        }

        protected StringBuilder PropertyBuilder => _propertyBuilder ?? (_propertyBuilder = new StringBuilder());
    }
}
