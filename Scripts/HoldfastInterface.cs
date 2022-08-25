using _mods.XMGDuelMod.Scripts._Core;
using HoldfastSharedMethods;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts
{
    public class HoldfastInterface : IHoldfastSharedMethods
    {
        private MainController _mainController;

        private bool _isServer = false;
        private bool _isClient = false;
        
        public void OnIsServer(bool server)
        {
            _mainController = Factory.CreateMainController();
            _isServer = server;
        }
        public void OnIsClient(bool client, ulong steamId)
        {
            _isClient = client;
        }
        
        public void OnPlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot)
        {
            if (!_isServer){return;}

            _mainController.OnPlayerJoined(playerId, steamId, name, regimentTag, isBot);
        }

        public void OnPlayerLeft(int playerId)
        {
            _mainController.OnPlayerLeft(playerId);
        }

        public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
        {
            _mainController.OnPlayerHurt(playerId, oldHp, newHp);
            throw new System.NotImplementedException();
        }
        
        public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass,
            int uniformId, GameObject playerObject)
        {
            _mainController.OnPlayerSpawned(playerId, playerFaction, playerClass, playerObject);
        }
        
        
        public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction,
            FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType)
        {
            _mainController.OnRoundDetails(serverName, mapName, attackingFaction, defendingFaction, gameplayMode,
                gameType);
        }
        
        
        
        
        
        public void OnSyncValueState(int value)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdateSyncedTime(double time)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdateElapsedTime(float time)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdateTimeRemaining(float time)
        {
            throw new System.NotImplementedException();
        }






        public void PassConfigVariables(string[] value)
        {
            throw new System.NotImplementedException();
        }





        public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string details)
        {
            throw new System.NotImplementedException();
        }

        public void OnScorableAction(int playerId, int score, ScorableActionType reason)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerShoot(int playerId, bool dryShot)
        {
            throw new System.NotImplementedException();
        }

        public void OnShotInfo(int playerId, int shotCount, Vector3[][] shotsPointsPositions, float[] trajectileDistances,
            float[] distanceFromFiringPositions, float[] horizontalDeviationAngles, float[] maxHorizontalDeviationAngles,
            float[] muzzleVelocities, float[] gravities, float[] damageHitBaseDamages, float[] damageRangeUnitValues,
            float[] damagePostTraitAndBuffValues, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections,
            int[] hitPlayerIds, int[] hitDamageableObjectIds, int[] hitShipIds, int[] hitVehicleIds)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerBlock(int attackingPlayerId, int defendingPlayerId)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerMeleeStartSecondaryAttack(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerWeaponSwitch(int playerId, string weapon)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerStartCarry(int playerId, CarryableObjectType carryableObject)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerEndCarry(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerShout(int playerId, CharacterVoicePhrase voicePhrase)
        {
            throw new System.NotImplementedException();
        }

        public void OnConsoleCommand(string input, string output, bool success)
        {
            throw new System.NotImplementedException();
        }

        public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
        {
            throw new System.NotImplementedException();
        }

        public void OnRCCommand(int playerId, string input, string output, bool success)
        {
            throw new System.NotImplementedException();
        }

        public void OnTextMessage(int playerId, TextChatChannel channel, string text)
        {
            throw new System.NotImplementedException();
        }

        public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason)
        {
            throw new System.NotImplementedException();
        }

        public void OnDamageableObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteractableObjectInteraction(int playerId, int interactableObjectId, GameObject interactableObject,
            InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex)
        {
            throw new System.NotImplementedException();
        }

        public void OnEmplacementPlaced(int itemId, GameObject objectBuilt, EmplacementType emplacementType)
        {
            throw new System.NotImplementedException();
        }

        public void OnEmplacementConstructed(int itemId)
        {
            throw new System.NotImplementedException();
        }

        public void OnCapturePointCaptured(int capturePoint)
        {
            throw new System.NotImplementedException();
        }

        public void OnCapturePointOwnerChanged(int capturePoint, FactionCountry factionCountry)
        {
            throw new System.NotImplementedException();
        }

        public void OnCapturePointDataUpdated(int capturePoint, int defendingPlayerCount, int attackingPlayerCount)
        {
            throw new System.NotImplementedException();
        }

        public void OnBuffStart(int playerId, BuffType buff)
        {
            throw new System.NotImplementedException();
        }

        public void OnBuffStop(int playerId, BuffType buff)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoundEndFactionWinner(FactionCountry factionCountry, FactionRoundWinnerReason reason)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoundEndPlayerWinner(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public void OnVehicleSpawned(int vehicleId, FactionCountry vehicleFaction, PlayerClass vehicleClass, GameObject vehicleObject,
            int ownerPlayerId)
        {
            throw new System.NotImplementedException();
        }

        public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerKilledVehicle(int killerPlayerId, int victimVehicleId, EntityHealthChangedReason reason, string details)
        {
            throw new System.NotImplementedException();
        }

        public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry shipfaction, ShipType shipType, int shipName)
        {
            throw new System.NotImplementedException();
        }

        public void OnShipDamaged(int shipId, int oldHp, int newHp)
        {
            throw new System.NotImplementedException();
        }
    }
}