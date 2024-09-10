using System.Collections.Generic;
using Fusion;

namespace Infrastructure.Network.CallbacksServices.Spawners
{
    public interface INetworkCallbacksPlayerSpawner
    {
        void SpawnPlayer(Dictionary<PlayerRef, NetworkObject> playersDict, NetworkRunner runner, PlayerRef playerRef);
        void DespawnPlayer(Dictionary<PlayerRef, NetworkObject> playersDict, NetworkRunner runner, PlayerRef playerRef);
    }
}