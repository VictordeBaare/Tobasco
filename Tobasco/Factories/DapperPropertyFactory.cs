using System;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.Model.Properties;

namespace Tobasco.Factories
{
    public class DapperPropertyFactory : PropertyFactory
    {
        public override string GetProperty(ClassProperty prop, bool generateRules)
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
                    PropertyBuilder.Append("private " + prop.GetValueType + " _" + prop.Property.Name.ToLower() + ";");
                    PropertyBuilder.Append(Environment.NewLine);

                    if (generateRules)
                    {
                        foreach (string bepaalBusinessRule in prop.CalcRules)
                        {
                            PropertyBuilder.AppendWithTabs(bepaalBusinessRule, 2);
                            PropertyBuilder.Append(Environment.NewLine);
                        }
                    }

                    PropertyBuilder.AppendWithTabs("public " + prop.GetValueType + " " + prop.Property.Name, 2);
                    PropertyBuilder.Append(Environment.NewLine);
                    PropertyBuilder.AppendWithTabs("{", 2);
                    PropertyBuilder.Append(Environment.NewLine);
                    PropertyBuilder.AppendWithTabs("get { return" + " _" + prop.Property.Name.ToLower() + "; }", 3);
                    PropertyBuilder.Append(Environment.NewLine);
                    PropertyBuilder.AppendWithTabs("set { SetField(ref " + " _" + prop.Property.Name.ToLower() + ", value, nameof(" + prop.Property.Name + ")); }", 3);
                    PropertyBuilder.Append(Environment.NewLine);
                    PropertyBuilder.AppendWithTabs("}", 2);

                    break;
            }
            return PropertyBuilder.ToString();
        }
    }
}
