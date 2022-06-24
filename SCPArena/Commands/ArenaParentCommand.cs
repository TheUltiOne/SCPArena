using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SCPArena.Commands.Subcommands.Loadout;
using SCPArena.Commands.Subcommands.Scenario;

namespace SCPArena.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ArenaParentCommand : ParentCommand
    {
        public ArenaParentCommand()
            => LoadGeneratedCommands();

        public override string Command { get; } = "scparena";
        public override string[] Aliases { get; } = {"sa"};
        public override string Description { get; } = "The parent command for SCP Arena";

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            var build = new StringBuilder("Here are all the commands you can use:\n\n");
            foreach (var command in Commands.Values)
                build.Append($"<b><color=white>scparena</color> <color=yellow>{command.Command}</color></b> {(command.Aliases.Any() ? $"<color=#3C3C3C>({String.Join(", ", command.Aliases)})</color>" : String.Empty)}\n<color=white>{command.Description}</color>\n\n");

            response = build.ToString();
            return true;
        }

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new ScenarioCommand());
            RegisterCommand(new LoadoutCommand());
        }
    }
}   