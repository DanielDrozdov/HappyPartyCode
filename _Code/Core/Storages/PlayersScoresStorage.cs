using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion;
using Infrastructure.Network;
using UnityEngine;

namespace Core.Storages
{
    public class PlayersScoresStorage : IPlayersScoresStorage, IPlayersScoresStorageInitializer, IPlayersMiniGameScoresStorage
    {
        private Dictionary<PlayerRef, int> _playersTotalScores = new();
        private Dictionary<PlayerRef, int> _miniGameScores = new();
        private Dictionary<byte, PlayerRef> _playerRefsById = new();
        private readonly INetworkConnectorCallbacksObserver _networkConnectorCallbacksObserver;
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public PlayersScoresStorage(INetworkConnectorCallbacksObserver networkConnectorCallbacksObserver)
        {
            _networkConnectorCallbacksObserver = networkConnectorCallbacksObserver;
            _networkConnectorCallbacksObserver.OnSetNewRoomConnection += UpdateSubscribes;
        }

        public void TransferMiniGameScoresToTotalBalance()
        {
            List<PlayerRef> players = _playersTotalScores.Keys.ToList();
            
            foreach (PlayerRef playerRef in players)
            {
                _playersTotalScores[playerRef] += _miniGameScores[playerRef];
                _miniGameScores[playerRef] = 0;
            }
        }

        public void AddMiniGameScoreToPlayer(byte playerId, int score)
        {
            PlayerRef playerRef = _playerRefsById[playerId];
            _miniGameScores[playerRef] += score;
        }

        public int GetPlayerTotalScore(PlayerRef playerRef)
        {
            return _playersTotalScores.GetValueOrDefault(playerRef);
        }

        public int GetPlayerMiniGameScore(PlayerRef playerRef)
        {
            return _miniGameScores.GetValueOrDefault(playerRef);
        }

        public ReadOnlyDictionary<PlayerRef, int> GetPlayersMiniGameScores()
        {
            return new ReadOnlyDictionary<PlayerRef, int>(_miniGameScores);
        }
        
        public ReadOnlyDictionary<PlayerRef, int> GetPlayersTotalScores()
        {
            return new ReadOnlyDictionary<PlayerRef, int>(_playersTotalScores);
        }

        public void Initialize(IEnumerable<PlayerRef> playersRefs)
        {
            foreach (PlayerRef playerRef in playersRefs)
            {
                _playersTotalScores.Add(playerRef, 0);
                _miniGameScores.Add(playerRef, 0);
                _playerRefsById.Add((byte)playerRef.PlayerId, playerRef);
            }

            _isInitialized = true;
        }

        private void ClearScoresData()
        {
            _isInitialized = false;
            _playersTotalScores.Clear();
            _miniGameScores.Clear();
            _playerRefsById.Clear();
        }

        #region Network Callbacks
        
        private void OnPlayerLeft(PlayerRef playerRef)
        {
            _playersTotalScores.Remove(playerRef);
            _miniGameScores.Remove(playerRef);
        }

        private void UpdateSubscribes(NetworkRunner networkRunner)
        {
            ClearScoresData();
            _networkConnectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerLeft += OnPlayerLeft;
        }
        
        #endregion
    }
}
