using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;

namespace SCPArena.Commands.Subcommands.Loadout.LoadoutSubcommands
{
    public class Save : ICommand
    {
        public string Command { get; } = "save";
        public string[] Aliases { get; } = {"s"};
        public string Description { get; } = "Saves the loadout you are holding";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null)
            {
                response = "You must provide a loadout name.";
                return false;
            }

            var player = Player.Get(sender);
            if (arg0.LoadoutInDatabase())
                DatabaseHandlers.LiteDatabase.GetCollection<Database.Modals.Loadout>().Delete(arg0);

            var loadout = Database.Modals.Loadout.Create(player);
            player.CreateLoadout(loadout, arg0);
            loadout.UpdateDatabase();
            response = "Saved loadout successfully.";
            return true;
        }
    }
}