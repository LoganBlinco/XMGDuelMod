using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts.PlayerInfo
{
    public class PlayerSpawnedInfo
    {
        public FactionCountry PlayerFaction { get; set; }
        public PlayerClass PlayerClass { get; set; }
        public GameObject PlayerObject { get; set; }

        public byte PlayerHealth { get; set; }
        
        public PlayerSpawnedInfo(FactionCountry playerFaction, PlayerClass playerClass,
            GameObject playerObject)
        {
            PlayerFaction = playerFaction;
            PlayerClass = playerClass;
            PlayerObject = playerObject;
            PlayerHealth = 100;
        }
    }
}