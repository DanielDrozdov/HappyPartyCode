using Data.Network;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class ApplicationInputProvider : IApplicationInputProvider, IApplicationInputBlocker
    {
        private readonly IPlatformInputProvider _platformInputProvider;
        private bool _isInputBlocked;
        
        public ApplicationInputProvider()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                _platformInputProvider = new StandaloneInputProvider();
            }
            else
            {
                _platformInputProvider = new MobileInputProvider();
            }
        }

        public bool IsInputBlocked() => _isInputBlocked;

        public PlayerNetworkInput GetPlayerInputForNetwork()
        {
            if (_isInputBlocked)
            {
                return new PlayerNetworkInput();
            }
            
            return _platformInputProvider.GetInput();
        }

        public void BlockUserInput()
        {
            _isInputBlocked = true;
        }

        public void UnblockUserInput()
        {
            _isInputBlocked = false;
        }
    }
}
