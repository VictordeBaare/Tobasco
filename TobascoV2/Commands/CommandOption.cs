namespace TobascoV2.Commands
{
    internal class CommandOption
    {
        public string Description { get; internal set; }
        public string LongName { get; internal set; }
        public string ShortName { get; internal set; }
        public string Value { get; internal set; }

        internal void Parse(string arg)
        {
            Value = arg;
        }
    }
}
