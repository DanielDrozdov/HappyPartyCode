using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Levels.AncientLasers
{
    [CreateAssetMenu(menuName = "Data/Levels/MiniGames/Ancient Lasers/Create Ancient Lasers Arena Settings", fileName = "AncientLasersArenaSettings")]
    public class AncientLasersArenaSettings : ScriptableObject
    {
        [field: SerializeField, Title("Lasers Arena Settings"), Unit(Units.Second)]
        public float MiniGamePlayingTime { get; private set; }
        
        [field: SerializeField, Title("Lasers Settings"), Unit(Units.Second)] 
        public float SpawnTimeDelayBetweenMultiplyLasers { get; private set; }
        
        [field: SerializeField, Unit(Units.MetersPerSecond)] 
        public float LaserSpeed { get; private set; }
        
        [field: SerializeField, Title("Arena Border Settings"), Unit(Units.Second)] 
        public float BorderLasersSpawnWarnDuration { get; private set; }
        
        [field: SerializeField] 
        public Color BorderWarnColor { get; private set; }
    }
}
