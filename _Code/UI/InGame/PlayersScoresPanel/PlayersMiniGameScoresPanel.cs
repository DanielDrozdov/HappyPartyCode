using Core.Storages;
using Fusion;
using Zenject;

namespace UI.InGame.PlayersScoresPanel
{
    public class PlayersMiniGameScoresPanel : PlayersScoresPanel, IPlayersMiniGameScoresPanel
    {
        private IPlayersMiniGameScoresStorage _miniGameScoresStorage;

        [Inject]
        private void Construct(IPlayersMiniGameScoresStorage miniGameScoresStorage)
        {
            _miniGameScoresStorage = miniGameScoresStorage;
        }

        public void UpdateMiniGamePlayersScores()
        {
            foreach (PlayerRef playerRef in _playersPanelsDict.Keys)
            {
                UpdateMiniGamePlayerScorePanel(playerRef);
            }
        }

        private void UpdateMiniGamePlayerScorePanel(PlayerRef playerRef)
        {
            int playerScore = _miniGameScoresStorage.GetPlayerMiniGameScore(playerRef);
            _playersPanelsDict[playerRef].UpdateScore(playerScore);
        }
    }
}