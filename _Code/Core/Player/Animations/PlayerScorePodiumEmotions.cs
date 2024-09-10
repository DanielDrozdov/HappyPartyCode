using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Player.Animations
{
    public class PlayerScorePodiumEmotions : NetworkBehaviour
    {
        [SerializeField] 
        private int _victoryEmotionsCount;

        [SerializeField] 
        private int _lossEmotionsCount;

        [SerializeField, PropertySpace(15), Sirenix.OdinInspector.ReadOnly]
        private Animator _animator;

        private int _emotionTriggerHash;
        private int _victoryEmotionNumberHash;
        private int _lossEmotionNumberHash;

        private void Awake()
        {
            SetAnimationParametersHashes();
        }
        
        public void PlayVictoryEmotion()
        {
            RPC_PlayRandomEmotion(true);
        }

        public void PlayLossEmotion()
        {
            RPC_PlayRandomEmotion(false);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_PlayRandomEmotion(NetworkBool isVictory)
        {
            int emotionsMaxCount = isVictory ? _victoryEmotionsCount : _lossEmotionsCount;
            int emotionNumberHash = isVictory ? _victoryEmotionNumberHash : _lossEmotionNumberHash;
            int randomEmotionNumber = Random.Range(1, emotionsMaxCount + 1);
            _animator.SetTrigger(_emotionTriggerHash);
            _animator.SetInteger(emotionNumberHash, randomEmotionNumber);
        }

        private void SetAnimationParametersHashes()
        {
            _emotionTriggerHash = Animator.StringToHash("ScorePodiumEmotionTrigger");
            _victoryEmotionNumberHash = Animator.StringToHash("ScorePodiumVictoryEmotionNumber");
            _lossEmotionNumberHash = Animator.StringToHash("ScorePodiumLossNumber");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
        }
#endif
    }
}
