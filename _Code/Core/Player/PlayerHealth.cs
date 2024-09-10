using Core.Network;
using DG.Tweening;
using Fusion;
using UnityEngine;
using Zenject;

namespace Core.Player
{
    [RequireComponent(typeof(PlayerDamageReceiveBlinker))]
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField]
        private int _health;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private PlayerDamageReceiveBlinker _damageReceiveBlinker;

        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private PlayerMovement _playerMovement;

        private IPlayersMiniGameDeactivator _playersMiniGameDeactivator;

        [Inject]
        private void Construct(IPlayersMiniGameDeactivator playersMiniGameDeactivator)
        {
            _playersMiniGameDeactivator = playersMiniGameDeactivator;
        }
        
        public void Hit(int damage)
        {
            _health -= damage;

            TweenCallback onCompletedDamageReceiveAction = _health <= 0 ? RPC_Die : null;
            
            _damageReceiveBlinker.BlinkDamageReceive(onCompletedDamageReceiveAction);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_Die()
        {
            _playersMiniGameDeactivator.DeactivatePlayerForMiniGame(Runner, _playerMovement);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_damageReceiveBlinker == null) TryGetComponent(out _damageReceiveBlinker);

            if (_playerMovement == null) TryGetComponent(out _playerMovement);
        }
#endif
    }
}
