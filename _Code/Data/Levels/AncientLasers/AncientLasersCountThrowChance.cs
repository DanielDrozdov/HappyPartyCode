using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Levels.AncientLasers
{
    [Serializable]
    public struct AncientLasersCountThrowChance
    {
        [field: SerializeField, Unit(Units.Percent), MinValue(0), MaxValue(100)]
        public int ChanceThrowLasers { get; private set; }
        
        [field: SerializeField,  MinValue(1), MaxValue(3)]
        public int ThrowLasersCount{ get; private set; }
    }
}
