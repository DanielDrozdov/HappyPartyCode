using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Screens
{
    public class InGameUIScreensSwitcher : MonoBehaviour, IInGameUIScreensSwitcher
    {
        [SerializeField, ReadOnly] 
        private GameScreen _mainGameScreen;

        [SerializeField, ReadOnly] 
        private InGameMenuScreen _menuScreen;

        public void OpenGameScreen()
        {
            OpenScreen(_mainGameScreen);
        }

        public void OpenMenuScreen()
        {
            OpenScreen(_menuScreen);
        }

        private void OpenScreen(GameBaseScreen screenForActivation)
        {
             _mainGameScreen.Toogle(false);
             _menuScreen.Toogle(false);
             
             screenForActivation.Toogle(true);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_mainGameScreen == null) _mainGameScreen = GetComponentInChildren<GameScreen>();
            
            if (_menuScreen == null) _menuScreen = GetComponentInChildren<InGameMenuScreen>();
        }
#endif
    }
}
