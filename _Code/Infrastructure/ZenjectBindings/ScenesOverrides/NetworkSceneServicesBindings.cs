using Core.Network.SceneHandlers;
using Sirenix.OdinInspector;
using UI;
using UI.BaseElements.Timers;
using UI.InGame.Input;
using UI.InGame.PlayersScoresPanel;
using UI.Screens;
using UnityEngine;

namespace Infrastructure.ZenjectBindings.ScenesOverrides
{
    public class NetworkSceneServicesBindings : SceneServicesBindings
    {
        [SerializeField, ReadOnly] 
        private NetworkSceneHandler _networkSceneHandler;

        private NetworkSceneUICanvas _gameUI;
        
        public sealed override void InstallBindings()
        {
            // UI bindings
            _gameUI = _sceneUI as NetworkSceneUICanvas;
            BindUICountdownTimer();
            BindInputPanelSwitcher();
            BindGameUIScreensSwitcher();
            BindPlayersScoresPanel();
            BindScoreResultsClosingCountdownTimer();

            // Additional bindings
            BindAdditionalDependencies();
            
            InjectDependenciesInNetworkSceneHandler();
            
            base.InstallBindings();
        }

        protected virtual void BindAdditionalDependencies() { }

        private void BindInputPanelSwitcher()
        {
            Container
                .Bind<IInputPanelSwitcher>()
                .To<InputPanelSwitcher>()
                .FromInstance(_gameUI.InputPanelSwitcher)
                .AsSingle();
        }

        private void BindUICountdownTimer()
        {
            Container
                .Bind<ICountdownTimer>()
                .To<CountdownTimer>()
                .FromInstance(_gameUI.CountdownTimer)
                .AsSingle();
        }

        private void BindGameUIScreensSwitcher()
        {
            Container
                .Bind<IInGameUIScreensSwitcher>()
                .To<InGameUIScreensSwitcher>()
                .FromInstance(_gameUI.GameScreensSwitcher)
                .AsSingle();
        }
        
        private void BindPlayersScoresPanel()
        {
            Container
                .Bind<IPlayersScoresPanelDeactivator>()
                .To<PlayersScoresPanel>()
                .FromInstance(_gameUI.PlayersScoresPanel)
                .AsSingle();
        }
        
        private void BindScoreResultsClosingCountdownTimer()
        {
            Container
                .Bind<IScoreResultsClosingCountdownTimer>()
                .To<ScoreResultsClosingCountdownTimer>()
                .FromInstance(_gameUI.ScoreResultsClosingCountdownTimer)
                .AsSingle();
        }

        private void InjectDependenciesInNetworkSceneHandler()
        {
            Container.InjectGameObject(_networkSceneHandler.gameObject);
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (_networkSceneHandler == null) _networkSceneHandler = FindObjectOfType<NetworkSceneHandler>();
        }
#endif
    }
}