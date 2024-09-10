using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.SkinChanger
{
    [RequireComponent(typeof(SkinChangerPanelPresenter))]
    public class SkinChangerPanelView : MonoBehaviour
    {
        [SerializeField] 
        private Button _previousSkinButton;

        [SerializeField] 
        private Button _nextSkinButton;

        [SerializeField, ReadOnly] 
        private SkinChangerPanelPresenter _presenter;

        private void Awake()
        {
            _previousSkinButton.onClick.AddListener(SelectPreviousSkin_OnClickButton);
            _nextSkinButton.onClick.AddListener(SelectNextSkin_OnClickButton);
        }

        private void SelectPreviousSkin_OnClickButton()
        {
            _presenter.SelectPreviousSkin();
        }

        private void SelectNextSkin_OnClickButton()
        {
            _presenter.SelectNextSkin();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_presenter == null) TryGetComponent(out _presenter);
        }
#endif
    }
}
