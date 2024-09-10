using System;
using System.Collections.ObjectModel;
using Core.Storages;
using Data.Levels;
using Data.UI;
using Fusion;
using Infrastructure.Network;
using UI.BaseElements.Timers;
using Zenject;

namespace Core.Network.SceneHandlers
{
    public class MiniGameResultsDisplayer : BaseGameResultsDisplayer
    {
        private CountdownTimerTimesData _countdownTimerTimesData;
        private LobbySettingsData _lobbySettingsData;
        private IMiniGameCallbacks _miniGameCallbacks;
        private INetworkSceneLoader _networkSceneLoader;
        private IPlayersMiniGameScoresStorage _playersMiniGameScoresStorage;
        private IScoreResultsClosingCountdownTimer _scoreResultsClosingCountdownTimer;


        [Inject]
        private void Construct(CountdownTimerTimesData countdownTimerTimesData, LobbySettingsData lobbySettingsData, IMiniGameCallbacks miniGameCallbacks,
            INetworkSceneLoader networkSceneLoader, IPlayersMiniGameScoresStorage playersMiniGameScoresStorage, IScoreResultsClosingCountdownTimer scoreResultsClosingCountdownTimer)
        {
            _countdownTimerTimesData = countdownTimerTimesData;
            _lobbySettingsData = lobbySettingsData;
            _miniGameCallbacks = miniGameCallbacks;
            _networkSceneLoader = networkSceneLoader;
            _playersMiniGameScoresStorage = playersMiniGameScoresStorage;
            _scoreResultsClosingCountdownTimer = scoreResultsClosingCountdownTimer;
        }

        public override void Spawned()
        {
            _miniGameCallbacks.OnMiniGameEnded += DisplayResults;
        }

        protected override ReadOnlyDictionary<PlayerRef, int> GetPlayersScores() => _playersMiniGameScoresStorage.GetPlayersMiniGameScores();

        protected override void SetUpPodiumAndPlayers()
        {
            base.SetUpPodiumAndPlayers();
            _playersMiniGameScoresStorage.TransferMiniGameScoresToTotalBalance();
        }

        protected override void OnScorePodiumLookTransferCompleted()
        {
            base.OnScorePodiumLookTransferCompleted();
            StartReturnToLobbyCountdown();
        }

        private void StartReturnToLobbyCountdown()
        {
            Action onCompleteCountdownAction = Runner.IsServer ? LoadLobbyScene : null;

            _scoreResultsClosingCountdownTimer.StartCountdown(_countdownTimerTimesData.TimeToReturnLobbyAfterMiniGameResultsDisplayed, onCompleteCountdownAction);

            void LoadLobbyScene()
            {
                _networkSceneLoader.LoadScene(Runner, _lobbySettingsData);
            }
        }
    }
}
