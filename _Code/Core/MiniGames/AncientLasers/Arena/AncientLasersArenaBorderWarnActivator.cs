using System;
using Data.Levels.AncientLasers;
using Fusion;
using UnityEngine;
using UpdateSys;
using Zenject;

namespace Core.MiniGames.AncientLasers.Arena
{
    public class AncientLasersArenaBorderWarnActivator : NetworkBehaviour, IUpdatable
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private MeshRenderer _meshRenderer;

        private AncientLasersArenaSettings _arenaSettings;
        private MaterialPropertyBlock _materialPropertyBlock;

        private readonly string _matColorPropertyName = "_Color";
        private Color _normalColor;

        private float _warnActivationRemainder;
        private Action _onWarnEnded;

        [Inject]
        private void Construct(AncientLasersArenaSettings arenaSettings)
        {
            _arenaSettings = arenaSettings;
        }
        
        private void Awake()
        {
            _materialPropertyBlock = new();
            _normalColor = _meshRenderer.material.color;
        }
        
        public void OnSystemUpdate(float deltaTime)
        {
            _warnActivationRemainder -= deltaTime;

            if (_warnActivationRemainder <= 0)
            {
                _onWarnEnded.Invoke();
                ChangeBorderColor(_normalColor);
                RPC_ChangeProxiesBorderColor(false);
                this.StopUpdate();
            }
        }

        public void ActivateWarnState(Action onWarnEnded)
        {
            _onWarnEnded = onWarnEnded;
            _warnActivationRemainder = _arenaSettings.BorderLasersSpawnWarnDuration;
            ChangeBorderColor(_arenaSettings.BorderWarnColor);
            RPC_ChangeProxiesBorderColor(true);
            this.StartUpdate();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_ChangeProxiesBorderColor(NetworkBool isInWarnState)
        {
            Color color = isInWarnState ? _arenaSettings.BorderWarnColor : _normalColor;
            ChangeBorderColor(color);
        }

        private void ChangeBorderColor(Color color)
        {
            _materialPropertyBlock.SetColor(_matColorPropertyName, color);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_meshRenderer == null) _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
#endif
    }
}