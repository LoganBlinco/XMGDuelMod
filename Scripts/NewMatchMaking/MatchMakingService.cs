using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.PlayerInfo;

namespace _mods.XMGDuelMod.Scripts
{
    public class MatchMakingService : IMatchMaking
    {

        public AllPlayersState PlayerStates { get; }
        public RoundState RoundState { get; }
        
        public IConsole Console { get; }
        private IMatchMakingQueue MatchMakingQueueHandler { get; }
        private IMatchMakingAction MatchMakingActionActionHandler { get; }
        
        public MatchMakingService(IConsole console,IMatchMakingQueue matchMakingQueueHandler, IMatchMakingAction matchMakingActionActionHandler, AllPlayersState playerStates, RoundState roundState)
        {
            PlayerStates = playerStates;
            RoundState = roundState;
            Console = console;
            MatchMakingQueueHandler = matchMakingQueueHandler;
            MatchMakingActionActionHandler = matchMakingActionActionHandler;
        }
        
        public void CreateMatch(ArenaDefinition arenaToUse, List<int> attackingPlayers, List<int> defendingPlayers)
        {
            MatchMakingActionActionHandler.CreateMatch(this, arenaToUse, attackingPlayers, defendingPlayers);
        }

        public void EndMatch(Match matchEnded)
        {
            MatchMakingActionActionHandler.EndMatch(this, matchEnded);
            RegisterArenaForUse(matchEnded.ArenaDefinition);
            MatchMakingQueueHandler.RegisterLostPlayersFromMatch(matchEnded);
            
        }

        public void PlayerSpawned(int playerId)
        {
            
            MatchMakingActionActionHandler.PlayerSpawned(playerId);
        }

        public void PlayerKilledPlayer(int killerPlayerId, int victimPlayerID)
        {
            MatchMakingActionActionHandler.PlayerKilledPlayer(killerPlayerId, victimPlayerID);
        }

        public void PlayerLeft(int playerId)
        {
            MatchMakingActionActionHandler.PlayerLeft(playerId);
        }

        public void RegisterPlayerForQueue(int playerId)
        {
            MatchMakingQueueHandler.RegisterPlayerForQueue(playerId);
        }

        public void RemovePlayerFromQueue(int playerId)
        {
            MatchMakingQueueHandler.RemovePlayerFromQueue(playerId);
        }

        public void PerformMatchMaking()
        {
            MatchMakingQueueHandler.PerformMatchMaking(this);
        }

        public void RegisterArenaForUse(ArenaDefinition arenaToAdd)
        {
            MatchMakingQueueHandler.RegisterArenaForUse(arenaToAdd);
        }

        public void UnregisterArenaAvailability(ArenaDefinition arenaToRemove)
        {
            MatchMakingQueueHandler.UnregisterArenaAvailability(arenaToRemove);
        }

        public void PlayerHasSwappedFaction(int playerId)
        {
            MatchMakingActionActionHandler.PlayerSwappedFaction(playerId);
            MatchMakingQueueHandler.PlayerSwappedFaction(playerId);
        }
    }
}