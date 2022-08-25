using UnityEngine;
using UnityEngine.UI;

namespace _mods.XMGDuelMod.Scripts.Console
{
    public interface IConsole
    {
        
        public InputField InputField { get; set; }

        public bool IsValid { get; }


        public void Invoke(string command);

        public void Broadcast(string message);
        
        public void BroadCastPrefix(string message);
        
        public void PrivateMessage(int playerID, string message);
        public void PrivateMessageDelayed(int playerID, string message, int delay);
        
        public void Slap(int playerID, int damage);
        
        public void TeleportPlayerToPosition(int playerId, Vector3 location);

        public void HealPlayer(int playerId, byte currentHp);

    }
}