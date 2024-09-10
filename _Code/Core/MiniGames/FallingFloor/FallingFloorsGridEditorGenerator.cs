using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Core.MiniGames.FallingFloor
{
    public class FallingFloorsGridEditorGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, MinValue(1), Title("Grid size")] 
        private int _columnsCount;

        [SerializeField, MinValue(1)] 
        private int _rowsCount;
        
        [SerializeField, MinValue(0), Title("Cells size")] 
        private float _xFloorCellSize;

        [SerializeField, MinValue(0)] 
        private float _yFloorCellSize;
        
        [SerializeField, MinValue(0)] 
        private float _zFloorCellSize;
        
        [SerializeField, MinValue(0), Title("Cells offsets")] 
        private float _xOffsetBetweenCells;

        [SerializeField, MinValue(0)] 
        private float _zOffsetBetweenCells;
        
        
        [SerializeField, Title("Floor cell prefab")] 
        private GameObject _floorCellPrefab;

        [SerializeField, Title("Components")] 
        private FallingFloorDestructor _floorDestructor;
        
        [Button("Generate floors grid")]
        private void GenerateFloorsGrid()
        {
            DeleteOldGrid();

            for (int column = 1; column <= _columnsCount; column++)
            {
                for (int row = 1; row <= _rowsCount; row++)
                {
                    CreateFloorCell(column, row);
                }
            }
            
            _floorDestructor.SetNewFloorCells(GetComponentsInChildren<FallingFloorCell>());
        }

        private void CreateFloorCell(int column, int row)
        {
            Vector3 floorCellPos = GetFloorCellPosition(column, row);
            Vector3 floorSize = new Vector3(_xFloorCellSize, _yFloorCellSize, _zFloorCellSize);

            GameObject floorCell = PrefabUtility.InstantiatePrefab(_floorCellPrefab, transform) as GameObject;
            int floorCellId = (column - 1) * _rowsCount + row;

            FallingFloorCell fallingFloorCell = floorCell.GetComponent<FallingFloorCell>();
            fallingFloorCell.SetId(floorCellId);
            fallingFloorCell.SetSize(floorSize);
            floorCell.name = "Cell " + column + "_" + row;
            floorCell.transform.localPosition = floorCellPos;
        }

        private Vector3 GetFloorCellPosition(int column, int row)
        {
            float totalXOffset = (row - 1) * _xOffsetBetweenCells;
            float totalZOffset = (column - 1) * _zOffsetBetweenCells;
            float cellsSizeX = (row - 1) * _xFloorCellSize + _xFloorCellSize / 2;
            float cellsSizeZ = (column - 1) * _zFloorCellSize + _zFloorCellSize / 2;

            return new Vector3(cellsSizeX + totalXOffset, 0, cellsSizeZ + totalZOffset);
        }

        private void DeleteOldGrid()
        {
            FallingFloorCell[] cells = GetComponentsInChildren<FallingFloorCell>();

            for (int i = 0; i < cells.Length; i++)
            {
                DestroyImmediate(cells[i].gameObject);
            }
        }

        private void OnValidate()
        {
            if (_floorDestructor == null) TryGetComponent(out _floorDestructor);
        }
#endif
    }
}
