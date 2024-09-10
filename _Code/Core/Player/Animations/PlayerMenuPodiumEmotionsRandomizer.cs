using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Units = Sirenix.OdinInspector.Units;

namespace Core.Player.Animations
{
    public class PlayerMenuPodiumEmotionsRandomizer : NetworkBehaviour
    {
        [SerializeField] 
        private int _idleAnimationsCount;

        [SerializeField] 
        private int _emotionReactionAnimationsCount;

        [SerializeField, Header("Time"), Sirenix.OdinInspector.Unit(Units.Second)] 
        private float _playEmotionReactionAnimInTime;

        [SerializeField, Sirenix.OdinInspector.ReadOnly, FoldoutGroup("Components")] 
        private NetworkMecanimAnimator _animator;

        [SerializeField, Sirenix.OdinInspector.ReadOnly, FoldoutGroup("Components")]
        private PodiumPlayerAnimationEvents _animationsEvents;

        private bool _isEmotionReactionPlaying;
        private int _defaultIdleNumber;
        private float _remainderTimeToPlayReactionAnim;
        private readonly float _randomTimeInaccuracyToActivateEmotion = 5f;
        
        private int _idleNumberHash;
        private int _emotionReactionNumberHash;
        private int _emotionReactionTriggerHash;
        
        [Networked, OnChangedRender(nameof(OnIdleAnimNumberChanged))]
        private int _idleAnimNumber { get; set; }
        
        [Networked, OnChangedRender(nameof(OnEmotionReactionAnimNumberChanged))]
        private int _emotionReactionAnimNumber { get; set; }
        

        private void Awake()
        {
            SetAnimHashes();
            ResetTimeRemainderToPlayNextReaction();
            _animationsEvents.OnEmotionReactionEnded += OnEmotionReactionEnded;
        }

        public override void Spawned()
        {
            SetDefaultRandomIdle();
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority || !Runner.IsForward || _isEmotionReactionPlaying) return;

            _remainderTimeToPlayReactionAnim -= Runner.DeltaTime;

            if (_remainderTimeToPlayReactionAnim <= 0)
            {
                PlayRandomEmotionReactionAnimation();
            }
        }

        private void OnEmotionReactionEnded()
        {
            ResetTimeRemainderToPlayNextReaction();
            _emotionReactionAnimNumber = 0;
            _isEmotionReactionPlaying = false;
        }

        private void ResetTimeRemainderToPlayNextReaction()
        {
            float randomTimeInaccuracy = Random.Range(-_randomTimeInaccuracyToActivateEmotion, _randomTimeInaccuracyToActivateEmotion);
            _remainderTimeToPlayReactionAnim = _playEmotionReactionAnimInTime + randomTimeInaccuracy;
        }

        private void PlayRandomEmotionReactionAnimation()
        {
            _isEmotionReactionPlaying = true;
            int randomAnimId = Random.Range(1, _emotionReactionAnimationsCount + 1);
            PlayEmotionReactionAnim(randomAnimId);
        }

        private void SetDefaultRandomIdle()
        {
            _defaultIdleNumber = Random.Range(1, _idleAnimationsCount + 1);
            ChangeIdleAnim(_defaultIdleNumber);
        }

        private void ChangeIdleAnim(int number)
        {
            _animator.Animator.SetInteger(_idleNumberHash, number);
            _idleAnimNumber = number;
        }

        private void PlayEmotionReactionAnim(int number)
        {
            _animator.Animator.SetInteger(_emotionReactionNumberHash, number);
            _animator.SetTrigger(_emotionReactionTriggerHash, true);
            _emotionReactionAnimNumber = number;
        }

        private void OnIdleAnimNumberChanged()
        {
            ChangeIdleAnim(_idleAnimNumber);
        }

        private void OnEmotionReactionAnimNumberChanged()
        {
            if (_emotionReactionAnimNumber == 0) return;
            
            PlayEmotionReactionAnim(_emotionReactionAnimNumber);
        }

        private void SetAnimHashes()
        {
            _idleNumberHash = Animator.StringToHash("IdleNumber");
            _emotionReactionNumberHash = Animator.StringToHash("ReactionNumber");
            _emotionReactionTriggerHash = Animator.StringToHash("ReactionTrigger");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null) TryGetComponent(out _animator);

            if (_animationsEvents == null) _animationsEvents = GetComponentInChildren<PodiumPlayerAnimationEvents>();
        }
#endif
    }
}
