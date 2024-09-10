using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UpdateSys;
using Utility;

namespace UI.System
{
    public class SceneTransitionPanel : MonoBehaviour, IUpdatable, ISceneTransition
    {
        [SerializeField] 
        private float _transitionSpeed;

        [SerializeField] 
        private float _maxTransitionCircleRadius;
        
        [SerializeField, ReadOnly] 
        private Image _image;

        private Material _imageMat;
        private Action _onTransitionCompleted;
        private float _targetCircleRadius;
        private float _circleRadius;
        private bool _ifNeedDeactivatePanel;
        
        private static readonly int _radius = Shader.PropertyToID("_Radius");

        private void Awake()
        {
            _imageMat = _image.material;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            _circleRadius = Mathf.MoveTowards(_circleRadius, _targetCircleRadius, deltaTime * _transitionSpeed); 
            UpdateMaterialData();
            
            if (_circleRadius == _targetCircleRadius)
            {
                if (_ifNeedDeactivatePanel)
                {
                    _image.gameObject.SetActive(false);
                    _ifNeedDeactivatePanel = false;
                }

                _onTransitionCompleted?.Invoke();
                this.StopUpdate();
            }
        }

        public void StartFadeInTransition(Action onTransitionCompleted)
        {
            StartCoroutine(StartFadeInWithOneQuarterOfSecondDelay(onTransitionCompleted));
        }

        public void StartFadeOutTransition(Action onTransitionCompleted)
        {
            StartFadeTransition(_maxTransitionCircleRadius, 0, onTransitionCompleted);
        }

        private IEnumerator StartFadeInWithOneQuarterOfSecondDelay(Action onTransitionCompleted)
        {
            yield return WaitForConstants.QuarterOfSecond;
            
            StartFadeTransition(0, _maxTransitionCircleRadius, onTransitionCompleted);
            _ifNeedDeactivatePanel = true;
        }

        private void StartFadeTransition(float startCircleRadius, float targetCircleRadius, Action onTransitionCompleted)
        {
            _circleRadius = startCircleRadius;
            _targetCircleRadius = targetCircleRadius;
            _onTransitionCompleted = onTransitionCompleted;
            _image.gameObject.SetActive(true);
            UpdateMaterialData();
            this.StartUpdate();
        }

        private void UpdateMaterialData()
        {
            _imageMat.SetFloat(_radius, _circleRadius);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_image == null) _image = GetComponentInChildren<Image>();
        }
#endif
    }
}