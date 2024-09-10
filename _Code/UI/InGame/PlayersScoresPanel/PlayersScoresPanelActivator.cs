using System;
using System.Collections.Generic;
using Core.Storages;
using Fusion;
using Infrastructure.Network;
using Infrastructure.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace UI.InGame.PlayersScoresPanel
{
    public class PlayersScoresPanelActivator : MonoBehaviour
    {
        [SerializeField, Fusion.ReadOnly] 
        private PlayerScorePanelPresenter[] _playersScores;

        private Dictionary<PlayerRef, PlayerScorePanelPresenter> _playersPanelsDict;
        
        private IPlayersScoresStorage _playersScoresStorage;
        private IPlayersNicknamesStorage _playersNicknamesStorage;
        private INetworkActiveRunnerInfoProvider _activeRunnerInfoProvider;
        private INetworkCallbacksEvents _networkCallbacksEvents;
        private IActiveSceneComparer _activeSceneComparer;

        [Inject]
        private void Construct(IPlayersScoresStorage playersScoresStorage, IPlayersMiniGameScoresStorage miniGameScoresStorage, 
            IPlayersNicknamesStorage playersNicknamesStorage, INetworkActiveRunnerInfoProvider activeRunnerInfoProvider,
            INetworkConnectorCallbacksObserver networkConnectorCallbacksObserver, IActiveSceneComparer activeSceneComparer)
        {
            _playersScoresStorage = playersScoresStorage;
            _playersNicknamesStorage = playersNicknamesStorage;
            _activeRunnerInfoProvider = activeRunnerInfoProvider;
            _activeSceneComparer = activeSceneComparer;
            _networkCallbacksEvents = networkConnectorCallbacksObserver.NetworkCallbacksEvents;
        }
        
        private void Start()
        {
            _networkCallbacksEvents.OnNetworkPlayerLeft += DeactivatePlayerScoreMiniPanel;
        }

        private void OnDestroy()
        {
            if (_networkCallbacksEvents == null) return;
            
            _networkCallbacksEvents.OnNetworkPlayerLeft -= DeactivatePlayerScoreMiniPanel;
        }
        
        public void InitializeScoresPanel(Dictionary<PlayerRef, PlayerScorePanelPresenter> playersPanelsDict)
        {
            _playersPanelsDict = playersPanelsDict;
            
            if (_activeSceneComparer.IsCurrentSceneLobby())
            {
                ActivatePlayersTotalScores();
            }
            else
            {
                ActivatePlayersMiniGameScores();
            }
        }
        
        private void ActivatePlayersTotalScores() => ActivatePlayersScores(_playersScoresStorage.GetPlayerTotalScore);

        private void ActivatePlayersMiniGameScores() => ActivatePlayersScores();

        private void ActivatePlayersScores(Func<PlayerRef, int> scoreReturnerStorageMethod = null)
        {
            IEnumerable<PlayerRef> activePlayersInSession = _activeRunnerInfoProvider.ActiveSessionPlayers;

            int playersPanelsIndex = 0;
            
            foreach (PlayerRef playerRef in activePlayersInSession)
            {
                int playerScore = scoreReturnerStorageMethod != null ? scoreReturnerStorageMethod(playerRef) : 0;
                ActivatePlayerScoreMiniPanel(playerRef, playerScore, playersPanelsIndex);
                playersPanelsIndex++;
            }
        }

        private void ActivatePlayerScoreMiniPanel(PlayerRef playerRef, int score, int activatedIndex)
        {
            PlayerScorePanelPresenter panel = _playersScores[activatedIndex];
            string playerNickname = _playersNicknamesStorage.GetPlayerNickname(playerRef);
            panel.Activate(playerNickname, score);
            _playersPanelsDict.Add(playerRef, panel);
        }

        private void DeactivatePlayerScoreMiniPanel(PlayerRef playerRef)
        {
            if (!_playersPanelsDict.ContainsKey(playerRef)) return;
            
            _playersPanelsDict[playerRef].Deactivate();
            _playersPanelsDict.Remove(playerRef);
        }
        
#if UNITY_EDITOR

        [Button("Update Scores Panels List")]
        private void UpdatePlayersScoresPanelsList()
        {
            _playersScores = GetComponentsInChildren<PlayerScorePanelPresenter>();
        }
        
        private void OnValidate()
        {
            if (_playersScores == null) _playersScores = GetComponentsInChildren<PlayerScorePanelPresenter>();
        }
#endif
    }
}