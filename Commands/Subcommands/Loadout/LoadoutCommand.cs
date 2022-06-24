using System;
using System.Linq;
using System.Text;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SCPArena.Commands.Subcommands.Loadout.LoadoutSubcommands;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Loadout
{
    public class LoadoutCommand : ParentCommand
    {
        public LoadoutCommand()
            => LoadGeneratedCommands();

        public override string Command { get; } = "loadout";
        public override string[] Aliases { get; } = {"l"};
        public override string Description { get; } = "A sub-parent-command to handle loadouts.";

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null || arg0 == "help")
            {
                var build = new StringBuilder("Here are all the commands you can use:\n\n");
                foreach (var command in Commands.Values)
                    build.Append($"<b><color=white>scparena loadout</color> <color=yellow>{command.Command}</color></b> {(command.Aliases.Any() ? $"<color=#3C3C3C>({String.Join(", ", command.Aliases)})</color>" : String.Empty)}\n<color=white>{command.Description}</color>\n\n");

                response = build.ToString();
                return true;
            }

            if (!arg0.TryGetLoadout(out var loadout))
            {
                response = "Could not find a loadout with that name.";
                return false;
            }

            response =
                $"Loadout Information:\n\n{loadout}";
            return true;
        }

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Apply());
            RegisterCommand(new Save());
            RegisterCommand(new List());
        }
    }
}   