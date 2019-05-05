using System.Text;

namespace TobascoV2.Builder
{
    public class IndentStringBuilder
    {
        private readonly int _identSize = 4;
        private readonly StringBuilder _stringBuilder;

        private int _timesIndented = 0;

        public IndentStringBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public IndentStringBuilder(int indentSize) : base()
        {
            _identSize = indentSize;
        }

        public IndentStringBuilder AddIndent()
        {
            _timesIndented++;
            return this;
        }

        public IndentStringBuilder RemoveIndent()
        {
            _timesIndented--;
            return this;
        }

        public IndentStringBuilder AppendLine(string value)
        {
            _stringBuilder.AppendLine(value);
            return this;
        }

        public IndentStringBuilder Append(string value)
        {
            _stringBuilder.Append(value);
            return this;
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
