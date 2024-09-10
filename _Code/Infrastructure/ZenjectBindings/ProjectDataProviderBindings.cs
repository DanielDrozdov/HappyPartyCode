using Data.Levels;
using Data.Network;
using Data.UI;
using Infrastructure.Network;
using UnityEngine;
using Zenject;

namespace Infrastructure.ZenjectBindings
{
    public class ProjectDataProviderBindings : MonoInstaller
    {
        [SerializeField] 
        private NetworkRoomSettings _networkRoomSettings;

        [SerializeField] 
        private LobbySettingsData _lobbySettingsData;

        [SerializeField] 
        private CountdownTimerTimesData _countdownTimerTimesData;

        [SerializeField] 
        private MiniGamesListData _miniGamesListData;
        
        public override void InstallBindings()
        {
            BindNetworkData();
            BindNetworkLevelSettingsProvider();
            BindLobbySettingsData();
            BindCountdownTimerTimesData();
            BindMiniGamesListData();
        }

        private void BindNetworkData()
        {
            Container
                .Bind<NetworkRoomSettings>()
                .FromInstance(_networkRoomSettings)
                .AsSingle()
                .NonLazy();
        }

        private void BindNetworkLevelSettingsProvider()
        {
            Container
                .BindInterfacesTo<NetworkSceneSettingsProvider>()
                .AsSingle()
                .NonLazy();
        }

        private void BindLobbySettingsData()
        {
            Container
                .Bind<LobbySettingsData>()
                .FromInstance(_lobbySettingsData)
                .AsSingle()
                .NonLazy();
        }

        private void BindCountdownTimerTimesData()
        {
            Container
                .Bind<CountdownTimerTimesData>()
                .FromInstance(_countdownTimerTimesData)
                .AsSingle()
                .NonLazy();
        }

        private void BindMiniGamesListData()
        {
            Container
                .Bind<MiniGamesListData>()
                .FromInstance(_miniGamesListData)
                .AsSingle()
                .NonLazy();
        }
    }
}
