using System.Collections.Generic;
using System.Linq;
using Data.Levels.AncientLasers;
using UnityEngine;

namespace Core.MiniGames.AncientLasers.Arena
{
    public class AncientLasersThrowCountRandomizer
    {
        private List<AncientLasersCountThrowChance> _ancientLasersCountThrowChances;
        
        public AncientLasersThrowCountRandomizer(AncientLasersCountThrowChance[] ancientLasersCountThrowChances)
        {
            SortArrayByChances(ancientLasersCountThrowChances);
        }

        public int GetRandomLasersCountToThrow()
        {
            int randomPercentage = Random.Range(0, 100);

            for (int i = 0; i < _ancientLasersCountThrowChances.Count; i++)
            {
                AncientLasersCountThrowChance ancientLasersCountThrowChance = _ancientLasersCountThrowChances[i];
                int previousChance = i == 0 ? 0 : _ancientLasersCountThrowChances[i - 1].ChanceThrowLasers;

                if (randomPercentage > previousChance && randomPercentage <= ancientLasersCountThrowChance.ChanceThrowLasers)
                {
                    return ancientLasersCountThrowChance.ThrowLasersCount;
                }
            }
            
            return 0;
        }
        
        private void SortArrayByChances(AncientLasersCountThrowChance[] ancientLasersCountThrowChances)
        {
            _ancientLasersCountThrowChances = ancientLasersCountThrowChances
                .OrderBy(x => x.ChanceThrowLasers)
                .ToList();
        }
    }
}