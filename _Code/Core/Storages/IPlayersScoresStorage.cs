using System.Collections.ObjectModel;
using Fusion;

namespace Core.Storages
{
    public interface IPlayersScoresStorage
    {
        int GetPlayerTotalScore(PlayerRef playerRef);
        ReadOnlyDictionary<PlayerRef, int> GetPlayersTotalScores();
    }
}