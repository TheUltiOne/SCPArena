using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Scenario
{
    public class ScenarioCommand : ParentCommand
    {
        public ScenarioCommand()
            => LoadGeneratedCommands();

        public override string Command { get; } = "scenario";
        public override string[] Aliases { get; } = {"s"};
        public override string Description { get; } = "A sub-parent-command to handle scenarios.";

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null || arg0 == "help")
            {
                var build = new StringBuilder("Here are all the commands you can use:\n\n");
                foreach (var command in Commands.Values)
                    build.Append($"<b><color=white>scparena scenario</color> <color=yellow>{command.Command}</color></b> {(command.Aliases.Any() ? $"<color=#3C3C3C>({String.Join(", ", command.Aliases)})</color>" : String.Empty)}\n<color=white>{command.Description}</color>\n\n");

                response = build.ToString();
                return true;
            }

            if (!arg0.TryGetScenario(out var scenario))
            {
                response = "Could not find a scenario with that name.";
                return false;
            }

            response = $"Scenario Information:\n\n{scenario}";
            return true;
        }

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Apply());
            RegisterCommand(new Unapply());
            RegisterCommand(new Save());
            RegisterCommand(new Delete());
            RegisterCommand(new Select());
            RegisterCommand(new Unselect());
            RegisterCommand(new Edit());
            RegisterCommand(new List());
        }
    }
}   