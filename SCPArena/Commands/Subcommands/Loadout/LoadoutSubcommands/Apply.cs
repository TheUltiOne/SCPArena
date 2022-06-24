using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Loadout.LoadoutSubcommands
{
    public class Apply : ICommand
    {
        public string Command { get; } = "apply";
        public string[] Aliases { get; } = {"a"};
        public string Description { get; } = "Applies a loadout to yourself.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null)
            {
                response = "You must provide a loadout name.";
                return false;
            }

            if (!arg0.TryGetLoadout(out var loadout))
            {
                response = "Didn't find a loadout with that name.";
                return false;
            }
            
            var player = Player.Get(sender);
            loadout.Apply(player);
            response = "Applied loadout successfully.";
            return true;
        }
    }
}