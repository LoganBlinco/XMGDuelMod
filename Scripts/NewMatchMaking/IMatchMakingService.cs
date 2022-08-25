using _mods.XMGDuelMod.Scripts.PlayerInfo;
using HoldfastSharedMethods;

namespace _mods.XMGDuelMod.Scripts
{
    public interface IMatchMakingService
    {
        public FactionCountry AttackingFaction { get; }
        public FactionCountry DefendingFaction { get; }
        public AllPlayersState PlayerStates { get; }
        
        public void CreateMatch();
        public void EndMatch(Match matchEnded);

        public void PlayerSpawned(int playerId);

        public void PlayerKilledPlayer(int killerPlayerId, int victimPlayerID);



    }
}