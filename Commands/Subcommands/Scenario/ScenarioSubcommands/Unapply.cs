using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Unapply : ICommand
    {
        public string Command { get; } = "unapply";
        public string[] Aliases { get; } = {"u",};
        public string Description { get; } = "Unapplies the currently applied scenario.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.AppliedScenario == null)
            {
                response = "There is no currently applied scenario.";
                return false;
            }

            Plugin.AppliedScenario = null;
            response = "Unapplied scenario successfully.";
            return true;
        }
    }
}