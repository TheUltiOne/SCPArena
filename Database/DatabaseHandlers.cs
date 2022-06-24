using System;
using System.IO;
using System.Linq;
using SCPArena.Database.Modals;
using Exiled.API.Features;
using LiteDB;

namespace SCPArena.Database
{
    internal static class DatabaseHandlers
    {
        public static LiteDatabase LiteDatabase { get; set; }

        public static string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string DatabaseDirectory = Path.Combine(AppData, "EXILED", "Configs", "SCPArena");
        public static string DatabaseFullPath = Path.Combine(DatabaseDirectory, "SCPArenaSave.db");

        public static void InitializeDatabase()
        {
            if (!Directory.Exists(DatabaseDirectory))
                Directory.CreateDirectory(DatabaseDirectory);

            Log.Info("Opening database...");
            OpenDatabase();
        }

        public static void OpenDatabase()
        {
            try
            {
                LiteDatabase = new LiteDatabase(DatabaseFullPath);
                LiteDatabase.GetCollection<Scenario>().EnsureIndex(x => x.Name);
                LiteDatabase.GetCollection<Loadout>().EnsureIndex(x => x.Name);
                Log.Info("Database loaded!");
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't open database!\n{ex}");
            }
        }

        public static bool CreateLoadout(this Player player, Loadout loadout, string name)
        {
            try
            {
                loadout.Name = name;
                LiteDatabase.GetCollection<Loadout>().Insert(loadout);

                return true;
            }
            catch (Exception ex)
            {
                Log.Info($"An error occured while adding a Loadout to the database! {ex}");
                return false;
            }
        }

        public static bool CreateScenario(this Scenario scenario, Player player, string name = null)
        {
            try
            {
                var temp = scenario.Name;
                if (name != null)
                    temp = name;

                scenario.Name = temp;
                scenario.Author = player.Nickname;
                scenario.AuthorId = player.UserId;
                LiteDatabase.GetCollection<Scenario>().Insert(scenario);

                return true;
            }
            catch (Exception ex)
            {
                Log.Info($"An error occured while adding a Scenario to the database! {ex}");
                return false;
            }
        }

        public static Loadout GetLoadout(this string name) => LiteDatabase.GetCollection<Loadout>().FindOne(x => x.Name == name);
        public static Scenario GetScenario(this string name) => LiteDatabase.GetCollection<Scenario>().FindOne(x => x.Name == name);

        public static bool LoadoutInDatabase(this string name) => GetLoadout(name) != null;
        public static bool ScenarioInDatabase(this string name) => GetScenario(name) != null;

        public static void UpdateDatabase(this object obj)
        {
            try
            {
                switch (obj)
                {
                    case Scenario scenario:
                        LiteDatabase.GetCollection<Scenario>().Update(scenario);
                        break;
                    case Loadout loadout:
                        LiteDatabase.GetCollection<Loadout>().Update(loadout);
                        break;
                    default:
                        Log.Warn("Attempted to update database with an unrecognized object.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Warn(ex.ToString());
            }
        }

        public static bool TryGetLoadout(this string name, out Loadout item)
        {
            item = GetLoadout(name);
            return item != null;
        }

        public static bool TryGetScenario(this string name, out Scenario item)
        {
            item = GetScenario(name);
            return item != null;
        }

        public static void Dispose() => LiteDatabase.Dispose();
    }
}
