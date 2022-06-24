using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Select : ICommand
    {
        public string Command { get; } = "select";
        public string[] Aliases { get; } = {"sel"};
        public string Description { get; } = "Selects a scenario. IF YOU DID NOT SAVE YOUR CURRENTLY SELECTED SCENARIO, THIS WILL OVERWRITE IT.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null)
            {
                response = "You must provide a scenario name.";
                return false;
            }

            if (!arg0.TryGetScenario(out var scenario))
            {
                response = "Didn't find a scenario with that name.";
                return false;
            }

            var player = Player.Get(sender);
            if (!Plugin.SavedScenarios.ContainsKey(player))
                Plugin.SavedScenarios.Add(player, scenario);
            else
                Plugin.SavedScenarios[player] = scenario;

            response = "Selected scenario successfully.";
            return true;
        }
    }
}