using TMPro;
using UnityEngine;

namespace UI.InGame.PlayersScoresPanel
{
    public class PlayerScorePanelView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _nicknameText;

        [SerializeField] 
        private TMP_Text _scoreText;

        public void UpdatePlayerData(string nickname, int score)
        {
            _nicknameText.text = nickname;
            _scoreText.text = score.ToString();
        }
    }
}