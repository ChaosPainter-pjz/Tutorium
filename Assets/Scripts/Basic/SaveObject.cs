using SaveManager.Scripts;
using UnityEngine;

namespace Basic
{
    [CreateAssetMenu]
    public class SaveObject : ScriptableObject
    {
        public SaveData SaveData;
        public OverSaveData OverSaveData;
    }
}