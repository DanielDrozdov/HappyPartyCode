using Data.Network;
using UniRx;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public abstract class BasePlatformInputProvider : IPlatformInputProvider
    {
        private bool _isJumpPressed;

        protected bool IsJumpPressed
        {
            get
            {
                bool isJumpPressed = _isJumpPressed;
                _isJumpPressed = false;
                
                return isJumpPressed;
            }
        }

        protected BasePlatformInputProvider()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => CheckForJumpPressed());
        }

        public abstract PlayerNetworkInput GetInput(); 
        protected abstract bool SetJumpPressedState();

        private void CheckForJumpPressed()
        {
            if (_isJumpPressed) return;
            
            _isJumpPressed = SetJumpPressedState();
        }

    }
}