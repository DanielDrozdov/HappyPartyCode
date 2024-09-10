using Data.Network;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class StandaloneInputProvider : BasePlatformInputProvider
    {
        public override PlayerNetworkInput GetInput()
        {
            return new PlayerNetworkInput()
            {
                MovementVector = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical")),
                IsJumpPressed = IsJumpPressed
            };
        }

        protected override bool SetJumpPressedState()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Space);
        }
    }
}
