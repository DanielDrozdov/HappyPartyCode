using Data.Levels;

namespace Infrastructure.Network
{
    public interface INetworkSceneSettingsProvider
    {
        LevelSettingsData CurrentSceneSettings { get; }
    }

    public interface INetworkSceneSettingsChanger
    {
        void ChangeLevelSettings(LevelSettingsData newSettings);
    }
}