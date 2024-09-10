using System;
using Fusion;

namespace Core.Network
{
    public abstract class MiniGameStarter : NetworkBehaviour, IMiniGameStarter, IMiniGameCallbacks
    {
        public virtual void StartMiniGame()
        {
            OnMiniGameStarted?.Invoke();
            RPC_OnMiniGameStarted();
        }

        protected virtual void EndMiniGame()
        {
            OnMiniGameEnded?.Invoke();
            RPC_OnMiniGameEnded();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_OnMiniGameStarted()
        {
            OnMiniGameStarted?.Invoke();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_OnMiniGameEnded()
        {
            OnMiniGameEnded?.Invoke();
        }

        
        public event Action OnMiniGameStarted;
        public event Action OnMiniGameEnded;
    }
}