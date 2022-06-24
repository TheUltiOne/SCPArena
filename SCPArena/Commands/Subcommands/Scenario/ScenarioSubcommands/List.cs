using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.API.Features;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class List : ICommand
    {
        public string Command { get; } = "list";
        public string[] Aliases { get; } = {"l"};
        public string Description { get; } = "Lists all scenarios.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"List of saved scenarios:\n{string.Join(",\n", Database.DatabaseHandlers.LiteDatabase.GetCollection<Database.Modals.Scenario>().FindAll().OrderBy(x => x.Name).Select(x => x.Name))}";
            return true;
        }
    }
}