using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using HoldfastSharedMethods;
using UnityEditor;

namespace _mods.XMGDuelMod.Scripts
{
    public class MatchMakingService : IMatchMakingService
    {
        public FactionCountry AttackingFaction { get; }
        public FactionCountry DefendingFaction { get; }
        public AllPlayersState PlayerStates { get; }
        
        public IMatchMakingQueue MatchMakingQueue { get;}
        
        private Dictionary<int, Match> _playerIdToMatch;
        private Dictionary<Match, List<PlayerMatchmakingInfo>> _matchToPlayerInfo;



        public void CreateMatch()
        {
            throw new System.NotImplementedException();
        }

        public void EndMatch(Match matchEnded)
        {
            if (!_matchToPlayerInfo.TryGetValue(matchEnded, out List<PlayerMatchmakingInfo> playersFromMatch)){return;}
            foreach (PlayerMatchmakingInfo playerMatchmakingInfo in playersFromMatch)
            {
                MatchMakingQueue.RegisterPlayerForQueue(playerMatchmakingInfo);
                _playerIdToMatch.Remove(playerMatchmakingInfo.PlayerId);
            }
            _matchToPlayerInfo.Remove(matchEnded);
        }

        public void PlayerSpawned(int playerId)
        {
            if (!_playerIdToMatch.TryGetValue(playerId, out Match match)){return;}

            match.PlayerSpawned(playerId);

            throw new System.NotImplementedException();
        }

        public void PlayerKilledPlayer(int killerPlayerId, int victimPlayerID)
        {
            if (!_playerIdToMatch.TryGetValue(killerPlayerId, out Match match)){return;}

            if (!match.HasPlayer(victimPlayerID)){return;}

            match.PlayerKiledPlayer(killerPlayerId, victimPlayerID);
        }
    }
}