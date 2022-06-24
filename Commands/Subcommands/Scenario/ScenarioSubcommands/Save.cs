using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Save : ICommand
    {
        public string Command { get; } = "save";
        public string[] Aliases { get; } = {"s"};
        public string Description { get; } = "Saves the currently selected scenario";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null)
            {
                response = "You must provide a scenario name.";
                return false;
            }

            var player = Player.Get(sender);
            if (!Plugin.SavedScenarios.TryGetValue(player, out var scenario))
            {
                response = "You have no scenario selected. To select a scenario, either start editing it's properties or use sa scenario select.";
                return false;
            }

            if (arg0.ScenarioInDatabase())
                DatabaseHandlers.LiteDatabase.GetCollection<Database.Modals.Scenario>().Delete(arg0);

            scenario.CreateScenario(player, arg0);
            scenario.UpdateDatabase();
            response = "Saved scenario successfully.";
            return true;
        }
    }
}