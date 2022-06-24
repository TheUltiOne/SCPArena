using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Delete : ICommand
    {
        public string Command { get; } = "delete";
        public string[] Aliases { get; } = {"del", "d"};
        public string Description { get; } = "Deletes a scenario";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null)
            {
                response = "You must provide a scenario name.";
                return false;
            }

            if (!arg0.ScenarioInDatabase())
            {
                response = "There is no saved scenario with that name.";
                return false;
            }

            DatabaseHandlers.LiteDatabase.GetCollection<Database.Modals.Scenario>().Delete(arg0);
            response = "Deleted scenario successfully.";
            return true;
        }
    }
}