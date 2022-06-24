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
    }
}