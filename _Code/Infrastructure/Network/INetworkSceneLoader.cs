using Data.Levels;
using Fusion;

namespace Infrastructure.Network
{
    public interface INetworkSceneLoader
    {
        void LoadScene(NetworkRunner serverRunner, LevelSettingsData levelSettings);
    }
}