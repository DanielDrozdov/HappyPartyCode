using Data.Levels;

namespace Infrastructure.Network
{
    public class NetworkSceneSettingsProvider : INetworkSceneSettingsProvider, INetworkSceneSettingsChanger
    {
        public LevelSettingsData CurrentSceneSettings { get; private set; }

        public void ChangeLevelSettings(LevelSettingsData newSettings)
        {
            CurrentSceneSettings = newSettings;
        }
    }
}
