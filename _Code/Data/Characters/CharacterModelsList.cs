using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Character
{
    [CreateAssetMenu(menuName = "Data/Characters/Create Character Models List", fileName = "CharacterModelsList")]
    public class CharacterModelsList : ScriptableObject
    {
        [SerializeField, AssetsOnly] 
        private List<AssetReference> _modelsAssetReferences;

        public ReadOnlyCollection<AssetReference> ModelsAssetReferences => new ReadOnlyCollection<AssetReference>(_modelsAssetReferences);
    }
}
