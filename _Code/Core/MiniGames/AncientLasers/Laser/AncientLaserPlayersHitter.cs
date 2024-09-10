using Core.Player;
using Fusion;
using UnityEngine;

namespace Core.MiniGames.AncientLasers.Laser
{
    public class AncientLaserPlayersHitter : NetworkBehaviour
    {
        [SerializeField] 
        private int _hitDamage;

        [SerializeField] 
        private AncientLaserPlayersTrigger _playersTrigger;
        
        public override void Spawned()
        {
            if (!HasStateAuthority) return;

            _playersTrigger.OnCaughtPlayer += HitPlayer;
        }

        private void HitPlayer(Collider player)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.Hit(_hitDamage);
        }
    }
}