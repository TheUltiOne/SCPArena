using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using LiteDB;
using MEC;
using UnityEngine;

namespace SCPArena.Database.Modals
{
    public class Scenario
    {
        [BsonId] public string Name { get; set; }
        public bool InfiniteAmmo { get; set; }
        public bool DropItems { get; set; } = true;
        public List<SerializableVector> SpawnPositions { get; set; } = new List<SerializableVector>();
        public List<RoomType> RoomSpawnPositions { get; set; } = new List<RoomType>();
        public List<Loadout> Loadouts { get; set; } = new List<Loadout>();
        public int Players { get; set; } = 2;
        public string Map { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }

        public List<Vector3> FindVectors()
        {
            if (Plugin.ScenarioVectors.Any())
                return Plugin.ScenarioVectors;

            var anyRoomPositions = RoomSpawnPositions.Any();
            if (SpawnPositions.IsEmpty())
            {
                if (!anyRoomPositions)
                    throw new InvalidOperationException($"No spawn positions were found for Scenario '{Name}'.");

                var spawnVectors = new List<Vector3>();
                foreach (var room in RoomSpawnPositions)
                    spawnVectors.Add(Room.List.First(x => x.Type == room).Position + Vector3.up * 1.15f);

                return spawnVectors;
            }

            if (anyRoomPositions)
                Log.Warn($"There are saved room positions AND saved Vector3 positions for scenario {Name}; Vector3 positions will be used.");

            return SerializableVector.ToManyVectors(SpawnPositions);
        }

        public void Apply()
        {
            foreach (var player in Player.List)
                Timing.RunCoroutine(SpawnPlayer(player));

            if (Plugin.Instance.Config.CountdownEnabled)
                Timing.CallDelayed(1, () => Cassie.MessageTranslated("5 yield_1 4 yield_1 3 yield_1 2 yield_1 1 yield_1 go", "5, 4, 3, 2, 1, GO!", true, false));

            if (Map != null)
                GameCore.Console.singleton.TypeCommand($"/mp l {Map}");

            if (Plugin.Instance.Config.AutoCleanupBodies)
            {
                foreach (var ragdoll in Exiled.API.Features.Map.Ragdolls)
                    ragdoll.Delete();
            }

            if (Plugin.Instance.Config.AutoCleanupItems)
            {
                foreach (var pickup in Exiled.API.Features.Map.Pickups)
                    pickup.Destroy();
            }
        }

        private IEnumerator<float> SpawnPlayer(Player player)
        {
            if (Players <= Plugin.SpawnedPlayers)
            {
                player.Role.Type = RoleType.Spectator;
                yield break;
            }

            player.Role.Type = RoleType.Tutorial;
            player.EnableEffect<Ensnared>(5);

            yield return Timing.WaitForSeconds(1);
            Loadouts[Plugin.SpawnedPlayers].Apply(player);
            player.Position = Plugin.ScenarioVectors[Plugin.SpawnedPlayers];

            Plugin.SpawnedPlayers++;
        }

        public override string ToString()
        {
            StringBuilder spawnPoints = new StringBuilder("Spawn Points:\n");

            var anyRoomPositions = RoomSpawnPositions.Any();
            if (SpawnPositions.IsEmpty())
            {
                if (!anyRoomPositions)
                    throw new InvalidOperationException($"No spawn positions were found for Scenario '{Name}'.");

                spawnPoints.Append(string.Join(", ", RoomSpawnPositions.Select(x => x.ToString())));
            }
            else
                spawnPoints.Append(string.Join(", ", SpawnPositions.Select(x => x.ToString())));

            if (anyRoomPositions)
                Log.Warn($"There are saved room positions AND saved Vector3 positions for scenario {Name}; Vector3 positions will be used.");

            return $"Author: {Author} ({AuthorId})\nName: {Name}\nLoadouts: {string.Join(", ", Loadouts.Select(x => x.Name))}\n{(string.IsNullOrWhiteSpace(Map) ? string.Empty : $"Map: {Map}\n")}Infinite Ammo: {InfiniteAmmo}\nDropping Items: {DropItems}\n{spawnPoints}\nPlayer Count: {Players}";
        }
    }
}