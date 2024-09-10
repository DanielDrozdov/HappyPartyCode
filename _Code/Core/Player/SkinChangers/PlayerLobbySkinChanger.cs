using Core.Storages;
using Fusion;
using UniRx;
using Zenject;

namespace Core.Player.SkinChangers
{
    public class PlayerLobbySkinChanger : PlayerBaseSkinChanger
    {
        [Networked, OnChangedRender(nameof(OnSkinIdChanged))]
        private byte _skinId { get; set; }
        
        private IPlayersSkinsStorageSetter _playersSkinsStorageSetter;

        [Inject]
        private void Construct2(IPlayersSkinsStorageSetter playersSkinsStorageSetter)
        {
            _playersSkinsStorageSetter = playersSkinsStorageSetter;
        }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                _playersSkinsStorage.ReadOnlyLocalPlayerSkinId.Subscribe(RPC_SetPlayerSkinIdToServer).AddTo(this);
                RPC_SetPlayerSkinIdToServer(_playersSkinsStorage.ReadOnlyLocalPlayerSkinId.Value);
            }
            else if (_skinId != 0)
            {
                SetNewSkin();
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_SetPlayerSkinIdToServer(byte skinId)
        {
            _skinId = skinId;
        }

        private void OnSkinIdChanged()
        {
            SetNewSkin();
        }

        private void SetNewSkin()
        {
            _playersSkinsStorageSetter.SetPlayerSkin(Object.InputAuthority, _skinId);
            UpdateSkin(_skinId);
        }
    }
}
