using System.Collections.Generic;
using HoldfastSharedMethods;

namespace _mods.XMGDuelMod.Scripts.PlayerInfo
{
    public class RoundState
    {
        public string ServerName { get; }
        public string MapName { get; }
        public GameplayMode GameplayMode { get; }
        public GameType GameType { get; }
        public FactionCountry AttackingFaction { get; }
        public FactionCountry DefendingFaction { get; }
        
        public RoundState(string serverName, string mapName, GameplayMode gameplayMode, GameType gameType, FactionCountry attackingFaction, FactionCountry defendingFaction)
        {
            ServerName = serverName;
            MapName = mapName;
            GameplayMode = gameplayMode;
            GameType = gameType;
            AttackingFaction = attackingFaction;
            DefendingFaction = defendingFaction;
        }
    }
}