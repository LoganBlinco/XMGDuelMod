using UnityEngine;

namespace _mods.XMGDuelMod.Scripts._Core
{
    public class BaseSingleton<T> : MonoBehaviour where T : Object
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            Instance = FindObjectOfType<T>();
            if (Instance == null)
            {
                Debug.LogError($"Could not find an object of type {typeof(T)}");
            }
        }
    }
}