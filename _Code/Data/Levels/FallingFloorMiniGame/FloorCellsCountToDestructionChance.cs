using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Levels.FallingFloorMiniGame
{
    [Serializable]
    public class FloorCellsCountToDestructionChance
    {
        [field: SerializeField, MinValue(1), MaxValue(5)] 
        public int DestructedFloorCellsInIteration { get; private set; }
        
        [field: SerializeField, MinValue(0), MaxValue(100), Unit(Units.Percent)] 
        public int DestructedFloorCellsPercentageToBeValid { get; private set; }
    }
}
