using System;
using _mods.XMGDuelMod.Scripts.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace _mods.XMGDuelMod.Scripts.Console
{
    public class ConsoleManager : IConsole
    {
        private const string MESSAGE_PREFIX = "DUELBOT: ";
        public InputField InputField { get; set; }
        
        public bool IsValid => InputField == null;


        private Ilogger Logger { get; }
        

        public ConsoleManager(Ilogger logger)
        {
            Logger = logger;

            InputField = FindConsole();
        }

        private InputField FindConsole()
        {
            //Code from Wrex: https://github.com/CM2Walki/HoldfastMods/blob/master/NoShoutsAllowed/NoShoutsAllowed.cs

            //Get all the canvas items in the game
            Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();

            foreach (Canvas can in canvases)
            {
                //Find the one that's called "Game Console Panel"
                if (String.Compare(can.name, "Game Console Panel", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //Inside this, now we need to find the input field where the player types messages.
                    return can.GetComponentInChildren<InputField>(true);
                }
            }
            Logger.LogError($"Could not find the console");
            return null;
        }
        
        
        
        public void Invoke(string command)
        {
            if (!IsValid)
            {
                Logger.LogError($"We did not find the console");
                return;
            }
            Logger.Log($"DuelBot is inputting command {command}");
            InputField.onEndEdit.Invoke(command);
        }

        public void Broadcast(string message)
        {
            Invoke($"broadcast {message}");
        }

        public void BroadCastPrefix(string message)
        {
            Invoke($"broadcast {MESSAGE_PREFIX}{message}");
        }

        public void PrivateMessage(int playerID, string message)
        {
            Invoke($"serverAdmin privateMessage {playerID} {message}");
        }

        public void PrivateMessageDelayed(int playerID, string message, int delay)
        {
            throw new NotImplementedException();
        }

        public void Slap(int playerID, int damage)
        {
            Invoke($"serverAdmin slap {playerID} {damage}");
        }
        
        public void TeleportPlayerToPosition(int playerId, Vector3 location)
        {
            Invoke($"teleport {playerId} {location.x},{location.y},{location.z}");
        }

        public void HealPlayer(int playerId, byte currentHp)
        {
            if (currentHp <= 0 ){return;}

            byte amountToHeal = (byte) (100 - currentHp);
            Invoke($"serverAdmin slap {playerId} {-amountToHeal}");
        }
    }
}