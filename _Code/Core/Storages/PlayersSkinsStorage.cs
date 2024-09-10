using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Character;
using Fusion;
using Infrastructure.Network;
using UniRx;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Core.Storages
{
    public class PlayersSkinsStorage : IPlayersSkinsStorage, IPlayersSkinsStorageSetter
    {
        private Dictionary<byte, AssetReference> _modelsAssetReferencesById;
        private Dictionary<PlayerRef, byte> _playersSkinsId;
        private ReactiveProperty<byte> _localPlayerSkinId = new();
        private PlayerRef _localPlayerRef;
        
        private readonly INetworkConnectorCallbacksObserver _callbacksObserver;

        public IReadOnlyReactiveProperty<byte> ReadOnlyLocalPlayerSkinId => _localPlayerSkinId;
        public int ModelsCountInStorage => _modelsAssetReferencesById.Count;
        public event Action OnLocalPlayerSkinInitialized;
        
        public PlayersSkinsStorage(CharacterModelsList characterModelsList, INetworkConnectorCallbacksObserver callbacksObserver)
        {
            _callbacksObserver = callbacksObserver;
            _callbacksObserver.OnSetNewRoomConnection += OnSetNewNetworkConnection;
            InitializeCharacterModelsDict(characterModelsList);
        }

        public void SetPlayerSkin(PlayerRef playerRef, byte skinId)
        {
            _playersSkinsId[playerRef] = skinId;
            
            if (playerRef == _localPlayerRef)
            {
                _localPlayerSkinId.Value = skinId;
            }
        }

        public AssetReference GetSkinReferenceById(byte skinId)
        {
            return _modelsAssetReferencesById.GetValueOrDefault(skinId);
        }

        public byte GetPlayerSkinIdByRef(PlayerRef playerRef)
        {
            return _playersSkinsId.GetValueOrDefault(playerRef);
        }

        private void ClearPlayersSkinsDict()
        {
            _playersSkinsId.Clear();
        }

        private void InitializeCharacterModelsDict(CharacterModelsList characterModelsList)
        {
            _playersSkinsId = new Dictionary<PlayerRef, byte>();
            _modelsAssetReferencesById = new Dictionary<byte, AssetReference>();

            ReadOnlyCollection<AssetReference> modelsAssetReferences = characterModelsList.ModelsAssetReferences;
            byte modelId = 1;
            
            for (int i = 0; i < modelsAssetReferences.Count; i++)
            {
                _modelsAssetReferencesById.Add(modelId, modelsAssetReferences[i]);
                modelId++;
            }
        }
        
        private void OnSetNewNetworkConnection(NetworkRunner networkRunner)
        {
            _localPlayerRef = networkRunner.LocalPlayer;
            _callbacksObserver.NetworkCallbacksEvents.OnNetworkShutdown += ClearPlayersSkinsDict;
            _callbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerLeft += RemovePlayerSkinFromStorage;
            SetRandomizeLocalPlayerSkinWhenSetNewConnection(networkRunner.LocalPlayer);
        }

        private void RemovePlayerSkinFromStorage(PlayerRef playerRef)
        {
            if (_playersSkinsId.ContainsKey(playerRef))
            {
                _playersSkinsId.Remove(playerRef);
            }
        }

        private void SetRandomizeLocalPlayerSkinWhenSetNewConnection(PlayerRef currentPlayerRef)
        {
            int currentSkinId = Random.Range(1, ModelsCountInStorage + 1);
            SetPlayerSkin(currentPlayerRef, (byte)currentSkinId);
            OnLocalPlayerSkinInitialized?.Invoke();
        }
    }
}
