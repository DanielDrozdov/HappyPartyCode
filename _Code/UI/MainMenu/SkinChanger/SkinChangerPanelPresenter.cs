using Core.Storages;
using Infrastructure.Network;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.SkinChanger
{
    public class SkinChangerPanelPresenter : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _root;
        
        private IPlayersSkinsStorage _playersSkinsStorage;
        private IPlayersSkinsStorageSetter _playersSkinsStorageSetter;
        private INetworkActiveRunnerInfoProvider _networkActiveRunnerInfoProvider;
        private INetworkConnectorCallbacksObserver _callbacksObserver;
        private int _currentSkinId;
        private int _skinsCountInStorage;

        [Inject]
        private void Construct(IPlayersSkinsStorage playersSkinsStorage, IPlayersSkinsStorageSetter playersSkinsStorageSetter,
            INetworkActiveRunnerInfoProvider networkActiveRunnerInfoProvider)
        {
            _playersSkinsStorage = playersSkinsStorage;
            _playersSkinsStorageSetter = playersSkinsStorageSetter;
            _networkActiveRunnerInfoProvider = networkActiveRunnerInfoProvider;
        }

        private void Awake()
        {
            _skinsCountInStorage = _playersSkinsStorage.ModelsCountInStorage;
            _playersSkinsStorage.OnLocalPlayerSkinInitialized += SyncLocalPlayerSkinId;
        }

        private void OnDestroy()
        {
            _playersSkinsStorage.OnLocalPlayerSkinInitialized -= SyncLocalPlayerSkinId;
        }

        public void Switch(bool value)
        {
            _root.SetActive(value);
        }

        public void SelectNextSkin()
        {
            ChangeSkin(1);
        }

        public void SelectPreviousSkin()
        {
            ChangeSkin(-1);
        }

        private void ChangeSkin(int idDelta)
        {
            _currentSkinId += idDelta;
            ClampSkinId();
            UpdateSkinInStorage();
        }

        private void ClampSkinId()
        {
            if (_currentSkinId > _skinsCountInStorage)
            {
                _currentSkinId = 1;
            } 
            else if (_currentSkinId == 0)
            {
                _currentSkinId = _skinsCountInStorage;
            }
        }

        private void SyncLocalPlayerSkinId()
        {
            _currentSkinId = _playersSkinsStorage.ReadOnlyLocalPlayerSkinId.Value;
        }

        private void UpdateSkinInStorage()
        {
            _playersSkinsStorageSetter.SetPlayerSkin(_networkActiveRunnerInfoProvider.LocalPlayerRef, (byte)_currentSkinId);
        }
    }
}