using Infrastructure.Network;
using UI.Screens;
using UnityEngine;
using Zenject;

namespace UI.InGameMenu
{
    public class InGameMenuPresenter : MonoBehaviour
    {
        private IInGameUIScreensSwitcher _gameUIScreensSwitcher;
        private INetworkDisconnecter _networkDisconnecter;

        [Inject]
        private void Construct(IInGameUIScreensSwitcher gameUIScreensSwitcher, INetworkDisconnecter networkDisconnecter)
        {
            _gameUIScreensSwitcher = gameUIScreensSwitcher;
            _networkDisconnecter = networkDisconnecter;
        }
        
        public void ReturnToGame()
        {
            _gameUIScreensSwitcher.OpenGameScreen();
        }

        public void LeaveGame()
        {
            _networkDisconnecter.ShutdownConnection();
        }
    }
}
