using _mods.XMGDuelMod.Scripts.Enums;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public class Settings : BaseSingleton<Settings>
    {
        [SerializeField] private DebugMode _debugMode = DebugMode.Debug;
        public DebugMode DebugMode
        {
            get => _debugMode;
            private set => _debugMode = value;
        }

        [SerializeField] private MatchMakingPriority _startingMatchMakingPriority = MatchMakingPriority.All;
        public MatchMakingPriority StartingMatchMakingPriority
        {
            get => _startingMatchMakingPriority;
            private set => _startingMatchMakingPriority = value;
        }

        [SerializeField] private string _duelStartMessage = "Begin! Qapla'";
        public string DuelStartMessage
        {
            get => _duelStartMessage;
            private set => _duelStartMessage = value;
        }

        [SerializeField] private int _delayForTeleport = 2;
        public int DelayForTeleport
        {
            get => _delayForTeleport;
            private set => _delayForTeleport = value;
        }

        
        [SerializeField] private int _delayForTeleportShort = 1;
        public int DelayForTeleportShort
        {
            get => _delayForTeleport;
            private set => _delayForTeleport = value;
        }
        
        
        [SerializeField] private int _delayForMessage = 2;
        public int DelayForMessage
        {
            get => _delayForMessage;
            private set => _delayForMessage = value;
        }

        [SerializeField] private int _roundsNeededForWin = 2;
        public int RoundsNeededForWin
        {
            get => _roundsNeededForWin;
            private set => _roundsNeededForWin = value;
        }

        [SerializeField] private bool _shouldPerformMatchmaking = true;
        public bool ShouldPerformMatchmaking
        {
            get => _shouldPerformMatchmaking;
            private set => _shouldPerformMatchmaking = value;
        }


        
        [SerializeField] private float _matchMakingFrequency = 15;
        public float MatchMakingFrequency
        {
            get => _matchMakingFrequency;
            private set
            {
                if (value >= 1f) { _matchMakingFrequency = value;}
                else
                {
                    Debug.LogError($"Value input for {nameof(MatchMakingFrequency)} is too small. Input {value}");
                }
            }
        }

        public void PassConfigVariables(string[] value)
        {
            //TODFO IMplement.
        }
    }
}