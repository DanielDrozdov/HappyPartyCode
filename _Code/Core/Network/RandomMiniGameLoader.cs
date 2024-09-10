using System.Collections.Generic;
using Data.Levels;
using Fusion;
using Infrastructure.Network;
using UnityEngine;

namespace Core.Network
{
    public class RandomMiniGameLoader : IRandomMiniGameLoader
    {
        private NetworkRunner _currentRunner;
        private MiniGamesListData _miniGamesListData;
        private INetworkSceneLoader _sceneLoader;
        private INetworkConnectorCallbacksObserver _connectorCallbacksObserver;
        private List<LevelSettingsData> _availableMiniGamesList;
        private List<LevelSettingsData> _blockedMiniGamesList;
        private readonly int _maxBlockedScenesPoolSizeForLoad = 1;

        public RandomMiniGameLoader(MiniGamesListData miniGamesListData, INetworkSceneLoader sceneLoader, INetworkConnectorCallbacksObserver connectorCallbacksObserver)
        {
            _miniGamesListData = miniGamesListData;
            _sceneLoader = sceneLoader;
            _connectorCallbacksObserver = connectorCallbacksObserver;
            
            Initialize();
        }

        private void Initialize()
        {
            _availableMiniGamesList = new List<LevelSettingsData>(_miniGamesListData.MiniGames);
            _blockedMiniGamesList = new List<LevelSettingsData>();

            _connectorCallbacksObserver.OnSetNewRoomConnection += OnSetNewConnection;
        }
        
        public void LoadRandomMiniGame()
        {
            LevelSettingsData miniGameSettings = _availableMiniGamesList[Random.Range(0, _availableMiniGamesList.Count)];
            
            BlockMiniGameForLoading(miniGameSettings);

            if (_blockedMiniGamesList.Count - 1 >= _maxBlockedScenesPoolSizeForLoad)
            {
                UnblockMiniGameForLoading();
            }
            
            _sceneLoader.LoadScene(_currentRunner, miniGameSettings);
        }

        private void BlockMiniGameForLoading(LevelSettingsData miniGameSettings)
        {
            _availableMiniGamesList.Remove(miniGameSettings);
            _blockedMiniGamesList.Add(miniGameSettings);
        }

        private void UnblockMiniGameForLoading()
        {
            LevelSettingsData unblockedMiniGame = _blockedMiniGamesList[0];
            _availableMiniGamesList.Add(unblockedMiniGame);
            _blockedMiniGamesList.RemoveAt(0);
        }

        private void OnSetNewConnection(NetworkRunner runner)
        {
            _currentRunner = runner;
            _blockedMiniGamesList.Clear();
            _availableMiniGamesList = new(_miniGamesListData.MiniGames);
        }
    }
}
