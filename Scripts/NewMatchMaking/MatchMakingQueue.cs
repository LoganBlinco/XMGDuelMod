using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts._Core;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.Logger;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts
{
    public class MatchMakingQueue : IMatchMakingQueue
    {
        private readonly List<int> _attackingFactionDuelQueue;
        private readonly List<int> _defendingFactionDuelQueue;
        
        private readonly List<int> _attackingFactionGroupfightQueue;
        private readonly List<int> _defendingFactionGroupfightQueue;
        
        private readonly List<int> _playersStuckInArenas;

        private List<ArenaDefinition> _arenasAvailable;
        private readonly ArenaDefinition _moshPit;

        private AllPlayersState AllPlayersStates { get; }
        private RoundState RoundState { get; }
        private Ilogger Logger { get;}
        private IConsole Console { get;}

        
        public MatchMakingQueue(AllPlayersState allPlayersStates, Ilogger logger, RoundState roundState, 
            List<ArenaDefinition> arenasAvailable, ArenaDefinition moshPitDefinition, IConsole console)
        {
            AllPlayersStates = allPlayersStates;
            Logger = logger;
            RoundState = roundState;
            _moshPit = moshPitDefinition;
            Console = console;

            _arenasAvailable = new List<ArenaDefinition>(arenasAvailable);
            _playersStuckInArenas = new List<int>();
            _attackingFactionDuelQueue = new List<int>();
            _defendingFactionDuelQueue = new List<int>();
            _attackingFactionGroupfightQueue = new List<int>();
            _defendingFactionGroupfightQueue = new List<int>();
        }


        public void RegisterPlayerForQueue(int playerId)
        {
            Logger.LogDebug($"Registering {playerId}");
            if (!AllPlayersStates.IdToMatchmakingInfo.TryGetValue(playerId,
                    out PlayerMatchmakingInfo playerMatchmakingInfo))
            {
                Logger.LogError($"Did not find a player with ID {playerId} size is {AllPlayersStates.IdToMatchmakingInfo.Count}");
                return;
            }
            Logger.LogDebug($"Player has priority {playerMatchmakingInfo.MatchMakingPriority}");
            switch (playerMatchmakingInfo.MatchMakingPriority)
            {
                case MatchMakingPriority.All:
                    AddPlayerToDuelQueue(playerId);
                    AddPlayerToGroupfightQueue(playerId);
                    return;
                case MatchMakingPriority.Duels:
                    AddPlayerToDuelQueue(playerId);
                    return;
                case MatchMakingPriority.Groupfights:
                    AddPlayerToGroupfightQueue(playerId);
                    return;
                case MatchMakingPriority.None:
                    return;
                default: 
                    Logger.LogError($"{nameof(RegisterPlayerForQueue)} could not fine type {playerMatchmakingInfo.MatchMakingPriority}");
                    return;
            }
        }

        private void AddPlayerToDuelQueue(int playerId)
        {
            if (!AllPlayersStates.IdToSpawedData.TryGetValue(playerId, out PlayerSpawnedInfo player))
            {
                Logger.LogError($"Did not find a player with ID {playerId} in {nameof(AddPlayerToDuelQueue)}");
                return;
            }
            Logger.LogDebug($"Added {playerId} to Duel Queue");
            if (player.PlayerFaction == RoundState.AttackingFaction)
            {
                if (!_attackingFactionDuelQueue.Contains(playerId)) { _attackingFactionDuelQueue.Add(playerId); }
                if (_defendingFactionDuelQueue.Contains(playerId)) { _defendingFactionDuelQueue.Remove(playerId); }
                return;
            }
            if (player.PlayerFaction == RoundState.DefendingFaction)
            {
                if (!_defendingFactionDuelQueue.Contains(playerId)) { _defendingFactionDuelQueue.Add(playerId); }
                if (_attackingFactionDuelQueue.Contains(playerId)) { _attackingFactionDuelQueue.Remove(playerId); }
            }
        }
        private void AddPlayerToGroupfightQueue(int playerId)
        {
            Logger.LogDebug($"Added {playerId} to groupfight Queue");

            if (!AllPlayersStates.IdToSpawedData.TryGetValue(playerId, out PlayerSpawnedInfo player))
            {
                Logger.LogError($"Did not find a player with ID {playerId} in {nameof(AddPlayerToGroupfightQueue)}");
                return;
            }

            if (player.PlayerFaction == RoundState.AttackingFaction)
            {
                if (!_attackingFactionGroupfightQueue.Contains(playerId)) { _attackingFactionGroupfightQueue.Add(playerId); }
                if (_defendingFactionGroupfightQueue.Contains(playerId)) { _defendingFactionGroupfightQueue.Remove(playerId); }
                return;
            }
            if (player.PlayerFaction == RoundState.DefendingFaction)
            {
                if (!_defendingFactionGroupfightQueue.Contains(playerId)) { _defendingFactionGroupfightQueue.Add(playerId); }
                if (_attackingFactionGroupfightQueue.Contains(playerId)) { _attackingFactionGroupfightQueue.Remove(playerId); }
            }
        }

        public void RemovePlayerFromQueue(int playerId)
        {
            RemovePlayerFromDuelQueue(playerId);
            RemovePlayerFromGroupfightQueue(playerId);
            
        }
        
        public void PerformMatchMaking(IMatchMaking matchMakingService)
        {
            if (_arenasAvailable.Count == 0)
            {
                Logger.LogDebug("No available arenas for matchmaking");
                return;
            }

            ArenaDefinition arenaToUse = _arenasAvailable[0];
            if (PerformMatchMakingGroupfight(matchMakingService, arenaToUse, 2, 4))
            {
                UnregisterArenaAvailability(arenaToUse);
            }
            else
            {
                Logger.LogDebug("Invalid matcmake.");
            }
            HandleLostPlayers();
        }

        private void HandleLostPlayers()
        {
            Settings settings = Settings.Instance;
            
            for (int i = 0; i < _playersStuckInArenas.Count; i++)
            {
                int playerId = _playersStuckInArenas[i];
                Console.TeleportPlayerToPositionDelayed(playerId, _moshPit.GetMoshPitSpawn(),settings.DelayForTeleport);
                RemovePlayerLost(playerId);
            }
        }
        public void RegisterArenaForUse(ArenaDefinition arenaToAdd)
        {
            if (_arenasAvailable.Contains(arenaToAdd))
            {
                Logger.LogError($"We already have {arenaToAdd} as an arena open");
                return;
            }
            _arenasAvailable.Add(arenaToAdd);
            Logger.LogDebug($"Registered arena: {arenaToAdd.name}. Total is {_arenasAvailable.Count}");
        }
        
        public void UnregisterArenaAvailability(ArenaDefinition arenaToRemove)
        {
            _arenasAvailable.Remove(arenaToRemove);
            Logger.LogDebug($"Unregistered {arenaToRemove}. Currnet size is {_arenasAvailable.Count}");
        }

        public void RegisterLostPlayersFromMatch(Match match)
        {
            Dictionary<int, FactionCountry> matchPlayers = match.GetPlayers;
            foreach (KeyValuePair<int, FactionCountry> playerPair in matchPlayers)
            {
                AddPlayerLost(playerPair.Key);
            }
        }


        private void AddPlayerLost(int playerId)
        {
            if (_playersStuckInArenas.Contains(playerId)){return;}

            _playersStuckInArenas.Add(playerId);
            Logger.LogDebug($"Added {playerId} to lost players list");
        }
        private void RemovePlayerLost(int playerId)
        {
            _playersStuckInArenas.Remove(playerId);
            Logger.LogDebug($"Removed {playerId} from the lost list");
        }
        
        private void RemovePlayerFromDuelQueue(int playerId)
        {
            _attackingFactionDuelQueue.Remove(playerId);
            _defendingFactionDuelQueue.Remove(playerId);
            Logger.LogDebug($"Removed {playerId} from Duel Queue");
        }
        private void RemovePlayerFromGroupfightQueue(int playerId)
        {
            _attackingFactionGroupfightQueue.Remove(playerId);
            _defendingFactionGroupfightQueue.Remove(playerId);
            Logger.LogDebug($"Removed {playerId} from groupfight Queue");
        }

        private bool PerformMatchMakingGroupfight(IMatchMaking matchMakingService, ArenaDefinition arenaToUse, int minSize, int maxSize)
        {
            int attackingQueueSize = _attackingFactionGroupfightQueue.Count;
            int defendingQueueSize = _defendingFactionGroupfightQueue.Count;
            if (attackingQueueSize < minSize)
            {
                Logger.LogDebug($"Not enough attackers : {attackingQueueSize}");
                return false;
            }

            if (defendingQueueSize < minSize)
            {           
                Logger.LogDebug($"Not enough defenders : {defendingQueueSize}");
                return false;
            }

            int teamSizeForUse = GetTeamSizeToUse(minSize, maxSize, attackingQueueSize, defendingQueueSize);

            List<int> attackingPlayers = new List<int>();
            List<int> defendingPlayers = new List<int>();

            Logger.LogDebug($"Team size for fight is {teamSizeForUse}");
            
            for (int i = 0; i < teamSizeForUse; i++)
            {
                int attackerId = _attackingFactionGroupfightQueue[0];
                attackingPlayers.Add(attackerId);
                RemovePlayerFromGroupfightQueue(attackerId);
                RemovePlayerLost(attackerId);
                
                int defenderId = _defendingFactionGroupfightQueue[0];
                defendingPlayers.Add(defenderId);
                RemovePlayerFromGroupfightQueue(defenderId);
                RemovePlayerLost(defenderId);
            }
            
            Logger.LogDebug($"Creating match");
            matchMakingService.CreateMatch(arenaToUse, attackingPlayers, defendingPlayers);
            return true;
        }

        private int GetTeamSizeToUse(int minSize, int maxSize, int attackingQueueSize, int defendingQueueSize)
        {
            int smallestFaction = Mathf.Min(attackingQueueSize, defendingQueueSize);
            int maxSizeToUse = Mathf.Min(smallestFaction, maxSize);
            return Random.Range(minSize, maxSizeToUse + 1);
        }
        
        
        public void PlayerSwappedFaction(int playerId)
        {
            RemovePlayerFromQueue(playerId);
        }
    }
}