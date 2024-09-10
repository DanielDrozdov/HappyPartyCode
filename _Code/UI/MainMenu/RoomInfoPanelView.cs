using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Room
{
    [RequireComponent(typeof(RoomInfoPanelPresenter))]
    public class RoomInfoPanelView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _roomNameText;

        [SerializeField] 
        private TMP_Text _playersCountInRoomText;

        [SerializeField] 
        private TMP_Text _nicknameText;
        
        [SerializeField] 
        private Button _startGameButton;
        
        [SerializeField] 
        private Button _leaveRoomButton;
        
        [SerializeField, ReadOnly] 
        private RoomInfoPanelPresenter _presenter;

        private void Awake()
        {
            SubscribeButtons();
        }

        public void UpdatePlayersCountInRoom(int playersInRoom, int maxPlayersRoomSize)
        {
            _playersCountInRoomText.text = $"Players: {playersInRoom}/{maxPlayersRoomSize}";
        }

        public void SetNewRoomSessionSettings(string roomName, string nickname, bool isHost)
        {
            _startGameButton.gameObject.SetActive(isHost);
            
            _roomNameText.text = roomName;
            _nicknameText.text = nickname;
        }
        
        private void SubscribeButtons()
        {
            _startGameButton.onClick.AddListener(_presenter.StartGame);
            _leaveRoomButton.onClick.AddListener(_presenter.LeaveRoom);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_presenter == null) TryGetComponent(out _presenter);
        }
#endif
    }
}
