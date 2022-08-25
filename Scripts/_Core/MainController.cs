using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.Logger;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public class MainController
    {
        
        public Ilogger Logger { get; }
        public IConsole Console { get; }
        
        public RoundState RoundState { get; private set; }

        
        public AllPlayersState PlayerStates { get; }
        

        public MainController(Ilogger logger, IConsole console)
        {
            Logger = logger;
            Console = console;
            RoundState = null;

            PlayerStates = new AllPlayersState();

        }


        public void OnPlayerJoined(int playerId, ulong steamId, string playerName, string regimentTag, bool isBot)
        {
            PlayerStates.UpdateClientData(playerId, steamId, playerName, regimentTag, isBot);
        }

        public void OnPlayerLeft(int playerId)
        {

            PlayerStates.RemovePlayerFromDictionaries(playerId);
        }

        public void OnPlayerSpawned(int playerId, FactionCountry playerFaction, PlayerClass playerClass, GameObject playerObject)
        {
            PlayerStates.UpdatePlayerSpawnedData(playerId, playerFaction, playerClass, playerObject);
        }

        public void OnRoundDetails(string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction,
            GameplayMode gameplayMode, GameType gameType)
        {
            RoundState = new RoundState(serverName, mapName, gameplayMode,
                gameType, attackingFaction, defendingFaction);
        }

        public void OnPlayerHurt(int playerId, byte oldHp, byte newHp)
        {
            if (newHp > 0)
            {
                PlayerStates.UpdatePlayerHealth(playerId, newHp);
            }
        }
    }
}