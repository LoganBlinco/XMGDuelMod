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
    public class Match
    {
        
        public ArenaDefinition ArenaDefinition { get; }
        private IConsole Console { get; }
        private IMatchMaking MatchMakingService { get; }
        private FactionCountry AttackingFaction { get; }
        private FactionCountry DefendingFaction { get; }
        private int RoundsNeededForWin { get; }
        private Settings Settings { get; }
        private Ilogger Logger { get; }

        public Dictionary<int, FactionCountry> GetPlayers => _playerIdToFaction;


        private Dictionary<int, bool> _playerIdToHasBeenKilledThisRound;
        private Dictionary<int, PlayerMatchmakingInfo> _playerIdToInfo;
        private Dictionary<int, FactionCountry> _playerIdToFaction;
        
        private Dictionary<FactionCountry, int> _factionToAlivePlayers;
        private Dictionary<FactionCountry, int> _factionToTotalPlayers;
        private Dictionary<FactionCountry, int> _factionToRoundsLost;
        

        public Match(Ilogger logger, IMatchMaking matchMakingService, ArenaDefinition arenaDefinition,
            List<int> attackingPlayers, List<int> defendingPlayers,
            int roundsNeededForWin, Settings settings)
        {
            logger.LogDebug("We are creating a match");

            Logger = logger;
            MatchMakingService = matchMakingService;
            Console = matchMakingService.Console;
            Settings = settings;

            AttackingFaction = MatchMakingService.RoundState.AttackingFaction;
            DefendingFaction = MatchMakingService.RoundState.DefendingFaction;
            ArenaDefinition = arenaDefinition;
            RoundsNeededForWin = roundsNeededForWin;

            InitDictionaries();

            _factionToAlivePlayers[AttackingFaction] = attackingPlayers.Count;
            _factionToAlivePlayers[DefendingFaction] = defendingPlayers.Count;

            _factionToTotalPlayers[AttackingFaction] = attackingPlayers.Count;
            _factionToTotalPlayers[DefendingFaction] = defendingPlayers.Count;

            
            SetPlayerStates(attackingPlayers, AttackingFaction);
            SetPlayerStates(defendingPlayers, DefendingFaction);
            
            TeleportPlayersToStartPositions(Settings.DuelStartMessage);

        }
        
        private void InitDictionaries()
        {
            _playerIdToInfo = new Dictionary<int, PlayerMatchmakingInfo>();
            _playerIdToFaction = new Dictionary<int, FactionCountry>();
            _playerIdToHasBeenKilledThisRound = new Dictionary<int, bool>();

            _factionToTotalPlayers = new Dictionary<FactionCountry, int>();
            _factionToAlivePlayers = new Dictionary<FactionCountry, int>();
            _factionToRoundsLost = new Dictionary<FactionCountry, int>
            {
                [AttackingFaction] = 0,
                [DefendingFaction] = 0
            };
        }
        

        private void TeleportPlayersToStartPositions(string message)
        {
            foreach (KeyValuePair<int,FactionCountry> keyValuePair in _playerIdToFaction)
            {
                Vector3 nextLocation = GetSpawnLocationForFaction(keyValuePair);

                int playerId = keyValuePair.Key;
                Console.TeleportPlayerToPositionDelayed(playerId, nextLocation,Settings.DelayForTeleport);
                Console.PrivateMessageDelayed(playerId, message, Settings.DelayForMessage);
                _playerIdToHasBeenKilledThisRound[playerId] = false;
            }
        }

        private Vector3 GetSpawnLocationForFaction(KeyValuePair<int, FactionCountry> keyValuePair)
        {
            if (keyValuePair.Value == AttackingFaction)
            {
                return ArenaDefinition.GetNextAttackerSpawn();
            }
            return ArenaDefinition.GetNextDefenderSpawn();
        }
        
        private void SetPlayerStates(List<int> playerList, FactionCountry factionCountry)
        {
            foreach (int playerId in playerList)
            {
                if (!MatchMakingService.PlayerStates.IdToMatchmakingInfo.TryGetValue(playerId, out PlayerMatchmakingInfo matchmakingInfo)){continue;}
                matchmakingInfo.PlayerState = PlayerState.InMatch;
                _playerIdToHasBeenKilledThisRound[playerId] = false;
                _playerIdToInfo[playerId] = matchmakingInfo;
                _playerIdToFaction[playerId] = factionCountry;
            }
        }



        public bool HasPlayer(int playerIdToCheck)
        {
            return _playerIdToFaction.ContainsKey(playerIdToCheck);
        }

        
        
        
        public void PlayerKiledPlayer(int killerPlayerId, int victimPlayerID)
        {
            RegisterKill(victimPlayerID);
        }

        private void RegisterKill(int victimPlayerID)
        {
            if (_playerIdToHasBeenKilledThisRound[victimPlayerID]){return;} //most likely just killing the player in the mosh pit.
            FactionCountry faction = _playerIdToFaction[victimPlayerID];
            _playerIdToHasBeenKilledThisRound[victimPlayerID] = true;
            _factionToAlivePlayers[faction] -= 1;

            PerformEndOfRoundCheck(faction);
        }

        private void PerformEndOfRoundCheck(FactionCountry playerKilledFaction)
        {
            if (_factionToAlivePlayers[playerKilledFaction] <= 0)
            {
                //round over.
                _factionToRoundsLost[playerKilledFaction] += 1;
                if (_factionToRoundsLost[playerKilledFaction] >= RoundsNeededForWin)
                {
                    EndMatch();
                }
                else
                {
                    StartNextRound();
                }
            }
        }

        private void EndMatch()
        {
            //We need to message every player that the score.

            int attackersWon = _factionToRoundsLost[DefendingFaction];
            int defendersWon = _factionToRoundsLost[AttackingFaction];

            string finalMsg =
                $"Game over. Score is {AttackingFaction} {attackersWon} - {defendersWon} {DefendingFaction}";
            
            foreach (KeyValuePair<int, PlayerMatchmakingInfo> playerInfo in _playerIdToInfo)
            {
                Console.PrivateMessageDelayed(playerInfo.Key, finalMsg, Settings.DelayForMessage);
                playerInfo.Value.PlayerState = PlayerState.Idle;
            }
            MatchMakingService.EndMatch(this);
        }

        private void StartNextRound()
        {
            HealPlayers();

            _factionToAlivePlayers[AttackingFaction] = _factionToTotalPlayers[AttackingFaction];
            _factionToAlivePlayers[DefendingFaction] = _factionToTotalPlayers[DefendingFaction];
            
            int attackersWon = _factionToRoundsLost[DefendingFaction];
            int defendersWon = _factionToRoundsLost[AttackingFaction];
            
            string startMessage = $"Round over. Score is {AttackingFaction} {attackersWon} - {defendersWon} {DefendingFaction}. First to {RoundsNeededForWin} wins";
            TeleportPlayersToStartPositions(startMessage);
        }

        private void HealPlayers()
        {
            foreach (KeyValuePair<int,FactionCountry> keyValuePair in _playerIdToFaction)
            {
                int playerId = keyValuePair.Key;
                if (MatchMakingService.PlayerStates.IdToSpawedData.TryGetValue(playerId,
                        out PlayerSpawnedInfo spawnedInfo))
                {
                    Console.HealPlayer(playerId, spawnedInfo.PlayerHealth);
                }
            }
        }

        public void PlayerSpawned(int playerId)
        {
            if (_playerIdToHasBeenKilledThisRound[playerId]){return;} //player already been killed so they are just respawning in mosh pit.
            
            //Players not been killed. They are trying to escape!
            FactionCountry playersFaction = _playerIdToFaction[playerId];
            Vector3 nextLocation;
            if (playersFaction == AttackingFaction)
            {
                nextLocation = ArenaDefinition.GetNextAttackerSpawn();
            }
            else
            {
                nextLocation = ArenaDefinition.GetNextDefenderSpawn();
            }
            Console.TeleportPlayerToPositionDelayed(playerId, nextLocation, Settings.DelayForTeleportShort);
        }

        public void PlayerLeftServer(int playerId)
        {
            RegisterKill(playerId);
            //We then need to remove the player from all info

            _playerIdToHasBeenKilledThisRound.Remove(playerId);
            _playerIdToInfo.Remove(playerId);

            if (_playerIdToFaction.TryGetValue(playerId, out FactionCountry playersFaction))
            {
                DecreaseTotalPlayersForFaction(playersFaction);
                _playerIdToFaction.Remove(playerId);
            }
        }

        private void DecreaseTotalPlayersForFaction(FactionCountry playersFaction)
        {
            _factionToTotalPlayers[playersFaction] -= 1;
            if (_factionToTotalPlayers[playersFaction] <= 0)
            {
                Logger.LogDebug($"Player left resulting in faction {playersFaction} having no players remaining. Ending the match");
                EndMatch();
            }
        }

        public void PlayerSwappedFaction(int playerId)
        {
            //We just handle it as if the player left the server.
            if (_playerIdToInfo.TryGetValue(playerId, out PlayerMatchmakingInfo info))
            {
                info.PlayerState = PlayerState.Idle;
            }
            PlayerLeftServer(playerId);

            
        }
    }
}