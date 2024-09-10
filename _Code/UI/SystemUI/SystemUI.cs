using UnityEngine;

namespace UI.System
{
    public class SystemUI : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
