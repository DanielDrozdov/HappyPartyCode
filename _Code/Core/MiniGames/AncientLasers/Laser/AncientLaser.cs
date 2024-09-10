using Core.MiniGames.AncientLasers.Arena;
using Fusion;
using Odin = Sirenix.OdinInspector;
using UnityEngine;

namespace Core.MiniGames.AncientLasers.Laser
{
    [RequireComponent(typeof(AncientLaserMovement))]
    public class AncientLaser : NetworkBehaviour
    {
        [SerializeField] 
        private int _id;

        [SerializeField] 
        private GameObject _leftPlate;

        [SerializeField] 
        private GameObject _rightPlate;

        [SerializeField, Odin.ReadOnly] 
        private AncientLaserMovement _movement;

        [SerializeField, Odin.ReadOnly] 
        private AncientLaserPlayersTrigger _playersTrigger;

        public int Id => _id;

        private AncientArenaLasersPool _lasersPool;
        private Vector3 _deactivatedPos = new Vector3(0, 1000);

        public override void Spawned()
        {
            _movement.Teleport(_deactivatedPos, Quaternion.identity);
        }

        public void SetLasersPool(AncientArenaLasersPool lasersPool)
        {
            if (_lasersPool != null) return;
            
            _lasersPool = lasersPool;
        }

        public void ActivateLaserInDirection(float borderWidth, Vector3 startPoint, Quaternion forwardRotation, Vector3 endPoint)
        {
            endPoint.y = startPoint.y;
            _movement.Teleport(startPoint, forwardRotation, () =>
            {
                Activate(borderWidth);
                _playersTrigger.SetXSize(borderWidth);
                _movement.MoveTo(endPoint, Deactivate);
            });
        }

        private void Activate(float borderWidth)
        {
            _leftPlate.transform.localPosition = new Vector3(-borderWidth / 2, 0, 0);
            _rightPlate.transform.localPosition = new Vector3(borderWidth / 2, 0, 0);
            SwitchLaserComponents(true);

            if (Runner.IsServer)
            {
                RPC_ActivateSelfOnProxies(borderWidth);
            }
        }

        private void Deactivate()
        {
            _movement.Teleport(_deactivatedPos, Quaternion.identity, () =>
            {
                SwitchLaserComponents(false);
                RPC_DeactivateSelfOnProxies();
            });
            
            _lasersPool.ReturnLaserToPool(this);
        }

        private void SwitchLaserComponents(bool value)
        {
            if (HasStateAuthority)
            {
                _playersTrigger.Switch(value);
            }

            _leftPlate.SetActive(value);
            _rightPlate.SetActive(value);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_ActivateSelfOnProxies(float borderWidth)
        {
            Activate(borderWidth);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_DeactivateSelfOnProxies()
        {
            SwitchLaserComponents(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_movement == null) TryGetComponent(out _movement);

            if (_playersTrigger == null) _playersTrigger = GetComponentInChildren<AncientLaserPlayersTrigger>();
        }
#endif
    }
}