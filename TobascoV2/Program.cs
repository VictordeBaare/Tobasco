using TobascoV2.Commands;

namespace TobascoV2
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new Command { Name = "TobascoV2" };
            new RootCommand().Configure(command);

            command.Execute(args);
        }
    }
}
