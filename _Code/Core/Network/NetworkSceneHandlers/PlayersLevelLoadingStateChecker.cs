using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace Core.Network.SceneHandlers
{
    public class PlayersLevelLoadingStateChecker : NetworkBehaviour
    {
        private List<int> _playersId = new (4);
        
        public event Action OnAllPlayersLoaded;
        
        public override void Spawned()
        {
            RPC_OnLoadedInScene(Runner.LocalPlayer.PlayerId);
        }

        #region Network RPCs
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_OnLoadedInScene(int playerId)
        {
            _playersId.Add(playerId);
            CheckOnServerIfAllPlayersLoaded();
        }
        
        #endregion

        private void CheckOnServerIfAllPlayersLoaded()
        {
            IEnumerable<PlayerRef> activePlayersList = Runner.ActivePlayers;

            if (activePlayersList.Count() != _playersId.Count) return;

            bool hasBrokenPlayerId = false;
            
            foreach (PlayerRef playerRef in activePlayersList)
            {
                if (!_playersId.Contains(playerRef.PlayerId))
                {
                    hasBrokenPlayerId = true;
                }
            }

            if (!hasBrokenPlayerId)
            {
                OnAllPlayersLoaded?.Invoke();
            }
        }
    }
}
