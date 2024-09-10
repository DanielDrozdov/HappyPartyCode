using Core.Storages;
using Infrastructure.Network;
using Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Core.Network.SceneHandlers.Lobby
{
    public class LobbyLoadInitializer : MonoBehaviour
    {
        private INetworkActiveRunnerInfoProvider _networkActiveRunnerInfoProvider;
        private IPlayersScoresStorageInitializer _playersScoresStorageInitializer;
        private IApplicationInputBlocker _applicationInputBlocker;

        [Inject]
        private void Construct(INetworkActiveRunnerInfoProvider networkActiveRunnerInfoProvider, IPlayersScoresStorageInitializer playersScoresStorageInitializer,
            IApplicationInputBlocker applicationInputBlocker)
        {
            _networkActiveRunnerInfoProvider = networkActiveRunnerInfoProvider;
            _playersScoresStorageInitializer = playersScoresStorageInitializer;
            _applicationInputBlocker = applicationInputBlocker;
        }

        private void Awake()
        {
            CheckIfNeedInitializePlayersScoresStorages();
            _applicationInputBlocker.UnblockUserInput();
        }

        private void CheckIfNeedInitializePlayersScoresStorages()
        {
            if (_playersScoresStorageInitializer.IsInitialized) return;
            
            _playersScoresStorageInitializer.Initialize(_networkActiveRunnerInfoProvider.ActiveSessionPlayers);
        }
    }
}
