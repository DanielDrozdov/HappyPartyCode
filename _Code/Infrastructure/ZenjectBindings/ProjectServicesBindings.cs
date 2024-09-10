using Core.Network;
using Core.Storages;
using Data.Character;
using Data.Save;
using Infrastructure.Network;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Pooling;
using Sirenix.OdinInspector;
using UI.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;
using Utility.Assets;
using Utility.Factories;
using Zenject;

namespace Infrastructure.ZenjectBindings
{
    public class ProjectServicesBindings : MonoInstaller
    {
        [SerializeField] 
        private GameObject _systemUI;

        [SerializeField] 
        private CharacterModelsList _characterModelsList;
        
        [SerializeField] 
        private AssetReference _networkRunnerRef;

        [SerializeField] 
        private AssetReference _mainMenuSceneRef;
        
        public override void InstallBindings()
        {
            // Infrastructure Services
            BindApplicationInputProvider();
            BindDependenciesInjector();
            BindActiveSceneComparer();
            BindGameDataSaver();
            BindObjectsFactory();
            BindAssetsLoader();
            BindObjectsPool();
            
            // Network
            BindNetworkSceneLoader();
            BindNetworkConnector();
            BindNetworkRunnerInfoProvider();
            
            // Core 
            BindPlayersScoresStorage();
            BindPlayersNicknamesStorage();
            BindPlayersSkinsStorage();
            BindGameLevelsRandomizer();
            
            // UI
            CreateAndBindSystemUIDependencies();
        }

        #region Infrastructure Services

        private void BindApplicationInputProvider()
        {
            Container
                .BindInterfacesTo<ApplicationInputProvider>()
                .AsSingle()
                .NonLazy();
        }

        private void BindDependenciesInjector()
        {
            Container
                .Bind<IDependenciesInjector>()
                .To<DependenciesInjector>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindActiveSceneComparer()
        {
            Container
                .Bind<IActiveSceneComparer>()
                .To<ActiveSceneComparer>()
                .AsSingle()
                .WithArguments(_mainMenuSceneRef)
                .NonLazy();
        }

        private void BindGameDataSaver()
        {
            Container
                .Bind<IGameDataAccess>()
                .To<GameDataSaver>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAssetsLoader()
        {
            Container
                .Bind<IAssetsLoader>()
                .To<AssetsLoader>()
                .AsSingle()
                .NonLazy();
        }

        private void BindObjectsFactory()
        {
            Container
                .Bind<IObjectsFactory>()
                .To<ObjectsFactory>()
                .AsSingle()
                .NonLazy();
        }

        private void BindObjectsPool()
        {
            Container
                .Bind<IGenericPrefabsPool>()
                .To<GenericPrefabsPool>()
                .AsSingle()
                .NonLazy();
        }

        #endregion

        #region Network

        private void BindNetworkSceneLoader()
        {
            Container
                .Bind<INetworkSceneLoader>()
                .To<NetworkSceneLoader>()
                .AsSingle()
                .NonLazy();
        }

        private void BindNetworkConnector()
        {
            Container
                .BindInterfacesTo<NetworkConnector>()
                .AsSingle()
                .WithArguments(_networkRunnerRef)
                .NonLazy();
        }
        
        private void BindNetworkRunnerInfoProvider()
        {
            Container
                .BindInterfacesTo<NetworkActiveRunnerInfoProvider>()
                .AsSingle()
                .NonLazy();
        }

        #endregion

        #region Core

        private void BindPlayersScoresStorage()
        {
            Container
                .BindInterfacesTo<PlayersScoresStorage>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindPlayersNicknamesStorage()
        {
            Container
                .BindInterfacesTo<PlayersNicknamesStorage>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindPlayersSkinsStorage()
        {
            Container
                .BindInterfacesTo<PlayersSkinsStorage>()
                .AsSingle()
                .WithArguments(_characterModelsList)
                .NonLazy();
        }
        
        private void BindGameLevelsRandomizer()
        {
            Container
                .Bind<IRandomMiniGameLoader>()
                .To<RandomMiniGameLoader>()
                .AsSingle()
                .NonLazy();
        }

        #endregion

        #region UI
        
        private void CreateAndBindSystemUIDependencies()
        {
            GameObject systemUI = Container.InstantiatePrefab(_systemUI);

            SceneTransitionPanel sceneTransitionPanel = systemUI.GetComponentInChildren<SceneTransitionPanel>();

            Container
                .Bind<ISceneTransition>()
                .To<SceneTransitionPanel>()
                .FromInstance(sceneTransitionPanel)
                .AsSingle()
                .NonLazy();
        }
        
        #endregion
    }
}
