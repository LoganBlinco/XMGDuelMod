using UnityEngine;

namespace _mods.XMGDuelMod.Scripts.Logger
{
    public class DebugLogger : Ilogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogDebug(string message)
        {
            Debug.Log(message);
        }
    }
}