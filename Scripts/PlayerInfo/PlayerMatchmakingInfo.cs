using System;
using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.Logger;

namespace _mods.XMGDuelMod.Scripts.PlayerInfo
{
    public class PlayerMatchmakingInfo
    {


        public MatchMakingPriority MatchMakingPriority { get; set; }
        
        public PlayerState PlayerState { get; set; }

        public int PlayerId { get; }
        private Ilogger Logger { get; }
        
        public PlayerMatchmakingInfo(MatchMakingPriority matchMakingPriority, Ilogger logger, int playerId)
        {
            MatchMakingPriority = matchMakingPriority;
            PlayerState = PlayerState.Idle;
            Logger = logger;

            PlayerId = playerId;
        }

        public void PlayerRespawned()
        {
            switch (PlayerState)
            {
                case PlayerState.Idle:
                    return;
                case PlayerState.InQueue: 
                    return;
                case PlayerState.InDuel: 
                    //Telelprot player back
                    break;
                case PlayerState.InGroupFight: 
                    //Teleport player back
                    break;
                default: 
                    Logger.LogError($"We have no {nameof(PlayerRespawned)}() for state {PlayerState}");
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}