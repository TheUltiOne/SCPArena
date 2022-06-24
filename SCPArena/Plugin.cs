using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using SCPArena.Database;
using SCPArena.Database.Modals;
using UnityEngine;
using Server = Exiled.Events.Handlers.Server;

namespace SCPArena
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "SCPArena";
        public override string Author { get; } = "TheUltiOne";
        public override Version Version { get; } = new Version(0, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);

        public static Dictionary<Player, Scenario> SavedScenarios { get; set; } = new Dictionary<Player, Scenario>();
        public static List<Vector3> ScenarioVectors { get; set; } = new List<Vector3>();
        public static int SpawnedPlayers { get; set; }

        public static Scenario AppliedScenario
        {
            get => _scenario;
            set
            {
                SpawnedPlayers = 0;
                if (value == null)
                {
                    ScenarioVectors.Clear();
                    return;
                }

                _scenario = value;
                ScenarioVectors = _scenario.FindVectors();
                value.Apply();
            }
        }

        private static Scenario _scenario;
        private EventHandlers _eventHandlers;
        public static Plugin Instance;

        public override void OnEnabled()
        {
            DatabaseHandlers.InitializeDatabase();
            Instance = this;
            _eventHandlers = new EventHandlers();

            Server.RoundEnded += _eventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Dying += _eventHandlers.OnDying;
        }

        public override void OnDisabled()
        {
            Server.RoundEnded -= _eventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Dying -= _eventHandlers.OnDying;

            _eventHandlers = null;
            AppliedScenario = null;
            Instance = null;
            DatabaseHandlers.Dispose();
        }
    }
}