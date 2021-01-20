using CeloIsYou.Commands;
using CeloIsYou.Rules;
using System.Collections.Generic;

namespace CeloIsYou.Rules
{
    public class Result
    {

        public IReadOnlyCollection<ICommand> Commands { get; private set; }
        public IReadOnlyCollection<Rule> Rules { get; private set; }

        public Result(IReadOnlyCollection<ICommand> commands, IReadOnlyCollection<Rule> rules)
        {
            Commands = commands;
            Rules = rules;
        }
    }
}
