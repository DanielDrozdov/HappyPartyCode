using System;
using Data.Levels.AncientLasers;
using Fusion;
using UnityEngine;
using Zenject;

namespace Core.MiniGames.AncientLasers.Laser
{
    public class AncientLaserMovement : NetworkBehaviour
    {
        private Vector3 _teleportPos;
        private Quaternion _teleportRotation;
        private Vector3 _endPoint;
        private Transform _transform;
        private Action _onEndPointReachedAction;
        private Action _onTeleported;
        private float _speed;
        private bool _isMovementActivated;
        private bool _ifWasTeleported;

        [Inject]
        private void Construct(AncientLasersArenaSettings arenaSettings)
        {
            _speed = arenaSettings.LaserSpeed;
        }
        
        private void Awake()
        {
            _transform = transform;
        }

        public override void FixedUpdateNetwork()
        {
            if (_ifWasTeleported)
            {
                transform.position = _teleportPos;
                transform.rotation = _teleportRotation;
                _ifWasTeleported = false;
                _onTeleported?.Invoke();
            }

            if (_isMovementActivated)
            {
                Move();
            }
        }

        public void Teleport(Vector3 position, Quaternion teleportRotation, Action onTeleported = null)
        {
            _teleportPos = position;
            _teleportRotation = teleportRotation;
            _onTeleported = onTeleported;
            _ifWasTeleported = true;
        }
        
        public void MoveTo(Vector3 endPoint, Action onEndPointReachedAction)
        {
            _isMovementActivated = true;
            _endPoint = endPoint;
            _onEndPointReachedAction = onEndPointReachedAction;
        }

        private void Move()
        {
            Vector3 currentPosition = _transform.position;
            
            currentPosition = Vector3.MoveTowards(currentPosition, _endPoint, _speed * Runner.DeltaTime);
            _transform.position = currentPosition;
            
            if (currentPosition == _endPoint)
            {
                _onEndPointReachedAction?.Invoke();
                _isMovementActivated = false;
            }
        }
    }
}