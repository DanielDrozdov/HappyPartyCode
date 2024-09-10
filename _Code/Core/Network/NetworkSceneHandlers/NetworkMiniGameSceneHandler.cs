using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Network.SceneHandlers
{
    public class NetworkMiniGameSceneHandler : NetworkSceneHandler
    {
        [SerializeField, ReadOnly] 
        private NetworkMiniGameStarter _networkMiniGameStarter;
        
        public override void Spawned()
        {
            base.Spawned();

            if (Runner.IsServer)
            {
                OnSceneFadeInTransitionCompleted += _networkMiniGameStarter.StartMiniGame;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_networkMiniGameStarter == null) _networkMiniGameStarter = GetComponentInChildren<NetworkMiniGameStarter>();
        }
#endif
    }
}
