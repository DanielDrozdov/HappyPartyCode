using System.Collections.Generic;
using System.Linq;
using Data.Levels;
using Fusion;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Infrastructure.Network
{
    public class NetworkSceneLoader : INetworkSceneLoader
    {
        private readonly INetworkSceneSettingsChanger _sceneSettingsChanger;

        public NetworkSceneLoader(INetworkSceneSettingsChanger sceneSettingsChanger)
        {
            _sceneSettingsChanger = sceneSettingsChanger;
        }
        
        public void LoadScene(NetworkRunner serverRunner, LevelSettingsData levelSettings)
        {
            string scenePathKey = GetScenePathKey(levelSettings);
            SceneRef startLobbyScene = SceneRef.FromPath(scenePathKey);
            _sceneSettingsChanger.ChangeLevelSettings(levelSettings);
            serverRunner.LoadScene(startLobbyScene);
        }

        private static string GetScenePathKey(LevelSettingsData levelSettings)
        {
            IList<IResourceLocation> resourceLocations = Addressables.LoadResourceLocationsAsync(levelSettings.SceneRef).WaitForCompletion();
            string resourcePathKey = resourceLocations.First()?.PrimaryKey;
            return resourcePathKey;
        }
    }
}
