using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.Logger;
using _mods.XMGDuelMod.Scripts.PlayerInfo;

namespace _mods.XMGDuelMod.Scripts
{
    public class DefaultMMQueue : IMatchMakingQueue
    {
        private List<PlayerMatchmakingInfo> _attackingFactionDuelQueue;
        private List<PlayerMatchmakingInfo> _defendingFactionDuelQueue;
        
        private List<PlayerMatchmakingInfo> _attackingFactionGroupfightQueue;
        private List<PlayerMatchmakingInfo> _defendingFactionGroupfightQueue;


        public RoundState RoundState { get; }
        
        public Ilogger Logger { get;}
        
        public void RegisterPlayerForQueue(PlayerMatchmakingInfo player)
        {
            switch (player.MatchMakingPriority)
            {
                case MatchMakingPriority.All:
                    AddPlayerToDuelQueue(player);
                    AddPlayerToGroupfightQueue(player);
                    return;
                case MatchMakingPriority.Duels:
                    AddPlayerToDuelQueue(player);
                    return;
                case MatchMakingPriority.Groupfights:
                    AddPlayerToGroupfightQueue(player);
                    return;
                case MatchMakingPriority.None:
                    return;
                default: 
                    Logger.LogError($"{nameof(RegisterPlayerForQueue)} could not fine type {player.MatchMakingPriority}");
                    return;
            }
        }

        private void AddPlayerToDuelQueue(PlayerMatchmakingInfo player)
        {
            if (player.PlayerSpawnedInfo.PlayerFaction == RoundState.AttackingFaction)
            {
                if (!_attackingFactionDuelQueue.Contains(player))
                {
                    _attackingFactionDuelQueue.Add(player);
                    
                }
                if (_defendingFactionDuelQueue.Contains(player))
                {
                    _defendingFactionDuelQueue.Remove(player);
                }
            }
            else if (player.PlayerSpawnedInfo.PlayerFaction == RoundState.DefendingFaction)
            {
                if (!_defendingFactionDuelQueue.Contains(player))
                {
                    _defendingFactionDuelQueue.Add(player);
                }
                if (_attackingFactionDuelQueue.Contains(player))
                {
                    _attackingFactionDuelQueue.Remove(player);
                }
            }
        }
        private void AddPlayerToGroupfightQueue(PlayerMatchmakingInfo player)
        {
            if (player.PlayerSpawnedInfo.PlayerFaction == RoundState.AttackingFaction)
            {
                if (!_attackingFactionGroupfightQueue.Contains(player))
                {
                    _attackingFactionGroupfightQueue.Add(player);
                }
                if (_defendingFactionGroupfightQueue.Contains(player))
                {
                    _defendingFactionGroupfightQueue.Remove(player);
                }
            }
            else if (player.PlayerSpawnedInfo.PlayerFaction == RoundState.DefendingFaction)
            {
                if (!_defendingFactionGroupfightQueue.Contains(player))
                {
                    _defendingFactionGroupfightQueue.Add(player);
                }
                if (_attackingFactionGroupfightQueue.Contains(player))
                {
                    _attackingFactionGroupfightQueue.Remove(player);
                }
            }
        }

        public void RemovePlayerFromQueue(PlayerMatchmakingInfo player)
        {
            RemovePlayerFromDuelQueue(player);
            RemovePlayerFromGroupfightQueue(player);

        }

        private void RemovePlayerFromDuelQueue(PlayerMatchmakingInfo player)
        {
            if (_attackingFactionDuelQueue.Contains(player))
            {
                _attackingFactionDuelQueue.Remove(player);
            }

            if (_defendingFactionDuelQueue.Contains(player))
            {
                _defendingFactionDuelQueue.Remove(player);
            }
        }
        private void RemovePlayerFromGroupfightQueue(PlayerMatchmakingInfo player)
        {
            if (_attackingFactionGroupfightQueue.Contains(player))
            {
                _attackingFactionGroupfightQueue.Remove(player);
            }

            if (_defendingFactionGroupfightQueue.Contains(player))
            {
                _defendingFactionGroupfightQueue.Remove(player);
            }
        }
    }
}