using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.PlayerInfo;

namespace _mods.XMGDuelMod.Scripts
{
    public interface IMatchMaking
    {
        
        public AllPlayersState PlayerStates { get; }
        public RoundState RoundState { get; }
        
        public IConsole Console { get; }
        
        public void CreateMatch(ArenaDefinition arenaToUse, List<int> attackingPlayers, List<int> defendingPlayers);
        public void EndMatch(Match matchEnded);

        public void PlayerSpawned(int playerId);

        public void PlayerKilledPlayer(int killerPlayerId, int victimPlayerID);

        public void PlayerLeft(int playerId);
        
        
        public void RegisterPlayerForQueue(int playerId);
        public void RemovePlayerFromQueue(int playerId);

        public void PerformMatchMaking();
        
        public void RegisterArenaForUse(ArenaDefinition arenaToAdd);
        
        public void UnregisterArenaAvailability(ArenaDefinition arenaToRemove);
        public void PlayerHasSwappedFaction(int playerId);
    }
}