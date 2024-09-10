
using System.Collections.ObjectModel;
using Fusion;

namespace Core.Storages
{
    public interface IMiniGamePlayersStorage
    {
        ReadOnlyCollection<PlayerRef> ActiveLivePlayers { get; }
        void SetPlayerAsInactive(PlayerRef player);
    }
}