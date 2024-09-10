using System;

namespace Core.Player.SkinChangers
{
    public class PlayerSkinChanger : PlayerBaseSkinChanger
    {
        public event Action OnSkinChanged;
        
        public override async void Spawned()
        {
            byte skinId = _playersSkinsStorage.GetPlayerSkinIdByRef(Object.InputAuthority);
            await UpdateSkin(skinId);
            OnSkinChanged?.Invoke();
        }
    }
}