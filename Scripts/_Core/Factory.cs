using System;
using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.Logger;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public static class Factory
    {
        private static Ilogger CreateLogger()
        {
            Settings instance = Settings.Instance;
            if (instance == null)
            {
                Debug.LogError($"{nameof(Settings)} has no reference. Error in factory");
                throw new NullReferenceException();
            }

            DebugMode debugMode = instance.DebugMode;
            switch (debugMode)
            {
                case DebugMode.Debug: 
                    Debug.Log("Using debug logger");
                    return new DebugLogger();
                case DebugMode.Normal: 
                    Debug.Log("Using normal logger");
                    return new NormalLogger();
                default:
                    Debug.LogError($"Could not find a logger type for {debugMode}");
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IConsole CreateConsole(Ilogger logger)
        {
            return new ConsoleManager(logger);
        }


        public static MainController CreateMainController()
        {
            Ilogger logger = CreateLogger();
            IConsole console = CreateConsole(logger);
            AllPlayersState allPlayersStates = new AllPlayersState();

            return new MainController(logger, console, allPlayersStates);
        }

        public static IMatchMaking CreateMatchMakingRefactored(Ilogger logger, IConsole console,
            AllPlayersState playerStates, RoundState roundState)
        {
            List<ArenaDefinition> arenasForUse = MatchMakingManager.Instance.AreansForUse;
            ArenaDefinition moshPit = MatchMakingManager.Instance.MoshPit;
            IMatchMakingQueue queueHandler = new MatchMakingQueue(playerStates, logger, roundState, arenasForUse, moshPit,console);
            IMatchMakingAction actionHandler = new MatchMakingActions();
            
            IMatchMaking matchMaking = new MatchMakingService(console,queueHandler, actionHandler, playerStates, roundState);

            return matchMaking;
        }

        public static PlayerMatchmakingInfo CreateMatchMakingInfo(Ilogger logger, int playerId)
        {
            MatchMakingPriority priority = Settings.Instance.StartingMatchMakingPriority;
            return new PlayerMatchmakingInfo(priority, logger, playerId);
        }

        public static Match CreateNewMatch(IMatchMaking matchMakingService, ArenaDefinition arenaToUse, List<int> attackingPlayers,
            List<int> defendingPlayers)
        {
            Settings settings = Settings.Instance;
            Ilogger logger = CreateLogger();
            
            int roundsNeededForWin = settings.RoundsNeededForWin;
            return new Match(logger, matchMakingService, arenaToUse, attackingPlayers, defendingPlayers,
                roundsNeededForWin, settings);
        }
    }
}