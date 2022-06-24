using System;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using CommandSystem;
using Exiled.API.Features;
using SCPArena.Database;
using SCPArena.Database.Modals;

namespace SCPArena.Commands.Subcommands.Scenario.ScenarioSubcommands
{
    public class Edit : ICommand
    {
        public string Command { get; } = "edit";
        public string[] Aliases { get; } = {"e"};
        public string Description { get; } = "Edits an item.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var arg0 = arguments.FirstOrDefault();
            if (arg0 == null || arg0 == "help")
            {
                response =
                    "List of customizable items:\n<b>scparena scenario edit</b>\ninfiniteammo (true/false)\nspawnpos (number [for a player])\nspawnroom (number)\ndropitems (true/false)\nmap <name>\nloadout <name>\nplayers <number>";
                return true;
            }

            var arg1 = arguments.ElementAtOrDefault(1);
            if (arg1 == null)
            {
                response = "You must specify an argument. Refer to scparena scenario edit help for more info.";
                return false;
            }

            var player = Player.Get(sender);
            if (!Plugin.SavedScenarios.TryGetValue(player, out var scenario))
            {
                Plugin.SavedScenarios.Add(player, new Database.Modals.Scenario());
                scenario = Plugin.SavedScenarios[player];
            }

            switch (arg0)
            {
                case "infiniteammo":
                    if (!bool.TryParse(arg1, out var infresult))
                    {
                        response = $"Argument not valid (true/false, provided '{arg1}')";
                        return false;
                    }

                    scenario.InfiniteAmmo = infresult;
                    break;
                case "dropitems":
                    if (!bool.TryParse(arg1, out var dropresult))
                    {
                        response = $"Argument not valid (true/false, provided '{arg1}')";
                        return false;
                    }

                    scenario.DropItems = dropresult;
                    break;
                case "spawnpos":
                    if (!int.TryParse(arg1, out var posPlyResult))
                    {
                        response = $"Argument not valid (integer, provided '{arg1}')";
                        return false;
                    }

                    if (scenario.SpawnPositions.Count+1 >= posPlyResult)
                        scenario.SpawnPositions.Add(SerializableVector.FromVector(player.Position));
                    else
                        scenario.SpawnPositions[posPlyResult - 1] = SerializableVector.FromVector(player.Position);

                    break;
                case "spawnroom":
                    if (!int.TryParse(arg1, out var roomPlyResult))
                    {
                        response = $"Argument not valid (integer, provided '{arg1}')";
                        return false;
                    }

                    if (scenario.RoomSpawnPositions.Count+1 >= roomPlyResult)
                        scenario.RoomSpawnPositions.Add(player.CurrentRoom.Type);
                    else
                        scenario.RoomSpawnPositions[roomPlyResult - 1] = player.CurrentRoom.Type;

                    break;
                case "map":
                    scenario.Map = arg1;
                    break;
                case "loadout":
                    var arg2 = arguments.ElementAtOrDefault(1);
                    if (arg2 == null)
                    {
                        response = "Missing argument 2. (loadout name)";
                        return false;
                    }

                    if (!arg2.TryGetLoadout(out var loadout))
                    {
                        response = "There was no loadout found with that name.";
                        return false;
                    }

                    var arg3 = arguments.ElementAtOrDefault(2);
                    if (arg3 == null)
                    {
                        response = "Missing argument 3. (player number)";
                        return false;
                    }

                    if (!int.TryParse(arg3, out var loadoutPlyResult))
                    {
                        response = $"Argument not valid (integer, provided '{arg3}')";
                        return false;
                    }

                    if (scenario.Loadouts.Count+1 >= loadoutPlyResult)
                        scenario.Loadouts.Add(loadout);
                    else
                        scenario.Loadouts[loadoutPlyResult - 1] = loadout;
                    break;
                case "players":
                    if (!int.TryParse(arg1, out var plyresult))
                    {
                        response = $"Argument not valid (number, provided '{arg1}')";
                        return false;
                    }

                    scenario.Players = plyresult;
                    break;
            }

            response = "Edited scenario successfully.";
            return true;
        }
    }
}