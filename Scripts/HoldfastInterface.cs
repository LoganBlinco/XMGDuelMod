using _mods.XMGDuelMod.Scripts._Core;
using _mods.XMGDuelMod.Scripts.Console;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts
{
    public class HoldfastInterface : IHoldfastSharedMethods
    {
        private MainController _mainController;

        private bool _isServer = true;
        private bool _isClient = false;
        
        public void OnIsServer(bool server)
        {
            AssignController();
            Debug.Log($"Is server assinged {server}");
            _isServer = server;
        }

        private void AssignController()
        {
            _mainController = Factory.CreateMainController();
            Debug.Log("Assigned controller");
        }

        public void OnIsClient(bool client, ulong steamId)
        {
            Debug.Log($"Is client assinged {client}");
            _isClient = client;
        }
        
        public void OnPlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot)
        {
            if (!_isServer){return;}
            _mainController.OnPlayerJoined(playerId, steamId, name, regimentTag, isBot);
        }

        public void OnPlayerLeft(int playerId)
        {
            if (!_isServer){return;}
            _mainController.OnPlayerLeft(playerId);
        }

        public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
        {
            if (!_isServer){return;}
            _mainController.OnPlayerHurt(playerId, oldHp, newHp);
        }
        
        public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass,
            int uniformId, GameObject playerObject)
        {
            if (!_isServer){return;}
            _mainController.OnPlayerSpawned(playerId, playerFaction, playerClass, playerObject);
        }
        
        
        public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction,
            FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType)
        {
            if (!_isServer){return;}
            _mainController.OnRoundDetails(serverName, mapName, attackingFaction, defendingFaction, gameplayMode,
                gameType);
        }
        
        
        public void PassConfigVariables(string[] value)
        {
            Debug.Log($"PAssocnfig variables is seerver {_isServer}");
            
            if (!_isServer){return;}

            if (_mainController == null)
            {
                AssignController();
            }
            _mainController.PassConfigVariables(value);
        }
        
        public void OnUpdateTimeRemaining(float time)
        {
            ConsoleManager.CurrentTime = time;
        }
        
        public void OnSyncValueState(int value)
        {
        }

        public void OnUpdateSyncedTime(double time)
        {
        }

        public void OnUpdateElapsedTime(float time)
        {
        }













        public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string details)
        {
            if (!_isServer){return;}
            _mainController.OnPlayerKilledPlayer(killerPlayerId, victimPlayerId);
        }

        public void OnScorableAction(int playerId, int score, ScorableActionType reason)
        {
        }

        public void OnPlayerShoot(int playerId, bool dryShot)
        {
        }

        public void OnShotInfo(int playerId, int shotCount, Vector3[][] shotsPointsPositions, float[] trajectileDistances,
            float[] distanceFromFiringPositions, float[] horizontalDeviationAngles, float[] maxHorizontalDeviationAngles,
            float[] muzzleVelocities, float[] gravities, float[] damageHitBaseDamages, float[] damageRangeUnitValues,
            float[] damagePostTraitAndBuffValues, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections,
            int[] hitPlayerIds, int[] hitDamageableObjectIds, int[] hitShipIds, int[] hitVehicleIds)
        {
        }

        public void OnPlayerBlock(int attackingPlayerId, int defendingPlayerId)
        {
        }

        public void OnPlayerMeleeStartSecondaryAttack(int playerId)
        {
        }

        public void OnPlayerWeaponSwitch(int playerId, string weapon)
        {
        }

        public void OnPlayerStartCarry(int playerId, CarryableObjectType carryableObject)
        {
        }

        public void OnPlayerEndCarry(int playerId)
        {
        }

        public void OnPlayerShout(int playerId, CharacterVoicePhrase voicePhrase)
        {
        }

        public void OnConsoleCommand(string input, string output, bool success)
        {
        }

        public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
        {
        }

        public void OnRCCommand(int playerId, string input, string output, bool success)
        {
        }

        public void OnTextMessage(int playerId, TextChatChannel channel, string text)
        {
        }

        public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason)
        {
        }

        public void OnDamageableObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp)
        {
        }

        public void OnInteractableObjectInteraction(int playerId, int interactableObjectId, GameObject interactableObject,
            InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex)
        {
        }

        public void OnEmplacementPlaced(int itemId, GameObject objectBuilt, EmplacementType emplacementType)
        {
        }

        public void OnEmplacementConstructed(int itemId)
        {
        }

        public void OnCapturePointCaptured(int capturePoint)
        {
        }

        public void OnCapturePointOwnerChanged(int capturePoint, FactionCountry factionCountry)
        {
        }

        public void OnCapturePointDataUpdated(int capturePoint, int defendingPlayerCount, int attackingPlayerCount)
        {
        }

        public void OnBuffStart(int playerId, BuffType buff)
        {
        }

        public void OnBuffStop(int playerId, BuffType buff)
        {
        }

        public void OnRoundEndFactionWinner(FactionCountry factionCountry, FactionRoundWinnerReason reason)
        {
        }

        public void OnRoundEndPlayerWinner(int playerId)
        {
        }

        public void OnVehicleSpawned(int vehicleId, FactionCountry vehicleFaction, PlayerClass vehicleClass, GameObject vehicleObject,
            int ownerPlayerId)
        {
        }

        public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
        {
        }

        public void OnPlayerKilledVehicle(int killerPlayerId, int victimVehicleId, EntityHealthChangedReason reason, string details)
        {
        }

        public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry shipfaction, ShipType shipType, int shipName)
        {
        }

        public void OnShipDamaged(int shipId, int oldHp, int newHp)
        {
        }
    }
}