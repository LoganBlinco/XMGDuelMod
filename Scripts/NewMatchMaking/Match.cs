using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts._Core;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts
{
    public class Match
    {



        private IArenaDefinition ArenaDefinition { get; }
        private IConsole Console { get; }
        private IMatchMakingService MatchMakingService { get; }
        private FactionCountry AttackingFaction { get; }
        private FactionCountry DefendingFaction { get; }
        private int RoundsNeededForWin { get; }
        private Settings Settings { get; }


        private Dictionary<int, bool> _playerIdToHasBeenKilledThisRound;
        private Dictionary<int, PlayerMatchmakingInfo> _playerIdToInfo;
        private Dictionary<int, FactionCountry> _playerIdToFaction;
        
        private Dictionary<FactionCountry, int> _factionToAlivePlayers;
        private Dictionary<FactionCountry, List<PlayerMatchmakingInfo>> _factionToPlayerInfo;
        private Dictionary<FactionCountry, int> _factionToRoundsLost;
        

        public Match(IArenaDefinition arenaDefinition,
            List<PlayerMatchmakingInfo> attackingPlayers, List<PlayerMatchmakingInfo> defendingPlayers,
            int roundsNeededForWin, IMatchMakingService matchMakingService, IConsole console)
        {
            MatchMakingService = matchMakingService;
            Console = console;
            Settings = Settings.Instance;

            AttackingFaction = MatchMakingService.AttackingFaction;
            DefendingFaction = MatchMakingService.DefendingFaction;
            ArenaDefinition = arenaDefinition;
            RoundsNeededForWin = roundsNeededForWin;

            InitDictionaries();
            _factionToPlayerInfo[AttackingFaction] = attackingPlayers;
            _factionToPlayerInfo[DefendingFaction] = defendingPlayers;
            
            
            _factionToAlivePlayers[AttackingFaction] = attackingPlayers.Count;
            _factionToAlivePlayers[DefendingFaction] = defendingPlayers.Count;
            
            SetPlayerStates(attackingPlayers, AttackingFaction);
            SetPlayerStates(defendingPlayers, DefendingFaction);
            
        }
        
        private void InitDictionaries()
        {
            _playerIdToInfo = new Dictionary<int, PlayerMatchmakingInfo>();
            _playerIdToFaction = new Dictionary<int, FactionCountry>();
            _playerIdToHasBeenKilledThisRound = new Dictionary<int, bool>();
            
            _factionToAlivePlayers = new Dictionary<FactionCountry, int>();
            _factionToPlayerInfo = new Dictionary<FactionCountry, List<PlayerMatchmakingInfo>>();
            _factionToRoundsLost = new Dictionary<FactionCountry, int>
            {
                [AttackingFaction] = 0,
                [DefendingFaction] = 0
            };
        }
        

        private void TeleportPlayersToStartPositions()
        {
            TeleportAttackers();
            TeleportDefenders();
        }

        private void TeleportAttackers()
        {
            foreach (PlayerMatchmakingInfo playerInfo in _factionToPlayerInfo[AttackingFaction])
            {
                Vector3 nextLocation = ArenaDefinition.GetNextAttackerSpawn();
                Console.TeleportPlayerToPosition(playerInfo.PlayerId, nextLocation);
                Console.PrivateMessageDelayed(playerInfo.PlayerId, Settings.DuelStartMessage, Settings.DelayForMessage);
                _playerIdToHasBeenKilledThisRound[playerInfo.PlayerId] = false;
            }
        }
        private void TeleportDefenders()
        {
            foreach (PlayerMatchmakingInfo playerInfo in _factionToPlayerInfo[DefendingFaction])
            {
                Vector3 nextLocation = ArenaDefinition.GetNextDefenderSpawn();
                Console.TeleportPlayerToPosition(playerInfo.PlayerId, nextLocation);
                Console.PrivateMessageDelayed(playerInfo.PlayerId, Settings.DuelStartMessage, Settings.DelayForTeleport);
                _playerIdToHasBeenKilledThisRound[playerInfo.PlayerId] = false;

            }
        }


        private void SetPlayerStates(List<PlayerMatchmakingInfo> playerList, FactionCountry factionCountry)
        {
            foreach (PlayerMatchmakingInfo playerMatchmakingInfo in playerList)
            {
                playerMatchmakingInfo.PlayerState = PlayerState.InMatch;
                int playerId = playerMatchmakingInfo.PlayerId;
                _playerIdToHasBeenKilledThisRound[playerId] = false;
                _playerIdToInfo[playerId] = playerMatchmakingInfo;
                _playerIdToFaction[playerId] = factionCountry;

            }
        }



        public bool HasPlayer(int playerIdToCheck)
        {
            return _playerIdToInfo.ContainsKey(playerIdToCheck);
        }

        
        
        
        public void PlayerKiledPlayer(int killerPlayerId, int victimPlayerID)
        {
            if (_playerIdToHasBeenKilledThisRound[victimPlayerID]){return;} //most likely just killing the player in the mosh pit.

            RegisterKill(victimPlayerID);
        }

        private void RegisterKill(int victimPlayerID)
        {
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
            
            TeleportPlayersToStartPositions();
        }

        private void HealPlayers()
        {
            foreach (KeyValuePair<int,PlayerMatchmakingInfo> playerMatchmakingInfo in _playerIdToInfo)
            {
                int playerId = playerMatchmakingInfo.Key;
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
            Console.TeleportPlayerToPosition(playerId, nextLocation);
        }
    }
}