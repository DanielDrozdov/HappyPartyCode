using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Network
{
    [CreateAssetMenu(menuName = "Data/Network/Create Room Settings", fileName = "NetworkRoomSettings")]
    public class NetworkRoomSettings : ScriptableObject
    {
        [field: SerializeField]
        public int MaxPlayers { get; private set; }
        
        [field: SerializeField]
        public AssetReference MainMenuScenePath { get; private set; }

        [field: SerializeField]
        public GameObject PodiumPlayerPrb { get; private set; }
    }
}
