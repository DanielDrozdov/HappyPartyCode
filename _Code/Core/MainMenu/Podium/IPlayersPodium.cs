using Fusion;
using UnityEngine;

namespace Core.MainMenu.Podium
{
    public interface IPlayersPodium
    {
        Pedestal GetPedestalByNumber(int number);
        void ReservePedestalForPlayer(int pedestalNumber, NetworkObject player);
        void FreePedestalFromPlayer(PlayerRef player);
    }
}