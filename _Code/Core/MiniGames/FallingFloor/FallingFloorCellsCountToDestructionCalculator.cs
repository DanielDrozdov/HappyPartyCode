using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Data.Levels.FallingFloorMiniGame;

namespace Core.MiniGames.FallingFloor
{
    public class FallingFloorCellsCountToDestructionCalculator
    {
        private ReadOnlyCollection<FloorCellsCountToDestructionChance> _floorCellsCountToDestructionChances;
        
        public FallingFloorCellsCountToDestructionCalculator(List<FloorCellsCountToDestructionChance> floorCellsCountToDestructionChances)
        {
            _floorCellsCountToDestructionChances = GetSortByPercentagesReadOnlyCollection(floorCellsCountToDestructionChances);
        }

        public int GetCellsCountToDestruction(float percentageOfDestructedFloorCells, int activeFloorCells, int blockedFloorCellsCountToDestruction)
        {
            int unblockedCellsCountToDestruction = activeFloorCells - blockedFloorCellsCountToDestruction;

            if (unblockedCellsCountToDestruction == 0)
            {
                return 0;
            }
            
            int floorCellsCountToDestruction = GetCellsCountToDestructionFromDataByPercentage(percentageOfDestructedFloorCells);
            
            floorCellsCountToDestruction = floorCellsCountToDestruction > unblockedCellsCountToDestruction ? 
                unblockedCellsCountToDestruction : floorCellsCountToDestruction;

            return floorCellsCountToDestruction;
        }

        private int GetCellsCountToDestructionFromDataByPercentage(float percentageOfDestructedFloorCells)
        {
            int floorCellsCountToDestruction = 0;
            
            for (int i = 0; i < _floorCellsCountToDestructionChances.Count; i++)
            {
                if (i == _floorCellsCountToDestructionChances.Count - 1)
                {
                    floorCellsCountToDestruction = _floorCellsCountToDestructionChances[i].DestructedFloorCellsInIteration;
                    break;
                }
                
                if (percentageOfDestructedFloorCells >= _floorCellsCountToDestructionChances[i].DestructedFloorCellsPercentageToBeValid &&
                    percentageOfDestructedFloorCells < _floorCellsCountToDestructionChances[i + 1].DestructedFloorCellsPercentageToBeValid)
                {
                    floorCellsCountToDestruction = _floorCellsCountToDestructionChances[i].DestructedFloorCellsInIteration;
                    break;
                }
            }

            return floorCellsCountToDestruction;
        }

        private ReadOnlyCollection<FloorCellsCountToDestructionChance> GetSortByPercentagesReadOnlyCollection(List<FloorCellsCountToDestructionChance> floorCellsCountToDestructionChances)
        {
            List<FloorCellsCountToDestructionChance> sortedList = floorCellsCountToDestructionChances
                .OrderBy(x => x.DestructedFloorCellsPercentageToBeValid)
                .ToList();

            return new ReadOnlyCollection<FloorCellsCountToDestructionChance>(sortedList);
        }
    }
}