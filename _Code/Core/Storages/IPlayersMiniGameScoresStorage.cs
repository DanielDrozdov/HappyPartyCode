using System.Collections.ObjectModel;
using Fusion;

namespace Core.Storages
{
    public interface IPlayersMiniGameScoresStorage
    {
        void AddMiniGameScoreToPlayer(byte playerId, int score);
        void TransferMiniGameScoresToTotalBalance();
        int GetPlayerMiniGameScore(PlayerRef playerRef);
        ReadOnlyDictionary<PlayerRef, int> GetPlayersMiniGameScores();
    }
}