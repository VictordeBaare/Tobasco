using System;
using TobascoV2.Builder.BusinessRules;
using TobascoV2.Constants;
using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Builder
{
    public class ClassStringBuilder : IndentStringBuilder
    {
        internal ClassStringBuilder() : base(4) { }

        internal ClassStringBuilder AddUsing(string usingNs)
        {
            SetIndent(Indent.UsingNamespace);
            AppendLine($"using {usingNs};");
            return this;
        }

        internal ClassStringBuilder StartNamesspace(string ns)
        {
            AppendLine($"namespace {ns}");
            AppendLine("{");            
            return this;
        }

        internal ClassStringBuilder EndNamespace()
        {
            SetIndent(Indent.Namespace);
            AppendLine("}");
            return this;
        }

        internal ClassStringBuilder StartAbstractClass(string classname, string baseclass)
        {
            SetIndent(Indent.NamespaceBody);
            AppendLine($"public abstract class {classname} : {baseclass}");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder StartAbstractClass(string classname)
        {
            SetIndent(Indent.NamespaceBody);
            AppendLine($"public abstract class {classname}");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder StartClass(string classname, string baseclass)
        {
            SetIndent(Indent.NamespaceBody);
            AppendLine($"public class {classname} : {baseclass}");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder StartClass(string classname)
        {
            SetIndent(Indent.NamespaceBody);
            AppendLine($"public class {classname}");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder EndClass()
        {
            SetIndent(Indent.NamespaceBody);
            return End();
        }

        internal ClassStringBuilder StartConstructor(string name, params string[] parameters)
        {
            SetIndent(Indent.Constructor);
            AppendLine($"public void {name}({string.Join(", ", parameters)})");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder AddConstructorBody(string value)
        {
            SetIndent(Indent.ConstructorBody);
            AppendLine(value);
            return this;
        }

        internal ClassStringBuilder EndConstructor()
        {
            SetIndent(Indent.Constructor);
            return End();
        }

        internal ClassStringBuilder AddProperty(string modifier, XmlProperty xmlProperty)
        {
            SetIndent(Indent.Property);
            AppendLine($"{modifier} {xmlProperty.PropertyType.Name.GetDescription()} {xmlProperty.Name} {{ get; set; }}");
            return this;
        }

        internal ClassStringBuilder AddPropertyWithBusinessRule(string modifier, XmlProperty xmlProperty)
        {
            SetIndent(Indent.Property);
            AddDescription(xmlProperty.Description);
            AddBusinessRule(xmlProperty);
            return AddProperty(modifier, xmlProperty);
        }    

        internal ClassStringBuilder StartProperty(string modifier, XmlPropertyType propertyType, string name)
        {
            SetIndent(Indent.Property);
            AppendLine($"{modifier} {propertyType.Name.GetDescription()} {name}");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder StartProperty(string modifier, string dataType, string name, string description)
        {
            SetIndent(Indent.Property);
            AddDescription(description);
            AppendLine($"{modifier} {dataType} {name}");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder AddPropertyBody(string value)
        {
            SetIndent(Indent.PropertyBody);
            AppendLine(value);
            return this;
        }

        internal ClassStringBuilder EndProperty()
        {
            SetIndent(Indent.Property);
            return End();
        }

        internal ClassStringBuilder StartMethod(string name, params string[] parameters)
        {
            SetIndent(Indent.Method);
            AppendLine($"public void {name}({string.Join(", ", parameters)})");
            AppendLine("{");
            return this;
        }

        internal ClassStringBuilder EndMethod()
        {
            SetIndent(Indent.Method);
            return End();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private ClassStringBuilder End()
        {
            AppendLine("}");
            return this;
        }

        private void AddBusinessRule(XmlProperty property)
        {
            var rules = BusinessRuleManager.GetRules(property.PropertyType.Name.GetDescription());

            if (property.Required)
            {
                AppendLine($"[Required(ErrorMessage = @\"{property.Name} is required\")]");
            }

            foreach (var rule in rules)
            {
                AppendLine(rule.GetRule(property));
            }
        }

        private void AddDescription(string description) 
        {
            if (!string.IsNullOrEmpty(description))
            {
                AppendLine("/// <summary>");
                AppendLine($"/// {description}");
                AppendLine("/// </summary>");
            }            
        }
    }
}
