using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Builder;

namespace TobascoV2.Commands.TobascoCommands
{
    internal class DatabaseCommand : CommandBase
    {
        protected override void Execute()
        {
            IBuilder builder = (IBuilder)Activator.CreateInstance(typeof(DatabaseBuilder));

            builder.Build(new Dictionary<string, string>());

            base.Execute();
        }
    }
}
