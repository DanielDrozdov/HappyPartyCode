using Core.Player.Animations;
using Data.Network;
using Fusion;
using UnityEngine;

namespace Core.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private NetworkCharacterController _characterController;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private PlayerAnimations _playerAnimations;
        
        private readonly float _jumpBlockTimeDelay = 0.4f;
        private float _lastJumpTime;
        private bool _ifNeedTeleport;

        private Vector3 _forwardMoveVectorRelativeToCamera;
        private Vector3 _rightMoveVectorRelativeToCamera;
        private Vector3 _teleportPos;
        private Quaternion _teleportRotation;
        
        private void Awake()
        {
            SetMoveVectorsRelativeToCamera();
        }

        public override void FixedUpdateNetwork()
        {
            if (_ifNeedTeleport)
            {
                _characterController.Teleport(_teleportPos, _teleportRotation);
                _ifNeedTeleport = false;
            }

            if (GetInput(out PlayerNetworkInput input))
            {
                Move(input.MovementVector);

                if (input.IsJumpPressed && Time.time - _lastJumpTime >= _jumpBlockTimeDelay && _characterController.Grounded)
                {
                    Jump();
                }
            }
        }

        public void Teleport(Vector3 teleportPos, Quaternion? rotation = null)
        {
            _ifNeedTeleport = true;
            _teleportPos = teleportPos;
            _characterController.Velocity = Vector3.zero;
            _teleportRotation = rotation ?? Quaternion.identity;
        }

        private void Move(Vector2 movementInputVector)
        {
            Vector3 movementVector = (_forwardMoveVectorRelativeToCamera * movementInputVector.y + _rightMoveVectorRelativeToCamera * movementInputVector.x).normalized;
            _characterController.Move(movementVector);
        }

        private void Jump()
        {
            _characterController.Jump();
            _playerAnimations.AnimateJump();
            _lastJumpTime = Time.time;
        }

        private void SetMoveVectorsRelativeToCamera()
        {
            Transform cameraTrm = Camera.main.transform;
            
            Vector3 forward = cameraTrm.forward;
            Vector3 right = cameraTrm.right;

            forward.y = 0;
            right.y = 0;
            
            _forwardMoveVectorRelativeToCamera = forward.normalized;
            _rightMoveVectorRelativeToCamera= right.normalized;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_characterController == null) TryGetComponent(out _characterController);
            
            if (_playerAnimations == null) _playerAnimations = GetComponentInChildren<PlayerAnimations>();
        }
#endif
    }
}
