using Cinemachine;
using Core.Cameras;
using UnityEngine;

namespace Core.Network.SceneHandlers
{
    public class SceneCameraAssigner : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        
        public void FollowAndLookAt(GameObject localPlayer)
        {
            _cinemachineVirtualCamera.Follow = localPlayer.transform;
            _cinemachineVirtualCamera.LookAt = localPlayer.transform;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cinemachineVirtualCamera == null) _cinemachineVirtualCamera = FindObjectOfType<MainCinemachineCamera>()?.GetComponent<CinemachineVirtualCamera>();
        }
#endif
    }
}
