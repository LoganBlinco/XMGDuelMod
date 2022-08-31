using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.Logger;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public class MainController
    {
        private Ilogger Logger { get; }
        private IConsole Console { get; }
        private IMatchMaking MatchMakingService { get; set; }
        private RoundState RoundState { get; set; }
        private AllPlayersState PlayerStates { get; }
        

        public MainController(Ilogger logger, IConsole console, AllPlayersState playersStates)
        {
            Logger = logger;
            Console = console;
            PlayerStates = playersStates;
        }


        public void OnPlayerJoined(int playerId, ulong steamId, string playerName, string regimentTag, bool isBot)
        {
            PlayerStates.UpdateClientData(playerId, steamId, playerName, regimentTag, isBot);
            PlayerStates.CreatePlayerMatchmakingInfo(playerId, Logger);
        }

        public void OnPlayerLeft(int playerId)
        {
            PlayerStates.RemovePlayerFromDictionaries(playerId);

            MatchMakingService.PlayerLeft(playerId);
            MatchMakingService.RemovePlayerFromQueue(playerId);
        }

        public void OnPlayerSpawned(int playerId, FactionCountry playerFaction, PlayerClass playerClass, GameObject playerObject)
        {
            if (PlayerStates.IdToSpawedData.TryGetValue(playerId, out PlayerSpawnedInfo previousInfo) && playerFaction != previousInfo.PlayerFaction)
            {
                //Player has swapped faction.
                MatchMakingService.PlayerHasSwappedFaction(playerId);
            }

            PlayerStates.UpdatePlayerSpawnedData(Logger, playerId, playerFaction, playerClass, playerObject);
            MatchMakingService.PlayerSpawned(playerId);
            if (PlayerStates.IdToMatchmakingInfo.TryGetValue(playerId, out PlayerMatchmakingInfo info) && info.PlayerState == PlayerState.Idle)
            {
                Logger.LogDebug("Player was idle so we are registering them for a queue");
                MatchMakingService.RegisterPlayerForQueue(playerId);
            }
        }

        public void OnRoundDetails(string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction,
            GameplayMode gameplayMode, GameType gameType)
        {
            RoundState = new RoundState(serverName, mapName, gameplayMode,
                gameType, attackingFaction, defendingFaction);


            MatchMakingService = Factory.CreateMatchMakingRefactored(Logger, Console, PlayerStates,RoundState);
            MatchMakingManager.Instance.MatchMaking = MatchMakingService;
            Logger.LogDebug($"Assigned {nameof(MatchMakingService)}");
        }
        

        public void OnPlayerHurt(int playerId, byte oldHp, byte newHp)
        {
            if (newHp > 0)
            {
                PlayerStates.UpdatePlayerHealth(playerId, newHp);
            }
        }

        public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId)
        {
            MatchMakingService.PlayerKilledPlayer(killerPlayerId, victimPlayerId);
        }

        public void PassConfigVariables(string[] value)
        {
            Settings.Instance.PassConfigVariables(value);

            Logger.LogDebug("Running matchmaking");
            
            MatchMakingManager.Instance.BeginMatchMaking(Logger);
        }
    }
}