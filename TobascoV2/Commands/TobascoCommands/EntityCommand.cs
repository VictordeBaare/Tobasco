using System;
using System.Collections.Generic;
using TobascoV2.Builder;
using TobascoV2.Constants;
using TobascoV2.Exceptions;

namespace TobascoV2.Commands.TobascoCommands
{
    internal class EntityCommand : CommandBase
    {
        private CommandOption _path;
        private CommandOption _context;
        private CommandOption _assembly;

        public override void Configure(Command command)
        {
            _path = command.AddOption("path", "p", "Path where the xmls definitions are located");
            _context = command.AddOption("context", "c", "Name of the tobasco context");
            _assembly = command.AddOption("assembly", "a", "Location and name of the assembly containing the context");

            base.Configure(command);
        }

        protected override void Validate()
        {            
            base.Validate();
        }

        protected override void Execute()
        {
            IBuilder builder = (IBuilder)Activator.CreateInstance(typeof(EntityBuilder));

            builder.Build(GetParameters());

            base.Execute();
        }

        protected Dictionary<string, string> GetParameters()
        {
            return new Dictionary<string, string> { { BuilderConstants.EntityAssembly, _assembly.Value },
                { BuilderConstants.EntityContext, _context.Value },
                { BuilderConstants.EntityPath, _path.Value } };
        }
    }
}
