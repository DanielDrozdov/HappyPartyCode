using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure
{
    public class FirstLoadSceneLoader : MonoBehaviour
    {
        [SerializeField] 
        private AssetReference _mainMenuRef;
        
        private static bool _isInitialized;
        
        private void Awake()
        {
            if (_isInitialized) return;
            
            InitializeGame();
        }

        private void InitializeGame()
        {
            _isInitialized = true;
            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            Addressables.LoadSceneAsync(_mainMenuRef);
        }
    }
}
