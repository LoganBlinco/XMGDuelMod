using _mods.XMGDuelMod.Scripts.Enums;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public class Settings : BaseSingleton<Settings>
    {

        public DebugMode DebugMode { get; private set; } = DebugMode.Debug;
        public MatchMakingPriority StartingMatchMakingPriority { get; private set; } = MatchMakingPriority.All;

        public string DuelStartMessage { get; private set; } = "Begin! Qapla'";
        public int DelayForTeleport { get; private set; } = 1;
        public int DelayForMessage { get; private set; } = 1;
        
    }
}