using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts._Core;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Console;

namespace _mods.XMGDuelMod.Scripts
{
    public class MatchMakingActions : IMatchMakingAction
    {
        private Dictionary<int, Match> _playerIdToMatch;
        private Dictionary<Match, List<int>> _matchToPlayerInfo;


        public MatchMakingActions()
        {
            _playerIdToMatch = new Dictionary<int, Match>();
            _matchToPlayerInfo = new Dictionary<Match, List<int>>();
        }
        public void CreateMatch(IMatchMaking matchMaking, ArenaDefinition arenaToUse, List<int> attackingPlayers, List<int> defendingPlayers)
        {
            Match newMatch = Factory.CreateNewMatch(matchMaking, arenaToUse, attackingPlayers, defendingPlayers);
            
            _matchToPlayerInfo[newMatch] = new List<int>();
            AddMatchAssosiation(attackingPlayers,newMatch);
            AddMatchAssosiation(defendingPlayers,newMatch);

        }

        private void AddMatchAssosiation(List<int> playerIds, Match match)
        {
            foreach (int playerId in playerIds)
            {
                _playerIdToMatch[playerId] = match;
                _matchToPlayerInfo[match].Add(playerId);
            }
        }
        

        public void EndMatch(IMatchMaking matchMaking, Match matchEnded)
        {
            if (!_matchToPlayerInfo.TryGetValue(matchEnded, out List<int> playersFromMatch)){return;}
            foreach (int playerId in playersFromMatch)
            {
                matchMaking.RegisterPlayerForQueue(playerId);
                _playerIdToMatch.Remove(playerId);
            }
            _matchToPlayerInfo.Remove(matchEnded);
        }

        public void PlayerSpawned(int playerId)
        {
            if (!_playerIdToMatch.TryGetValue(playerId, out Match match)){return;}
            
            
            
            match.PlayerSpawned(playerId);
        }

        public void PlayerKilledPlayer(int killerPlayerId, int victimPlayerID)
        {
            if (!_playerIdToMatch.TryGetValue(killerPlayerId, out Match match)){return;}

            if (!match.HasPlayer(victimPlayerID)){return;}

            match.PlayerKiledPlayer(killerPlayerId, victimPlayerID);
        }

        public void PlayerLeft(int playerId)
        {
            if (!_playerIdToMatch.TryGetValue(playerId, out Match match)){return;}

            match.PlayerLeftServer(playerId);
            RemoveMatchAssosiactionToPlayer(playerId, match);
        }

        private void RemoveMatchAssosiactionToPlayer(int playerId, Match match)
        {
            _playerIdToMatch.Remove(playerId);
            if (_matchToPlayerInfo.TryGetValue(match, out List<int> playersInMatch))
            {
                playersInMatch.Remove(playerId);
            }
        }

        public void PlayerSwappedFaction(int playerId)
        {
            if (!_playerIdToMatch.TryGetValue(playerId, out Match match)){return;} //if they are not in a match. We dont care.

            match.PlayerSwappedFaction(playerId);

            RemoveMatchAssosiactionToPlayer(playerId, match);
        }
    }
}