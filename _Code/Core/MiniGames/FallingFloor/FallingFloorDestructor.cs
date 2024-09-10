using System.Collections.Generic;
using Core.Network;
using Core.Storages;
using Data.Levels.FallingFloorMiniGame;
using Fusion;
using UnityEngine;
using Odin = Sirenix.OdinInspector;
using UpdateSys;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.MiniGames.FallingFloor
{
    public class FallingFloorDestructor : MiniGameStarter, IUpdatable
    {
        [SerializeField, Unit(Units.Seconds), Odin.PropertySpace(15, 15)] 
        private float _timeToNextDestructionIteration;
        
        [SerializeField] 
        private List<FloorCellsCountToDestructionChance> _floorCellsCountToDestructionChances;
        
        [SerializeField, Odin.ReadOnly]
        private FallingFloorCell[] _floorCells;

        private List<FallingFloorCell> _activeFloorCells;
        private Dictionary<byte, FallingFloorCell> _floorCellsIdDict;
        private FallingFloorCellsCountToDestructionCalculator _fallingFloorCellsCountToDestructionCalculator;
        private IMiniGamePlayersStorage _playersStorage;
        private readonly int _blockedFloorCellsCountToDestruction = 2;
        private float _timeToNextDestructionIterationRemainder;

        [Inject]
        private void Construct(IMiniGamePlayersStorage playersStorage)
        {
            _playersStorage = playersStorage;
        }
        
        private void Awake()
        {
            _fallingFloorCellsCountToDestructionCalculator = 
                new FallingFloorCellsCountToDestructionCalculator(new List<FloorCellsCountToDestructionChance>(_floorCellsCountToDestructionChances));
            _activeFloorCells = new List<FallingFloorCell>(_floorCells);
            _timeToNextDestructionIterationRemainder = _timeToNextDestructionIteration;
            FillFloorCellsIdDict();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (!Runner.IsServer) return;
            
            this.StopUpdate();
        }

        public void OnSystemUpdate(float deltaTime)
        {
            if (_playersStorage.ActiveLivePlayers.Count == 0)
            {
                EndMiniGame();
                return;
            }
            
            _timeToNextDestructionIterationRemainder -= deltaTime;

            if (_timeToNextDestructionIterationRemainder <= 0)
            {
                DestructFloor();
            }
        }

        public override void StartMiniGame()
        {
            base.StartMiniGame();
            this.StartUpdate();
        }

        protected override void EndMiniGame()
        {
            base.EndMiniGame();
            this.StopUpdate();
        }

        private void DestructFloor()
        {
            _timeToNextDestructionIterationRemainder = _timeToNextDestructionIteration;
            float percentageOfDestructedFloorCells = (float)(_floorCells.Length - _activeFloorCells.Count) / _floorCells.Length * 100;
            int floorCellsCountToDestruction = _fallingFloorCellsCountToDestructionCalculator
                .GetCellsCountToDestruction(percentageOfDestructedFloorCells, _activeFloorCells.Count, _blockedFloorCellsCountToDestruction);

            if (floorCellsCountToDestruction == 0)
            {
                EndMiniGame();
                return;
            }
            
            byte[] floorCellsIdToDestruction = GetRandomFloorCellsIdToDestruction(floorCellsCountToDestruction);
            RPC_DestructFloorCells(floorCellsIdToDestruction);
        }

        private byte[] GetRandomFloorCellsIdToDestruction(int floorCellsCountToDestruction)
        {
            byte[] floorCellsToDestruction = new byte[floorCellsCountToDestruction];
            
            for (int i = 0; i < floorCellsCountToDestruction; i++)
            {
                FallingFloorCell floorCell = _activeFloorCells[Random.Range(0, _activeFloorCells.Count)];
                floorCellsToDestruction[i] = floorCell.Id;
                _activeFloorCells.Remove(floorCell);
            }

            return floorCellsToDestruction;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_DestructFloorCells(byte[] floorCellsIdToDestruction)
        {
            for (int i = 0; i < floorCellsIdToDestruction.Length; i++)
            {
                DestructFloorCell(floorCellsIdToDestruction[i]);
            }
        }

        private void DestructFloorCell(byte cellId)
        {
            FallingFloorCell floorCell = _floorCellsIdDict[cellId];
            _floorCellsIdDict.Remove(cellId);

            if (_activeFloorCells.Contains(floorCell))
            {
                _activeFloorCells.Remove(floorCell);
            }

            floorCell.StartDestructionProcess();
        }

        private void FillFloorCellsIdDict()
        {
            _floorCellsIdDict = new Dictionary<byte, FallingFloorCell>();

            foreach (FallingFloorCell floorCell in _floorCells)
            {
                _floorCellsIdDict.Add(floorCell.Id, floorCell);
            }
        }


#if UNITY_EDITOR
        public void SetNewFloorCells(FallingFloorCell[] floorCells)
        {
            _floorCells = floorCells;
        }
#endif
    }
}
