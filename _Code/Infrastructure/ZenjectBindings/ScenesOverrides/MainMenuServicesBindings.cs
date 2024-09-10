using Core.MainMenu.Podium;
using UnityEngine;

namespace Infrastructure.ZenjectBindings.ScenesOverrides
{
    public class MainMenuServicesBindings : SceneServicesBindings
    {
        [SerializeField] 
        private PlayersPodium _playersPodium;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            BindPlayersRoomPodium();
        }

        private void BindPlayersRoomPodium()
        {
            Container
                .Bind<IPlayersPodium>()
                .To<PlayersPodium>()
                .FromInstance(_playersPodium)
                .AsSingle()
                .NonLazy();
        }
    }
}