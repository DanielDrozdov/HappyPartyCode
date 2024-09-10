using System.Collections.Generic;
using Fusion;

namespace Core.Storages
{
    public interface IPlayersScoresStorageInitializer
    {
        void Initialize(IEnumerable<PlayerRef> playersRefs);
        bool IsInitialized { get; }
    }
}