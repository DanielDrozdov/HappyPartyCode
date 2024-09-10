using System;
using System.Threading.Tasks;
using Data.Network;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;
using Utility.Factories;

namespace Infrastructure.Network
{
    public class NetworkConnector : INetworkConnector, INetworkConnectorCallbacksObserver, INetworkDisconnecter
    {
        private readonly IObjectsFactory _objectsFactory;
        private readonly IDependenciesInjector _dependenciesInjector;
        private readonly INetworkActiveRunnerInfoProviderNewRunnerSetter _activeRunnerInfoProviderNewRunnerSetter;
        private readonly AssetReference _networkRunnerRef;
        private readonly NetworkRoomSettings _networkRoomSettings;

        private NetworkRunner _curNetworkRunner;

        public INetworkCallbacksEvents NetworkCallbacksEvents { get; private set; }
        public event Action<NetworkRunner> OnSetNewRoomConnection;

        public NetworkConnector(IObjectsFactory objectsFactory, IDependenciesInjector dependenciesInjector,
            INetworkActiveRunnerInfoProviderNewRunnerSetter activeRunnerInfoProviderNewRunnerSetter, AssetReference networkRunnerRef,
            NetworkRoomSettings networkRoomSettings)
        {
            _objectsFactory = objectsFactory;
            _dependenciesInjector = dependenciesInjector;
            _activeRunnerInfoProviderNewRunnerSetter = activeRunnerInfoProviderNewRunnerSetter;
            _networkRunnerRef = networkRunnerRef;
            _networkRoomSettings = networkRoomSettings;
        }

        public bool CanCreateNewConnection() => _curNetworkRunner == null;

        public async Task<StartGameResult> ConnectToRoom(string roomName = null)
        {
            return await CreateConnection(GameMode.Client, roomName);
        }
        
        public async Task<StartGameResult> CreateRoom(string roomName)
        {
            return await CreateConnection(GameMode.Host, roomName);
        }

        public void ShutdownConnection()
        {
            if (_curNetworkRunner == null) return;

            _curNetworkRunner.Shutdown();
        } 
        
        private async Task<StartGameResult> CreateConnection(GameMode gameMode, string roomName = null)
        {
            await CreateNetworkRunner();
            INetworkSceneManager networkSceneManager = _curNetworkRunner.GetComponent<INetworkSceneManager>();
            NetworkObjectProvider objectProvider = _curNetworkRunner.GetComponent<NetworkObjectProvider>();
            objectProvider.SetDependencies(_dependenciesInjector);
            
            StartGameResult startGameResult  = await _curNetworkRunner.StartGame(new StartGameArgs()
            {
                GameMode = gameMode,
                SessionName = roomName,
                ObjectProvider = objectProvider,
                SceneManager = networkSceneManager,
                PlayerCount = _networkRoomSettings.MaxPlayers,
                OnGameStarted = (runner) => OnSetNewRoomConnection?.Invoke(runner)
            });

            SetNewRunnerInDataIfResultIsOk(startGameResult.Ok);
            
            return startGameResult;
        }

        private async Task CreateNetworkRunner()
        {
            GameObject runner = await _objectsFactory.Create(_networkRunnerRef);
            _curNetworkRunner = runner.GetComponent<NetworkRunner>();
            NetworkCallbacksEvents = _curNetworkRunner.GetComponent<INetworkCallbacksEvents>();
        }

        private void SetNewRunnerInDataIfResultIsOk(bool isConnectionResultOk)
        {
            if (isConnectionResultOk)
            {
                _activeRunnerInfoProviderNewRunnerSetter.SetNewRunner(_curNetworkRunner);
            }
        }
    }
}
