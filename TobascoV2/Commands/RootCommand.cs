using TobascoV2.Commands.TobascoCommands;

namespace TobascoV2.Commands
{
    internal class RootCommand : CommandBase
    {
        public override void Configure(Command command)
        {
            command.AddCommand("Entity", new EntityCommand().Configure);
            command.AddCommand("DapperEntity", new DapperEntityCommand().Configure);
            command.AddCommand("Database", new DatabaseCommand().Configure);

            base.Configure(command);
        }
    }
}
