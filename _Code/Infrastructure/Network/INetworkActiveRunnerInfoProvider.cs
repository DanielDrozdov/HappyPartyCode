using System.Collections.Generic;
using Fusion;

namespace Infrastructure.Network
{
    public interface INetworkActiveRunnerInfoProvider
    {
        IEnumerable<PlayerRef> ActiveSessionPlayers { get; }
        PlayerRef LocalPlayerRef { get; }
    }
}