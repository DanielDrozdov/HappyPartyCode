using Core.MiniGames;
using Core.Network;
using Core.Storages;
using Sirenix.OdinInspector;
using UI;
using UI.BaseElements.Timers;
using UI.InGame.PlayersScoresPanel;
using UI.InGame.Timers;
using UnityEngine;

namespace Infrastructure.ZenjectBindings.ScenesOverrides
{
    public class MiniGameServicesBindings : NetworkSceneServicesBindings
    {
        [SerializeField, ReadOnly]
        private DeactivatedPlayersPlatform _deactivatedPlayersPlatform;

        [SerializeField, ReadOnly] 
        private MiniGameStarter _miniGameStarter;

        private MiniGameSceneUICanvas _miniGameSceneUICanvas;

        protected override void BindAdditionalDependencies()
        {
            BindDeactivatedPlayersPlatform();
            BindMiniGamePlayersStorage();
            BindMiniGameStarter();
            
            // UI bindings
            _miniGameSceneUICanvas = _sceneUI as MiniGameSceneUICanvas;
            BindMiniGameDurationCountdownTimer();
            BindPlayersScoresPanel();

            BindPlayersMiniGameDeactivator();
        }

        private void BindDeactivatedPlayersPlatform()
        {
            Container
                .Bind<IDeactivatedPlayersPlatform>()
                .To<DeactivatedPlayersPlatform>()
                .FromInstance(_deactivatedPlayersPlatform)
                .AsSingle();
        }

        private void BindMiniGamePlayersStorage()
        {
            Container
                .Bind<IMiniGamePlayersStorage>()
                .To<MiniGamePlayersStorage>()
                .AsSingle()
                .NonLazy();
        }

        private void BindMiniGameStarter()
        {
            Container
                .BindInterfacesTo<MiniGameStarter>()
                .FromInstance(_miniGameStarter)
                .AsSingle()
                .NonLazy();
        }

        private void BindMiniGameDurationCountdownTimer()
        {
            Container
                .Bind<IMiniGameDurationCountdownTimer>()
                .To<MiniGameDurationCountdownTimer>()
                .FromInstance(_miniGameSceneUICanvas.GameDurationCountdownTimer)
                .AsSingle();
        }
        
        private void BindPlayersScoresPanel()
        {
            Container
                .Bind<IPlayersMiniGameScoresPanel>()
                .To<PlayersMiniGameScoresPanel>()
                .FromInstance(_miniGameSceneUICanvas.MiniGamePlayersScoresPanel)
                .AsSingle();
        }

        private void BindPlayersMiniGameDeactivator()
        {
            Container
                .Bind<IPlayersMiniGameDeactivator>()
                .To<PlayersMiniGameDeactivator>()
                .AsSingle()
                .NonLazy();
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (_deactivatedPlayersPlatform == null) _deactivatedPlayersPlatform = FindObjectOfType<DeactivatedPlayersPlatform>();

            if (_miniGameStarter == null) _miniGameStarter = FindObjectOfType<MiniGameStarter>();
        }

#endif
    }
}
