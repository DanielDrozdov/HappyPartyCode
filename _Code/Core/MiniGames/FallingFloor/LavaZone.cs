using Core.Network;
using Core.Player;
using Fusion;
using UnityEngine;
using Zenject;

namespace Core.MiniGames.FallingFloor
{
    public class LavaZone : NetworkBehaviour
    {
        private IPlayersMiniGameDeactivator _playersMiniGameDeactivator;

        [Inject]    
        private void Construct(IPlayersMiniGameDeactivator playersMiniGameDeactivator)
        {
            _playersMiniGameDeactivator = playersMiniGameDeactivator;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                _playersMiniGameDeactivator.DeactivatePlayerForMiniGame(Runner, player);
            }
        }
    }
}