namespace _mods.XMGDuelMod.Scripts.PlayerInfo
{
    public class ClientData
    {
        public int PlayerID { get; }
        public ulong SteamId { get; }
        public string PlayerName { get; }
        public string RegimentTag { get; }
        public bool IsBot { get; }
        
        
        public ClientData(int playerID, ulong steamId, string playerName, string regimentTag, bool isBot)
        {
            PlayerID = playerID;
            SteamId = steamId;
            PlayerName = playerName;
            IsBot = isBot;
            if (regimentTag == "")
            {
                RegimentTag = "NoReg";
            }
            else
            {
                RegimentTag = regimentTag;
            }
        }
    }
}