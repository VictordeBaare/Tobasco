using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Commands.TobascoCommands;

namespace TobascoV2.Commands
{
    internal class RootCommand : CommandBase
    {
        public override void Configure(Command command)
        {
            command.AddCommand("Entity", new EntityCommand().Configure);

            base.Configure(command);
        }

        protected override void Execute()
        {
            base.Execute();
        }
    }
}
