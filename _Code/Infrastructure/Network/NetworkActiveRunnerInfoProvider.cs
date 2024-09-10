using System.Collections.Generic;
using Fusion;

namespace Infrastructure.Network
{
    public class NetworkActiveRunnerInfoProvider : INetworkActiveRunnerInfoProviderNewRunnerSetter, INetworkActiveRunnerInfoProvider
    {
        private NetworkRunner _runner;

        public IEnumerable<PlayerRef> ActiveSessionPlayers => _runner.ActivePlayers;
        public PlayerRef LocalPlayerRef => _runner.LocalPlayer;

        public void SetNewRunner(NetworkRunner runner)
        {
            _runner = runner;
        }
    }
}
