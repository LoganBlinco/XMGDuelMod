using UnityEngine;

namespace _mods.XMGDuelMod.Scripts.ArenaReferences
{
    public interface IArenaDefinition
    {


        public Vector3 GetNextDefenderSpawn();

        public Vector3 GetNextAttackerSpawn();
    }
}