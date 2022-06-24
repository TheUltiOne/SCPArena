using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace SCPArena.Commands.Subcommands.Loadout.LoadoutSubcommands
{
    public class List : ICommand
    {
        public string Command { get; } = "list";
        public string[] Aliases { get; } = {"l"};
        public string Description { get; } = "Lists all loadouts.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"List of saved loadouts:\n{string.Join(",\n", Database.DatabaseHandlers.LiteDatabase.GetCollection<Database.Modals.Loadout>().FindAll().OrderBy(x => x.Name).Select(x => x.Name))}";
            return true;
        }
    }
}