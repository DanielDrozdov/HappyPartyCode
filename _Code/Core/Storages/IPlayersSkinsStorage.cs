using System;
using Fusion;
using UniRx;
using UnityEngine.AddressableAssets;

namespace Core.Storages
{
    public interface IPlayersSkinsStorage
    {
        int ModelsCountInStorage { get; }
        IReadOnlyReactiveProperty<byte> ReadOnlyLocalPlayerSkinId { get; }
        event Action OnLocalPlayerSkinInitialized;
        AssetReference GetSkinReferenceById(byte skinId);
        byte GetPlayerSkinIdByRef(PlayerRef playerRef);
    }
}