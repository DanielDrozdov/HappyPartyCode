using TMPro;
using UnityEngine;

namespace UI.Player
{
    public class PlayerNicknamePanelView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _nicknameText;

        public void UpdateNickname(string nickname)
        {
            _nicknameText.text = nickname;
        }
    }
}
