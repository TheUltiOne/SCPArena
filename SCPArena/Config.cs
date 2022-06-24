using Exiled.API.Interfaces;

namespace SCPArena
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool AutoCleanupBodies { get; set; } = true;
        public bool AutoCleanupItems { get; set; } = true;
        public bool CountdownEnabled { get; set; } = true;
    }
}