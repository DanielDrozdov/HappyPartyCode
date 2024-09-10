using System;
using System.Collections.Generic;
using System.Linq;
using Data.Network;
using Fusion;
using Fusion.Sockets;
using Infrastructure.Network.CallbacksServices.Spawners;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Network
{
    public class NetworkCallbacks : MonoBehaviour, INetworkRunnerCallbacks, INetworkCallbacksEvents
    {
        [SerializeField, ReadOnly] 
        private NetworkRoomPodiumPlayerSpawner _roomPodiumPlayerSpawner;

        private IApplicationInputProvider _applicationInputProvider;
        private IActiveSceneComparer _activeSceneComparer;
        private NetworkRoomSettings _networkRoomSettings;

        public event Action OnNetworkShutdown;
        public event Action<PlayerRef> OnNetworkPlayerJoined;
        public event Action<PlayerRef> OnNetworkPlayerLeft;

        [Inject]
        private void Construct(IApplicationInputProvider applicationInputProvider, IActiveSceneComparer activeSceneComparer, NetworkRoomSettings networkRoomSettings)
        {
            _applicationInputProvider = applicationInputProvider;
            _activeSceneComparer = activeSceneComparer;
            _networkRoomSettings = networkRoomSettings;
        }

        #region Callbacks
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer && _activeSceneComparer.IsCurrentSceneMainMenu())
            {
                _roomPodiumPlayerSpawner.SpawnPlayer(runner, player);
            }
            
            OnNetworkPlayerJoined?.Invoke(player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer && _activeSceneComparer.IsCurrentSceneMainMenu())
            {
                _roomPodiumPlayerSpawner.DespawnPlayer(runner, player);
            }

            if (runner.IsServer && runner.ActivePlayers.Count() == 1 && !_activeSceneComparer.IsCurrentSceneMainMenu())
            {
                runner.Shutdown();
            }
            
            OnNetworkPlayerLeft?.Invoke(player);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            PlayerNetworkInput playerNetworkInput = _applicationInputProvider.GetPlayerInputForNetwork();
            input.Set(playerNetworkInput);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            OnNetworkShutdown?.Invoke();

            if (!_activeSceneComparer.IsCurrentSceneMainMenu())
            {
                Addressables.LoadSceneAsync(_networkRoomSettings.MainMenuScenePath);
            }
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            runner.Shutdown();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }
        
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
        
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_roomPodiumPlayerSpawner == null) _roomPodiumPlayerSpawner = GetComponentInChildren<NetworkRoomPodiumPlayerSpawner>();
        }
#endif
    }
}
