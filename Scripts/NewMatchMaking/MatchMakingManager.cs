using System.Collections.Generic;
using _mods.XMGDuelMod.Scripts._Core;
using _mods.XMGDuelMod.Scripts.ArenaReferences;
using _mods.XMGDuelMod.Scripts.Logger;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts
{
    public class MatchMakingManager : _Core.BaseSingleton<MatchMakingManager>
    {
        public IMatchMaking MatchMaking { get; set; } = null;

        
        [SerializeField] private List<ArenaDefinition> _areansForUse;
        public List<ArenaDefinition> AreansForUse => _areansForUse;
        private Ilogger Logger { get; set; }

        [SerializeField] private ArenaDefinition _moshPit;
        public ArenaDefinition MoshPit => _moshPit;

        public void BeginMatchMaking(Ilogger logger)
        {
            Logger = logger;
            Logger.LogDebug($"INside method");
            
            Settings settings = Settings.Instance;
            if (settings == null)
            {
                Settings.FindSingleton();
                settings = Settings.Instance;
                if (settings == null)
                {
                    Logger.LogError($"Settings is null");
                    return;
                }
            }
            Logger.LogDebug($"We found a settings object");
            float matchMakingFrequency = settings.MatchMakingFrequency;
            Logger.LogDebug($"{nameof(matchMakingFrequency)} has value {matchMakingFrequency}");
            InvokeRepeating(nameof(PerformMatchMaking), 1,matchMakingFrequency);
        }

        public void PerformMatchMaking()
        {
            Logger.LogDebug("Performing matchmaking");
            if (MatchMaking != null)
            {
                MatchMaking.PerformMatchMaking();
            }
        }
    }
}