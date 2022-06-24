using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using LiteDB;

namespace SCPArena.Database.Modals
{
    public class Loadout
    {
        [BsonId] public string Name { get; set; }
        public List<ItemType> Items { get; set; }
        public Dictionary<ItemType, ushort> Ammo { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }

        public void Apply(Player player)
        {
            player.ResetInventory(Items);
            foreach (var ammo in player.Ammo)
                player.Ammo[ammo.Key] = ammo.Value;
        }

        public static Loadout Create(Player player)
        {
            return new Loadout
            {
                Items = player.Items.Select(x => x.Type).ToList(),
                Ammo = player.Ammo,
                Author = player.Nickname,
                AuthorId = player.UserId
            };
        }

        public override string ToString()
        {
            var ammoText = Ammo.Aggregate("Ammo: ", (current, ammo) => current + $"{ammo.Key}: {ammo.Value}, ");
            return $"Author: {Author} ({AuthorId})\nName: {Name}\nItems: {string.Join(", ", Items.Select(x => x.ToString()))}\n{ammoText}";
        }
    }
}