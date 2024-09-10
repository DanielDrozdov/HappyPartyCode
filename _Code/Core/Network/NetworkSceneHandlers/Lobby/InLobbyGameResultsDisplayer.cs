using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Storages;
using Data.UI;
using Fusion;
using UI.BaseElements.Timers;
using UnityEngine;
using Utility;
using Zenject;

namespace Core.Network.SceneHandlers.Lobby
{
    public class InLobbyGameResultsDisplayer : BaseGameResultsDisplayer
    {
        [SerializeField] 
        private int _scoreToWin;

        [SerializeField, ReadOnly] 
        private PlayersLevelLoadingStateChecker _playersLevelLoadingStateChecker;
        
        private CountdownTimerTimesData _countdownTimerTimesData;
        private IPlayersScoresStorage _playersScoresStorage;
        private IScoreResultsClosingCountdownTimer _scoreResultsClosingCountdownTimer;

        [Inject]
        private void Construct(CountdownTimerTimesData countdownTimerTimesData, IPlayersScoresStorage playersScoresStorage,
            IScoreResultsClosingCountdownTimer scoreResultsClosingCountdownTimer)
        {
            _countdownTimerTimesData = countdownTimerTimesData;
            _playersScoresStorage = playersScoresStorage;
            _scoreResultsClosingCountdownTimer = scoreResultsClosingCountdownTimer;
        }

        private void Awake()
        {
            _playersLevelLoadingStateChecker.OnAllPlayersLoaded += ShowTotalResultForPlayersIfServer;
        }

        protected override ReadOnlyDictionary<PlayerRef, int> GetPlayersScores() => _playersScoresStorage.GetPlayersTotalScores();

        protected override void OnScorePodiumLookTransferCompleted()
        {
            base.OnScorePodiumLookTransferCompleted();
            StartReturnToMainMenuCountdown();
        }

        private void StartReturnToMainMenuCountdown()
        {
            _scoreResultsClosingCountdownTimer.StartCountdown(_countdownTimerTimesData.TimeToReturnMainMenuAfterTotalResultsDisplayed, 
                () => Runner.Shutdown());
        }

        private void ShowTotalResultForPlayersIfServer()
        {
            if (Runner.IsServer && IfSomeoneReachedWinScore())
            {
                StartCoroutine(DisplayResultsWithDelay());
            }
        }

        private bool IfSomeoneReachedWinScore()
        {
            foreach (KeyValuePair<PlayerRef,int> playersScore in GetPlayersScores())
            {
                if (playersScore.Value >= _scoreToWin)
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator DisplayResultsWithDelay()
        {
            yield return WaitForConstants.QuarterOfSecond;
            RPC_DisplayResults();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_DisplayResults()
        {
            DisplayResults();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_playersLevelLoadingStateChecker == null) transform.parent.TryGetComponent(out _playersLevelLoadingStateChecker);
        }
#endif
    }
}
