using System;
using System.Collections.Generic;
using System.Linq;

namespace TobascoV2.Commands
{
    internal class Command
    {
        public Command()
        {
            Commands = new List<Command>();
            CommandOptions = new List<CommandOption>();
        }

        public List<Command> Commands { get; }
        public List<CommandOption> CommandOptions { get; }
        internal string Name { get; set; }

        internal Action Invoke { get; set; }

        internal void OnExecute(Action invoke) => Invoke = invoke;

        internal void Execute(string[] args)
        {
            var command = this;

            for (int index = 0; index < args.Length; index++)
            {
                var arg = args[index];

                var isLongOption = arg.StartsWith("--");
                if (isLongOption || arg.StartsWith("-"))
                {
                    CommandOption result = ParseOption(isLongOption, command, args, ref index);                    
                }
                else
                {
                    Command subcommand = ParseSubCommand(arg, command);
                    if (subcommand != null)
                    {
                        command = subcommand;
                    }                    
                }
            }

            command.Invoke();
        }

        private Command ParseSubCommand(string arg, Command command)
        {
            foreach (var subcommand in command.Commands)
            {
                if (string.Equals(subcommand.Name, arg, StringComparison.OrdinalIgnoreCase))
                {
                    return subcommand;
                }
            }

            return null;
        }

        private CommandOption ParseOption(bool isLongOption, Command command, string[] args, ref int index)
        {
            CommandOption option = null;
            var arg = args[index];

            var optionPrefixLength = isLongOption ? 2 : 1;
            var optionComponents = arg.Substring(optionPrefixLength).Split(new[] { ':', '=' }, 2);
            var optionName = optionComponents[0];

            if (isLongOption)
            {
                option = command.CommandOptions.SingleOrDefault(
                    opt => string.Equals(opt.LongName, optionName, StringComparison.Ordinal));
            }
            else
            {
                option = command.CommandOptions.SingleOrDefault(
                    opt => string.Equals(opt.ShortName, optionName, StringComparison.Ordinal));
            }

            index++;
            arg = args[index];
            option.Parse(arg);

            return option;
        }

        internal Command AddCommand(string name, Action<Command> config)
        {
            var command = new Command { Name = name };
            Commands.Add(command);
            config(command);
            return command;
        }

        internal CommandOption AddOption(string longName, string shortName, string description, Action<CommandOption> config)
        {
            var option = new CommandOption { LongName = longName, ShortName = shortName, Description = description };
            config(option);
            CommandOptions.Add(option);
            return option;
        }

        internal CommandOption AddOption(string longName, string shortName, string description) => AddOption(longName, shortName, description, (option) => { });        
    }
}
