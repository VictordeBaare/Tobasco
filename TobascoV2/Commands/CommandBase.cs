using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobascoV2.Commands
{
    internal abstract class CommandBase
    {
        public virtual void Configure(Command command)
        {
            command.OnExecute(() =>
                {
                    Validate();

                    Execute();
                });
        }

        protected virtual void Validate() { }

        protected virtual void Execute() { }            
    }
}
