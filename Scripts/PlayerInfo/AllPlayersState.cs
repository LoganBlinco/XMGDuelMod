using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts._Core;
using _mods.XMGDuelMod.Scripts.Logger;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts.PlayerInfo
{
    public class AllPlayersState
    {
        public Dictionary<int, ClientData> IdToClientData { get; }
        public Dictionary<int, PlayerSpawnedInfo> IdToSpawedData { get; }
        public Dictionary<int, PlayerMatchmakingInfo> IdToMatchmakingInfo { get; }


        public AllPlayersState()
        {
            IdToClientData = new Dictionary<int, ClientData>();
            IdToSpawedData = new Dictionary<int, PlayerSpawnedInfo>();
            IdToMatchmakingInfo = new Dictionary<int, PlayerMatchmakingInfo>();
        }



        /// <summary>
        /// Removes the player data for all dictionaries assosiated with that players ID.
        /// </summary>
        public void RemovePlayerFromDictionaries(int id)
        {
            if (IdToClientData.ContainsKey(id)) { IdToClientData.Remove(id); }

            if (IdToSpawedData.ContainsKey(id)) { IdToSpawedData.Remove(id); }

            if (IdToMatchmakingInfo.ContainsKey(id)) { IdToMatchmakingInfo.Remove(id); }
            
        }

        /// <summary>
        /// Updates (or creates) PlayerSpawnedInfo for the player.
        /// </summary>
        public void UpdatePlayerSpawnedData(Ilogger logger, int playerId, FactionCountry playerFaction, PlayerClass playerClass, GameObject playerObject)
        {
            PlayerSpawnedInfo spawnedInfo = new PlayerSpawnedInfo(playerFaction, playerClass, playerObject);
            IdToSpawedData[playerId] = spawnedInfo;
            logger.LogDebug($"Adding {playerId} to the dictionary. Size is now {IdToSpawedData.Count}");
            
        }

        public void UpdateClientData(int playerId, ulong steamId, string playerName, string regimentTag, bool isBot)
        {
            ClientData clientData = new ClientData(playerId, steamId, playerName, regimentTag, isBot);
            IdToClientData[playerId] = clientData;
        }

        public void CreatePlayerMatchmakingInfo(int playerId, Ilogger logger)
        {
            IdToMatchmakingInfo[playerId] = Factory.CreateMatchMakingInfo(logger, playerId);
            
            logger.LogDebug($"Crated matchmaking info for player {playerId}. Size is {IdToMatchmakingInfo.Count}");
        }

        public void UpdatePlayerHealth(int playerId, byte newHp)
        {
            if (!IdToSpawedData.TryGetValue(playerId, out PlayerSpawnedInfo data)) {return;}
            data.PlayerHealth = newHp;
        }
    }
}