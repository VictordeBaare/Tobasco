using System;
using System.Text;
using TobascoV2.Constants;

namespace TobascoV2.Builder
{
    public class IndentStringBuilder
    {
        private readonly int _identSize = 4;
        private readonly StringBuilder _stringBuilder;

        private int _indent;

        public IndentStringBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public IndentStringBuilder(int indentSize) : this()
        {
            _identSize = indentSize;
        }

        internal IndentStringBuilder SetIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public IndentStringBuilder AppendLine(string value)
        {
            AddIndentValue()._stringBuilder.AppendLine(value);
            return this;
        }

        public virtual IndentStringBuilder AppendLine(string value, int indent)
        {
            SetIndent(indent);
            AddIndentValue()._stringBuilder.AppendLine(value);
            return this;
        }

        public virtual IndentStringBuilder Append(string value)
        {
            AddIndentValue()._stringBuilder.Append(value);
            return this;
        }

        public virtual T Append<T>(string value) where T : IndentStringBuilder
        {
            AddIndentValue()._stringBuilder.Append(value);
            return (T)this;
        }

        private IndentStringBuilder AddIndentValue()
        {
            if(_indent > 0)
            {
                _stringBuilder.Append(new string(' ', _indent * _identSize));
            }
            return this;
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
