using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Unselect : ICommand
    {
        public string Command { get; } = "unselect";
        public string[] Aliases { get; } = {"us", "uns", "unsel"};
        public string Description { get; } = "Unselects a scenario. IF YOU DID NOT SAVE YOUR CURRENTLY SELECTED SCENARIO, THIS WILL DELETE IT.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (!Plugin.SavedScenarios.ContainsKey(player))
            {
                response = "You are already not selecting any scenario.";
                return false;
            }

            Plugin.SavedScenarios.Remove(player);
            response = "Unselected scenario successfully.";
            return true;
        }
    }
}