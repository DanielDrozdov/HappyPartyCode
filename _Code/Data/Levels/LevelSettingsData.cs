using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Levels
{
    [CreateAssetMenu(menuName = "Data/Levels/Create Level Settings", fileName = "LevelSettingsData")]
    public class LevelSettingsData : ScriptableObject
    {
        [field: SerializeField] 
        public AssetReference SceneRef { get; private set; }
        
        [field: SerializeField, AssetsOnly] 
        public GameObject PlayerPrb { get; private set; }
    }
}
