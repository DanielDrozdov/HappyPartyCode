using System;
using Fusion;

namespace Infrastructure.Network
{
    public interface INetworkCallbacksEvents
    {
        event Action OnNetworkShutdown;
        event Action<PlayerRef> OnNetworkPlayerJoined;
        event Action<PlayerRef> OnNetworkPlayerLeft;
    }
}