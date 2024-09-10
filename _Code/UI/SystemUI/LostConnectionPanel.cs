using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.System
{
    public class LostConnectionPanel : MonoBehaviour
    {
        [SerializeField, Unit(Units.Second)] 
        private float _checkConnectionEverySeconds;

        [SerializeField] 
        private GameObject _lostConnectionPanel;
        
        private float _remainderUntilNextConnectionCheck;
        private bool _isActivated;

        private void Awake()
        {
            _remainderUntilNextConnectionCheck = _checkConnectionEverySeconds;
        }

        private void Update()
        {
            _remainderUntilNextConnectionCheck -= Time.deltaTime;

            if (_remainderUntilNextConnectionCheck <= 0 && Application.internetReachability == NetworkReachability.NotReachable
                                                        && !_isActivated)
            {
                SwitchPanel(true);
            } 
            else if (_isActivated && Application.internetReachability != NetworkReachability.NotReachable)
            {
                SwitchPanel(false);
            }
        }

        private void SwitchPanel(bool value)
        {
            _lostConnectionPanel.SetActive(value);
            _isActivated = value;
        }
    }
}
