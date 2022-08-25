namespace _mods.XMGDuelMod.Scripts.Logger
{
    public interface Ilogger
    {
        public void Log(string message);
        public void LogWarning(string message);
        public void LogError(string message);
        public void LogDebug(string message);
    }
}