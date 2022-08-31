using System.Collections.Generic;
using UnityEngine;

namespace _mods.XMGDuelMod.Scripts.ArenaReferences
{
    public class ArenaDefinition : MonoBehaviour
    {
        [SerializeField] private List<Transform> _attackerSpawns = new List<Transform>();
        [SerializeField] private List<Transform> _defenderSpawns = new List<Transform>();
        
        [SerializeField] private List<Transform> _moshPitSpawns = new List<Transform>();

        private List<Transform> AttackerSpawns => _attackerSpawns;
        private List<Transform> DefenderSpawns => _defenderSpawns;
        private List<Transform> MoshPitSpawns => _moshPitSpawns;


        private int _currentDefenderIndexPointer = 0;
        private int CurrentDefenderIndexPointer
        {
            get => _currentDefenderIndexPointer;
            set => _currentDefenderIndexPointer = value % DefenderSpawns.Count;
        }
        
        private int _currentAttackerIndexPointer = 0;
        private int CurrentAttackerIndexPointer
        {
            get => _currentAttackerIndexPointer;
            set => _currentAttackerIndexPointer = value % AttackerSpawns.Count;
        }
        private int _currentAMoshPitIndexPointer = 0;
        private int CurrentMoshPitIndexPointer
        {
            get => _currentAMoshPitIndexPointer;
            set => _currentAMoshPitIndexPointer = value % MoshPitSpawns.Count;
        }
        
        public Vector3 GetNextDefenderSpawn()
        {
            CurrentDefenderIndexPointer += 1;
            return DefenderSpawns[CurrentDefenderIndexPointer].position;
        }

        public Vector3 GetNextAttackerSpawn()
        {
            CurrentAttackerIndexPointer += 1;
            return AttackerSpawns[CurrentAttackerIndexPointer].position;
        }

        public Vector3 GetMoshPitSpawn()
        {
            int index = Random.Range(0, MoshPitSpawns.Count);
            return MoshPitSpawns[index].position;
        }
    }
}