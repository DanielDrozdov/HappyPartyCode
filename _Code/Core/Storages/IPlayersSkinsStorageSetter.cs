using Fusion;

namespace Core.Storages
{
    public interface IPlayersSkinsStorageSetter
    {
        void SetPlayerSkin(PlayerRef playerRef, byte skinId);
    }
}