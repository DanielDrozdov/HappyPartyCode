using Fusion;
using UnityEngine;

namespace Core.MiniGames.AncientLasers.Laser
{
    public class AncientLaserEffect : NetworkBehaviour
    {
        [SerializeField] 
        private float _hitOffset;
        
        [SerializeField]
        private Transform _firstHitEffectObj;
        
        [SerializeField]
        private Transform _secondHitEffectObj;

        [SerializeField] 
        private Transform _rightPlateTrm;

        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private LineRenderer _laserLineRenderer;

        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private ParticleSystem[] _effects;
        
        private Transform _transform;
        
        private void Awake()
        {
            ActivateEffect();
            _transform = transform;
        }

        public override void Render()
        {
            _laserLineRenderer.SetPosition(0, _transform.position);
            _laserLineRenderer.SetPosition(1, _rightPlateTrm.position);

            Vector3 invertLaserDir = (_transform.position - _rightPlateTrm.position).normalized;
            Vector3 laserDir = (_rightPlateTrm.position - _transform.position).normalized;
            _secondHitEffectObj.position = _rightPlateTrm.position + invertLaserDir * _hitOffset;
            _firstHitEffectObj.position = _transform.position + laserDir * _hitOffset;
        }

        private void ActivateEffect()
        {
            foreach (var AllPs in _effects)
            {
                if (!AllPs.isPlaying) AllPs.Play();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_laserLineRenderer == null) TryGetComponent(out _laserLineRenderer);
            
            if (_effects == null) _effects = GetComponentsInChildren<ParticleSystem>();
        }
#endif
    }
}
