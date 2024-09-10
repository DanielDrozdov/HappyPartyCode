using System.Linq;
using Core.Storages;
using Data.Levels;
using Data.Network;
using Data.Save;
using Fusion;
using Infrastructure.Network;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Room
{
    public class RoomInfoPanelPresenter : MonoBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private RoomInfoPanelView _view;

        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private RoomUIErrorDisplay _errorDisplay;

        private NetworkRunner _networkRunner;
        private LobbySettingsData _lobbySettingsData;
        private INetworkConnectorCallbacksObserver _connectorCallbacksObserver;
        private INetworkSceneLoader _networkSceneLoader;
        private IGameDataAccess _gameDataAccess;

        [Inject]
        private void Construct(IGameDataAccess gameDataAccess, INetworkConnectorCallbacksObserver connectorCallbacksObserver,
            INetworkSceneLoader sceneLoader, LobbySettingsData lobbySettingsData)
        {
            _lobbySettingsData = lobbySettingsData;
            _connectorCallbacksObserver = connectorCallbacksObserver;
            _networkSceneLoader = sceneLoader;
            _gameDataAccess = gameDataAccess;
        }

        private void OnDestroy()
        {
            UnsubscribeOnLeftAndJoinedPlayerEvents();
        }

        public void SyncNetworkRunnerVariables(NetworkRunner runner)
        {
            _networkRunner = runner;
            _view.SetNewRoomSessionSettings(runner.SessionInfo.Name, _gameDataAccess.GameSaveData.Nickname, runner.IsServer);
            SubscribeOnLeftAndJoinedPlayerEvents();
            UpdatePlayersInSession();
        }

        public void StartGame()
        {
            if (_networkRunner.SessionInfo.PlayerCount == 1)
            {
                _errorDisplay.Display("Need more than one player");
                return;
            }
            
            _networkRunner.SessionInfo.IsOpen = false;
            _networkSceneLoader.LoadScene(_networkRunner, _lobbySettingsData);
        }

        public void LeaveRoom()
        {
            _networkRunner.Shutdown();
        }

        private void UpdatePlayersInSession()
        {
            _view.UpdatePlayersCountInRoom(_networkRunner.ActivePlayers.Count(), _networkRunner.SessionInfo.MaxPlayers);
        }

        private void OnNewPlayerJoinedOrLeft(PlayerRef playerRef)
        {
            UpdatePlayersInSession();
        }

        private void SubscribeOnLeftAndJoinedPlayerEvents()
        {
            _connectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerJoined += OnNewPlayerJoinedOrLeft;
            _connectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerLeft += OnNewPlayerJoinedOrLeft;
        }

        private void UnsubscribeOnLeftAndJoinedPlayerEvents()
        {
            if (_connectorCallbacksObserver.NetworkCallbacksEvents != null)
            {
                _connectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerJoined -= OnNewPlayerJoinedOrLeft;
                _connectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerLeft -= OnNewPlayerJoinedOrLeft;
            }
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_view == null) TryGetComponent(out _view);

            if (_errorDisplay == null) _errorDisplay = GetComponentInChildren<RoomUIErrorDisplay>();
        }
#endif
    }
}