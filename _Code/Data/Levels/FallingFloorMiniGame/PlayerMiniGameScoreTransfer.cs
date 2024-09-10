using Fusion;

namespace Data.Levels.FallingFloorMiniGame
{
    public struct PlayerMiniGameScoreTransfer : INetworkStruct
    {
        public byte PlayerId { get; }
        public int Score { get; }

        public PlayerMiniGameScoreTransfer(byte playerId, int score)
        {
            PlayerId = playerId;
            Score = score;
        }
    }
}

