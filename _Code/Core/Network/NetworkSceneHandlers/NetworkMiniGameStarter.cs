using Data.UI;
using Fusion;
using Infrastructure.Services.Input;
using UI.BaseElements.Timers;
using Zenject;

namespace Core.Network.SceneHandlers
{
    public class NetworkMiniGameStarter : NetworkBehaviour
    {
        private CountdownTimerTimesData _countdownTimerTimesData;
        private IApplicationInputBlocker _applicationInputBlocker;
        private IMiniGameStarter _miniGameStarter;
        private ICountdownTimer _countdownTimer;

        [Inject]
        private void Construct(CountdownTimerTimesData countdownTimerTimesData, IApplicationInputBlocker applicationInputBlocker, ICountdownTimer countdownTimer,
            IMiniGameStarter miniGameStarter)
        {
            _countdownTimerTimesData = countdownTimerTimesData;
            _applicationInputBlocker = applicationInputBlocker;
            _countdownTimer = countdownTimer;
            _miniGameStarter = miniGameStarter;
        }

        public override void Spawned()
        {
            _applicationInputBlocker.BlockUserInput();
        }

        public void StartMiniGame()
        {
            RPC_StartMiniGame();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StartMiniGame()
        {
            _countdownTimer.StartCountdown(_countdownTimerTimesData.TimeToStartMiniGame, OnCountdownEnded);
        }

        private void OnCountdownEnded()
        {
            if (Runner.IsServer)
            {
                _miniGameStarter.StartMiniGame();
            }

            _applicationInputBlocker.UnblockUserInput();
        }
    }
}
