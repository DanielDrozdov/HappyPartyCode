using Fusion;
using UnityEngine;

namespace Core.Player.Animations
{
    public class PlayerAnimations : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private NetworkCharacterController _characterController;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private Animator _animator;

        private int _moveSpeedHash;
        private int _isGroundedHash;
        private int _jumpTriggerHash;
        private bool _isJumping;
        private bool _isFlying;
        private readonly float _groundLostMaxDuration = 0.25f;
        private float _groundLostMaxDurationBalance;

        private void Awake()
        {
            InitializeAnimationsHashes();
        }

        public override void Render()
        {
            float kMoveSpeed = _characterController.Velocity.magnitude / _characterController.maxSpeed;
            _animator.SetFloat(_moveSpeedHash, kMoveSpeed);
            _animator.SetBool(_isGroundedHash, _characterController.Grounded);
            
            CheckJumpingState();
            CheckIfPlayerLostGroundWithoutJump();
        }

        public void AnimateJump()
        {
            _isJumping = true;
            _animator.SetTrigger(_jumpTriggerHash);
        }

        private void CheckIfPlayerLostGroundWithoutJump()
        {
            if (!_characterController.Grounded && !_isJumping)
            {
                _groundLostMaxDurationBalance += Time.deltaTime;

                if (_groundLostMaxDurationBalance > _groundLostMaxDuration)
                {
                    AnimateJump();
                    _groundLostMaxDurationBalance = 0;
                }
            }
        }

        private void CheckJumpingState()
        {
            if (_isJumping)
            {
                bool IfLostGroundAfterJump = !_isFlying && !_characterController.Grounded;
                bool IfLanded = _isFlying && _characterController.Grounded;
                
                if (IfLostGroundAfterJump)
                {
                    _isFlying = true;
                }
                else if (IfLanded)
                {
                    _isJumping = false;
                    _isFlying = false;
                }
            }
        }

        private void InitializeAnimationsHashes()
        {
            _moveSpeedHash = Animator.StringToHash("MoveSpeed");
            _jumpTriggerHash = Animator.StringToHash("JumpTrigger");
            _isGroundedHash = Animator.StringToHash("IsGrounded");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_characterController == null) transform.parent.TryGetComponent(out _characterController);

            if (_animator == null) _animator = GetComponentInChildren<Animator>();
        }
#endif
    }
}
