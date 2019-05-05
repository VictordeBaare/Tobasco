using System;

namespace TobascoV2.Builder
{
    public class ClassStringBuilder
    {
        private IndentStringBuilder _indentStringBuilder;

        public ClassStringBuilder()
        {
            _indentStringBuilder = new IndentStringBuilder(4);
        }

        public ClassStringBuilder AddUsing(string usingNs)
        {
            _indentStringBuilder.AppendLine($"using {usingNs};");

            return this;
        }

        public ClassStringBuilder StartNamesspace(string ns)
        {
            _indentStringBuilder.AppendLine($"namespace {ns}");
            return this;
        }

        public ClassStringBuilder EndNamespace()
        {
            _indentStringBuilder.RemoveIndent().AppendLine("}");
            return this;
        }

        public ClassStringBuilder StartClass(string classname, string baseclass)
        {
            _indentStringBuilder.AppendLine($"public {classname} : {baseclass}");
            _indentStringBuilder.AppendLine("{").AddIndent();
            return this;
        }

        public ClassStringBuilder StartClass(string classname)
        {
            _indentStringBuilder.AppendLine($"public {classname}");
            _indentStringBuilder.AppendLine("{").AddIndent();
            return this;
        }

        public ClassStringBuilder EndClass()
        {
            return End();
        }

        public ClassStringBuilder StartConstructor(string name, params string[] parameters)
        {
            _indentStringBuilder.AppendLine($"public void {name}({string.Join(", ", parameters)})");
            _indentStringBuilder.AppendLine("{").AddIndent();
            return this;
        }

        public ClassStringBuilder AddConstructorBody(string value)
        {
            _indentStringBuilder.AppendLine(value);
            return this;
        }

        public ClassStringBuilder EndConstructor()
        {
            return End();
        }

        public ClassStringBuilder AddProperty(string modifier, string dataType, string name)
        {
            _indentStringBuilder.AddIndent();
            _indentStringBuilder.AppendLine($"{modifier} {dataType} {name} {{ get; set; }}").RemoveIndent();
            return this;
        }

        public ClassStringBuilder AddProperty(string modifier, string dataType, string name, string description)
        {
            _indentStringBuilder.AddIndent();
            AddDescription(description);
            _indentStringBuilder.AppendLine($"{modifier} {dataType} {name} {{ get; set; }}").RemoveIndent();
            return this;
        }

        public ClassStringBuilder StartProperty(string modifier, string dataType, string name)
        {
            _indentStringBuilder.AddIndent();
            _indentStringBuilder.AppendLine($"{modifier} {dataType} {name}");
            _indentStringBuilder.AppendLine("{").AddIndent();
            return this;
        }

        public ClassStringBuilder StartProperty(string modifier, string dataType, string name, string description)
        {
            _indentStringBuilder.AddIndent();
            AddDescription(description);
            _indentStringBuilder.AppendLine($"{modifier} {dataType} {name}");
            _indentStringBuilder.AppendLine("{").AddIndent();
            return this;
        }

        public ClassStringBuilder AddPropertyBody(string value)
        {
            _indentStringBuilder.AppendLine(value);
            return this;
        }

        public ClassStringBuilder EndProperty()
        {
            return End();
        }

        public ClassStringBuilder StartMethod(string name, params string[] parameters)
        {
            _indentStringBuilder.AppendLine($"public void {name}({string.Join(", ", parameters)})");
            _indentStringBuilder.AppendLine("{").AddIndent();
            return this;
        }

        public ClassStringBuilder EndMethod()
        {
            return End();
        }

        public override string ToString()
        {
            return _indentStringBuilder.ToString();
        }

        private ClassStringBuilder End()
        {
            _indentStringBuilder.AppendLine("{").RemoveIndent();
            return this;
        }

        private void AddDescription(string description)
        {
            _indentStringBuilder.AppendLine("/// <summary>");
            _indentStringBuilder.AppendLine($"/// {description}");
            _indentStringBuilder.AppendLine("/// </summary>");
        }
    }
}
