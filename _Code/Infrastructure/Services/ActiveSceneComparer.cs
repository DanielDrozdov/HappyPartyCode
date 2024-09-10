
using System.Collections.Generic;
using System.Linq;
using Data.Levels;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services
{
    public class ActiveSceneComparer : IActiveSceneComparer
    {
        private readonly AssetReference _lobbySceneRef;
        private readonly AssetReference _mainMenuSceneRef;

        private string _lobbySceneName;
        private string _mainMenuSceneName;
        
        public ActiveSceneComparer(LobbySettingsData lobbySettingsData, AssetReference mainMenuSceneRef)
        {
            _mainMenuSceneRef = mainMenuSceneRef;
            _lobbySceneRef = lobbySettingsData.SceneRef;

            SetBaseScenesNames();
        }
        
        public bool IsCurrentSceneLobby()
        {
            return SceneManager.GetActiveScene().name == _lobbySceneName;
        }
        
        public bool IsCurrentSceneMainMenu()
        {
            return SceneManager.GetActiveScene().name == _mainMenuSceneName;
        }

        private void SetBaseScenesNames()
        {
            _lobbySceneName = GetSceneNameByAssetReference(_lobbySceneRef);
            _mainMenuSceneName = GetSceneNameByAssetReference(_mainMenuSceneRef);
        }
        
        private string GetSceneNameByAssetReference(AssetReference sceneRef)
        {
            IList<IResourceLocation> resourceLocations = Addressables.LoadResourceLocationsAsync(sceneRef).WaitForCompletion();
            IResourceLocation resourceLocation = resourceLocations.First();
            return resourceLocation.PrimaryKey;
        }
    }
}
