using Core.MiniGames.AncientLasers.Arena;
using Data.Levels.AncientLasers;
using UnityEngine;
using Zenject;

namespace Infrastructure.ZenjectBindings.AncientLasers
{
    public class AncientLasersDataBindings : MonoInstaller
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private AncientArenaLasersThrower _ancientArenaLasersThrower;
        
        [SerializeField]
        private AncientLasersArenaSettings _arenaSettings;

        public override void InstallBindings()
        {
            BindAncientLasersArenaSettings();
            
            Container.InjectGameObject(_ancientArenaLasersThrower.gameObject);
        }

        private void BindAncientLasersArenaSettings()
        {
            Container
                .Bind<AncientLasersArenaSettings>()
                .FromInstance(_arenaSettings)
                .AsSingle()
                .NonLazy();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ancientArenaLasersThrower == null) _ancientArenaLasersThrower = FindObjectOfType<AncientArenaLasersThrower>();
        }
#endif
    }
}
