using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.MainMenu.Podium
{
    public class ScorePodiumCamera : MonoBehaviour
    {
        [SerializeField] 
        private float _animationDuration;
        
        [SerializeField, FoldoutGroup("Components")] 
        private Transform _cameraFollowPoint;

        [SerializeField, FoldoutGroup("Components")] 
        private Transform _endAnimationPoint;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")] 
        private CinemachineVirtualCamera _podiumCam;

        private readonly int _secondaryCameraPriority = 20;
        
        public void StartAnimationLookTransferToPlayersScorePodium(TweenCallback onCompleted)
        {
            _podiumCam.Priority = _secondaryCameraPriority;

            _cameraFollowPoint.transform
                .DOMoveY(_endAnimationPoint.position.y, _animationDuration)
                .OnComplete(onCompleted);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_podiumCam == null) _podiumCam = GetComponentInChildren<CinemachineVirtualCamera>();
        }
#endif
    }
}
