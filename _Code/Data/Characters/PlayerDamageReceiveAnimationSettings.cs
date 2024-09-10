using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Character
{
    [CreateAssetMenu(menuName = "Data/Characters/Create Player Damage Receive Animation Settings", fileName = "PlayerDamageReceiveAnimationSettings")]
    public class PlayerDamageReceiveAnimationSettings : ScriptableObject
    {
        [field: SerializeField, Unit(Units.Second)]
        public float BlinkDurationTime { get; private set; }
        
        [field: SerializeField]
        public Color BlinkColor { get; private set; }
    }
}
