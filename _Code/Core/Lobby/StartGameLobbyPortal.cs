using System.Linq;
using Core.Network;
using Data.UI;
using Fusion;
using Infrastructure.Network;
using UI.BaseElements.Timers;
using UnityEngine;
using Zenject;

namespace Core.Lobby
{
    public class StartGameLobbyPortal : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private StartGameLobbyPortalTrigger _trigger;

        private CountdownTimerTimesData _countdownTimerTimesData;
        private ICountdownTimer _uiCountdownTimer;
        private IRandomMiniGameLoader _randomMiniGameLoader;
        private INetworkActiveRunnerInfoProvider _networkActiveRunnerInfoProvider;

        [Inject]
        private void Construct(CountdownTimerTimesData countdownTimerTimesData, ICountdownTimer countdownTimer, 
            INetworkActiveRunnerInfoProvider networkActiveRunnerInfoProvider, IRandomMiniGameLoader randomMiniGameLoader)
        {
            _countdownTimerTimesData = countdownTimerTimesData;
            _uiCountdownTimer = countdownTimer;
            _randomMiniGameLoader = randomMiniGameLoader;
            _networkActiveRunnerInfoProvider = networkActiveRunnerInfoProvider;
        }
        
        private void Awake()
        {
            _trigger.OnPlayersCountInZoneUpdated += OnPlayersInZoneCountUpdated;
        }

        private void OnPlayersInZoneCountUpdated(int playersInZone)
        {
            int activePlayersInSession = _networkActiveRunnerInfoProvider.ActiveSessionPlayers.Count();
            
            if (_uiCountdownTimer.IsActivated && playersInZone != activePlayersInSession)
            {
                _uiCountdownTimer.BreakCountdown();
                RPC_BreakCountdownForClients();
            } 
            else if (playersInZone == activePlayersInSession)
            {
                _uiCountdownTimer.StartCountdown(_countdownTimerTimesData.TimeToStartNextMiniGameFromLobby, _randomMiniGameLoader.LoadRandomMiniGame);
                RPC_StartCountdownForClients();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StartCountdownForClients()
        {
            if (Object.HasStateAuthority) return;
            
            _uiCountdownTimer.StartCountdown(_countdownTimerTimesData.TimeToStartNextMiniGameFromLobby, null);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_BreakCountdownForClients()
        {
            if (Object.HasStateAuthority) return;
            
            _uiCountdownTimer.BreakCountdown();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_trigger == null) _trigger = GetComponentInChildren<StartGameLobbyPortalTrigger>();
        }
#endif
    }
}
