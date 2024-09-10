using Data.Network;
using JoysticksPack;
using UI.InGame.Input;

namespace Infrastructure.Services.Input
{
    public class MobileInputProvider : BasePlatformInputProvider
    {
        private bool _isUIJumpButtonPressed;
        
        public MobileInputProvider()
        {
            JumpButton.OnPressed += OnUIJumpButtonPressed;
        }

        public override PlayerNetworkInput GetInput()
        {
            return new PlayerNetworkInput()
            {
                MovementVector = Joystick.Direction,
                IsJumpPressed = IsJumpPressed
            };
        }

        protected override bool SetJumpPressedState()
        {
            bool isPressed = _isUIJumpButtonPressed;
            _isUIJumpButtonPressed = false;
            
            return isPressed;
        }

        private void OnUIJumpButtonPressed()
        {
            _isUIJumpButtonPressed = true;
        }
    }
}