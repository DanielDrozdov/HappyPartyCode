using Core.Player;
using Fusion;

namespace Core.Network
{
    public interface IPlayersMiniGameDeactivator
    {
        void DeactivatePlayerForMiniGame(NetworkRunner currentRunner, PlayerMovement player);
    }
}