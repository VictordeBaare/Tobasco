using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Builder;

namespace TobascoV2.Commands.TobascoCommands
{
    internal class DapperRepositoryCommand : EntityCommand
    {
        protected override void Execute()
        {
            IBuilder builder = (IBuilder)Activator.CreateInstance(typeof(DapperRepositoryBuilder));

            builder.Build(GetParameters());            
        }
    }
}
