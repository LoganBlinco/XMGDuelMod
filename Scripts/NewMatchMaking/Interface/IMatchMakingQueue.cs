using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.ArenaReferences;

namespace _mods.XMGDuelMod.Scripts
{
    public interface IMatchMakingQueue
    {
        public void RegisterPlayerForQueue(int playerId);
        public void RemovePlayerFromQueue(int playerId);
        public void PerformMatchMaking(IMatchMaking matchMakingService);

        public void RegisterArenaForUse(ArenaDefinition arenaToAdd);
        
        public void UnregisterArenaAvailability(ArenaDefinition arenaToRemove);
        
        public void RegisterLostPlayersFromMatch(Match match);
        public void PlayerSwappedFaction(int playerId);
        
    }
}