using UnityEngine;

namespace UI.InGame.Input
{
    public class InputPanelSwitcher : MonoBehaviour, IInputPanelSwitcher
    {
        [SerializeField] 
        private GameObject _inputPanel;
        
        public void Hide()
        {
            _inputPanel.SetActive(false);    
        }

        public void Show()
        {
            _inputPanel.SetActive(true);    
        }
    }
}
