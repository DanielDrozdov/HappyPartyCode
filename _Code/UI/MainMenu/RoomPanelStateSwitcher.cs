using Fusion;
using Infrastructure.Network;
using UI.MainMenu.SkinChanger;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Room
{
    public class RoomPanelStateSwitcher : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _connectionPanel;
        
        [SerializeField] 
        private GameObject _infoPanel;

        [SerializeField, ReadOnly] 
        private RoomInfoPanelPresenter _infoPanelPresenter;

        [SerializeField, ReadOnly] 
        private SkinChangerPanelPresenter _skinChangerPanel;
        
        private INetworkConnectorCallbacksObserver _networkConnectorCallbacksObserver;

        [Inject]
        private void Construct(INetworkConnectorCallbacksObserver networkConnectorCallbacksObserver)
        {
            _networkConnectorCallbacksObserver = networkConnectorCallbacksObserver;
        }

        private void Awake()
        {
            _networkConnectorCallbacksObserver.OnSetNewRoomConnection += SubscribeOnEvents;
            _networkConnectorCallbacksObserver.OnSetNewRoomConnection += SwitchOnInfoPanel;
        }

        private void OnDestroy()
        {
            _networkConnectorCallbacksObserver.OnSetNewRoomConnection -= SubscribeOnEvents;
            _networkConnectorCallbacksObserver.OnSetNewRoomConnection -= SwitchOnInfoPanel;
            
            if (_networkConnectorCallbacksObserver.NetworkCallbacksEvents != null)
            {
                _networkConnectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkShutdown -= SwitchOnConnectionPanel;
            }
        }

        private void SubscribeOnEvents(NetworkRunner runner)
        {
            _networkConnectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkShutdown += SwitchOnConnectionPanel;
        }

        private void SwitchOnConnectionPanel()
        {
            _connectionPanel.SetActive(true);
            _skinChangerPanel.Switch(false);
            _infoPanel.SetActive(false);
        }
        
        private void SwitchOnInfoPanel(NetworkRunner runner)
        {
            _infoPanelPresenter.SyncNetworkRunnerVariables(runner);
            _connectionPanel.SetActive(false);
            _skinChangerPanel.Switch(true);
            _infoPanel.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_infoPanelPresenter == null) _infoPanelPresenter = GetComponentInChildren<RoomInfoPanelPresenter>();

            if (_skinChangerPanel == null) _skinChangerPanel = GetComponentInChildren<SkinChangerPanelPresenter>();
        }
#endif
    }
}
