using Core.MiniGames;
using Core.Player;
using Core.Storages;
using Fusion;
using Infrastructure.Services.Input;
using UI.InGame.Input;
using UnityEngine;

namespace Core.Network
{
    public class PlayersMiniGameDeactivator : IPlayersMiniGameDeactivator
    {
        private IInputPanelSwitcher _inputPanelSwitcher;
        private IApplicationInputBlocker _applicationInputBlocker;
        private IDeactivatedPlayersPlatform _deactivatedPlayersPlatform;
        private IMiniGamePlayersStorage _miniGamePlayersStorage;
        

        public PlayersMiniGameDeactivator(IInputPanelSwitcher inputPanelSwitcher, IApplicationInputBlocker applicationInputBlocker, 
            IDeactivatedPlayersPlatform deactivatedPlayersPlatform, IMiniGamePlayersStorage miniGamePlayersStorage)
        {
            _inputPanelSwitcher = inputPanelSwitcher;
            _applicationInputBlocker = applicationInputBlocker;
            _deactivatedPlayersPlatform = deactivatedPlayersPlatform;
            _miniGamePlayersStorage = miniGamePlayersStorage;
        }

        public void DeactivatePlayerForMiniGame(NetworkRunner currentRunner, PlayerMovement player)
        {
            if (player.HasInputAuthority)
            {
                _inputPanelSwitcher.Hide();
                _applicationInputBlocker.BlockUserInput();
            }

            if (currentRunner.IsServer)
            {
                _miniGamePlayersStorage.SetPlayerAsInactive(player.Object.InputAuthority);
                Vector3 pos = _deactivatedPlayersPlatform.GetPlatformRandomPosition();
                player.Teleport(pos);
            }
        }
    }
}
