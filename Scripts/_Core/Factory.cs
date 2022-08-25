using System;
using _mods.XMGDuelMod.Scripts.Console;
using _mods.XMGDuelMod.Scripts.Enums;
using _mods.XMGDuelMod.Scripts.Logger;
using _mods.XMGDuelMod.Scripts.PlayerInfo;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public static class Factory
    {

        public static Ilogger CreateLogger()
        {
            Settings instance = Settings.Instance;
            if (instance == null){Debug.LogError($"{nameof(Settings)} has no reference. Error in factory");throw new NullReferenceException();}
            
            DebugMode debugMode = instance.DebugMode;
            switch (debugMode)
            {
                case DebugMode.Debug: return new DebugLogger();
                case DebugMode.Normal: return new NormalLogger();
                default:
                    Debug.LogError($"Could not find a logger type for {debugMode}");
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static IConsole CreateConsole(Ilogger logger)
        {
            return new ConsoleManager(logger);
        }
        

        public static MainController CreateMainController()
        {
            Ilogger logger = CreateLogger();
            IConsole console = CreateConsole(logger);


            return new MainController(logger,console);
        }

        public static PlayerMatchmakingInfo CreateMatchMakingInfo(Ilogger logger, int playerId)
        {
            MatchMakingPriority priority = Settings.Instance.StartingMatchMakingPriority;
            return new PlayerMatchmakingInfo(priority, logger, playerId);
        }
    }
}