using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.ArenaReferences;

namespace _mods.XMGDuelMod.Scripts
{
    public interface IMatchMakingAction
    {
        public void CreateMatch(IMatchMaking matchMaking, ArenaDefinition arenaToUse, List<int> attackingPlayers, List<int> defendingPlayers);
        public void EndMatch(IMatchMaking matchMaking, Match matchEnded);
        public void PlayerSpawned(int playerId);
        public void PlayerKilledPlayer(int killerPlayerId, int victimPlayerID);
        public void PlayerLeft(int playerId);

        public void PlayerSwappedFaction(int playerId);
    }
}