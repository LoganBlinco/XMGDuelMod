using _mods.XMGDuelMod.Scripts.PlayerInfo;

namespace _mods.XMGDuelMod.Scripts
{
    public interface IMatchMakingQueue
    {
        public void RegisterPlayerForQueue(PlayerMatchmakingInfo player);
        public void RemovePlayerFromQueue(PlayerMatchmakingInfo player);
    }
}