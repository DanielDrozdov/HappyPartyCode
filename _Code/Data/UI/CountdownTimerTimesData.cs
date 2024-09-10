using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.UI
{
    [CreateAssetMenu(menuName = "Data/Another/Create Countdown timer times data", fileName = "CountdownTimerTimesData")]
    public class CountdownTimerTimesData : ScriptableObject
    {
        [field: SerializeField, Unit(Units.Second), MinValue(0)]
        public float TimeToStartNextMiniGameFromLobby { get; private set; }
        
        [field: SerializeField, Unit(Units.Second), MinValue(0)]
        public float TimeToStartMiniGame { get; private set; }
        
        [field: SerializeField, Unit(Units.Second), MinValue(0)]
        public float TimeToReturnLobbyAfterMiniGameResultsDisplayed { get; private set; }
        
        [field: SerializeField, Unit(Units.Second), MinValue(0)]
        public float TimeToReturnMainMenuAfterTotalResultsDisplayed { get; private set; }
    }
}
