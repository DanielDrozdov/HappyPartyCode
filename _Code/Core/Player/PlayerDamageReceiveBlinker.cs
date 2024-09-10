using System;
using Core.Player.SkinChangers;
using Data.Character;
using DG.Tweening;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Player
{
    public class PlayerDamageReceiveBlinker : NetworkBehaviour
    {
        [SerializeField, InlineEditor] 
        private PlayerDamageReceiveAnimationSettings _blinkAnimationSettings;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private PlayerSkinChanger _skinChanger;

        private Material _skinMaterial;
        private Color _normalColor = Color.white;
        private TweenCallback _onCompleted;

        private void Awake()
        {
            _skinChanger.OnSkinChanged += SetUpSkinMaterial;
        }

        public void BlinkDamageReceive(TweenCallback onCompleted = null)
        {
            _onCompleted = onCompleted;
            RPC_BlinkDamageReceiveOnProxies();
        }
        
        private void ReturnToNormalColor()
        {
            BlinkColor(_normalColor, _onCompleted);
        }

        private void BlinkColor(Color color, TweenCallback onComplete)
        {
            _skinMaterial
                .DOColor(color, _blinkAnimationSettings.BlinkDurationTime / 2)
                .OnComplete(onComplete);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_BlinkDamageReceiveOnProxies()
        {
            BlinkColor(_blinkAnimationSettings.BlinkColor, ReturnToNormalColor);
        }

        private void SetUpSkinMaterial()
        {
            _skinMaterial = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_skinChanger == null) _skinChanger = GetComponentInChildren<PlayerSkinChanger>();
        }
#endif
    }
}