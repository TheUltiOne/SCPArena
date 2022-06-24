using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Apply : ICommand
    {
        public string Command { get; } = "apply";
        public string[] Aliases { get; } = {"a"};
        public string Description { get; } = "Applies the currently selected scenario.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (!Plugin.SavedScenarios.TryGetValue(player, out var scenario))
            {
                response = "You do not have any scenario selected.";
                return false;
            }

            Plugin.AppliedScenario = scenario;
            response = "Applied scenario successfully.";
            return true;
        }
    }
}