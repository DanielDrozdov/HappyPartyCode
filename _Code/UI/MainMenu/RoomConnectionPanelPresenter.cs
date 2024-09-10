using System;
using System.Threading.Tasks;
using Data.Save;
using Fusion;
using Infrastructure.Network;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Room
{
    public class RoomConnectionPanelPresenter : MonoBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private RoomConnectionPanelView _view;
        
        private INetworkConnector _networkConnector;
        private IGameDataAccess _gameDataAccess;

        private void Awake()
        {
            _view.UpdateNickname(_gameDataAccess.GameSaveData.Nickname);
        }

        [Inject]
        private void Construct(INetworkConnector networkConnector, IGameDataAccess gameDataAccess)
        {
            _networkConnector = networkConnector;
            _gameDataAccess = gameDataAccess;
        }

        public void CreateRoom(string roomName)
        {
            if (!_networkConnector.CanCreateNewConnection()) return;
            
            DoNetworkTask(roomName, _networkConnector.CreateRoom);
        }

        public void ConnectToRoom(string roomName)
        {
            if (!_networkConnector.CanCreateNewConnection()) return;
            
            DoNetworkTask(roomName, _networkConnector.ConnectToRoom);
        }

        public void SaveNickname(string nickname)
        {
            _gameDataAccess.GameSaveData.Nickname = nickname;
        }

        private async void DoNetworkTask(string roomName, Func<string, Task<StartGameResult>> task)
        {
            StartGameResult gameResult = await task(roomName);
            ProcessConnectionResult(gameResult);
        }

        private void ProcessConnectionResult(StartGameResult startGameResult)
        {
            if (!startGameResult.Ok)
            {
                HandleErrors(startGameResult.ErrorMessage);
            }
        }
        
        private void HandleErrors(string errorMessage)
        {
            _view.DisplayConnectionError(FilterFusionErrorText(errorMessage));
        }
        
        private string FilterFusionErrorText(string errorMessage)
        {
            string filteredError;
            
            if (errorMessage.Contains("ErrorCode"))
            {
                int indexOfStartErrorNumberMessage = errorMessage.IndexOf('(');
                filteredError = errorMessage.Substring(0, indexOfStartErrorNumberMessage - 1);
            }
            else
            {
                filteredError = errorMessage.SplitPascalCase();
            }

            return filteredError;
        }

        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_view == null) TryGetComponent(out _view);
        }
#endif
    }
}
