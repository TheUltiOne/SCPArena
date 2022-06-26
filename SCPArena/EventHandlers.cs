using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;

namespace SCPArena
{
    public class EventHandlers
    {
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Plugin.AppliedScenario = null;
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (Plugin.AppliedScenario == null || ev.ItemsToDrop == null || ev.ItemsToDrop.IsEmpty() || Plugin.AppliedScenario.DropItems) return;
            ev.ItemsToDrop.Clear();
        }

        public void OnShooting(ShootingEventArgs ev)
        {
            if (Plugin.AppliedScenario == null || !Plugin.AppliedScenario.InfiniteAmmo) return;
            (ev.Shooter.CurrentItem as Firearm).Ammo++;
        }
    }
}