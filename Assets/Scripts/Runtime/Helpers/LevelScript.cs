using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Helpers
{
    [ExecuteInEditMode]
    public class LevelCreatorScript : MonoBehaviour
    {
        [Header("Grid Settings")]
        [Range( 50f, 100f)]
        public float _spaceModifier = 50f;
        [Range( 50f, 100f)]
        public float _gridSize = 50f;
        
        [Header("References")]
        public CD_LevelData LevelData;
        public CD_GameColor colorData;

        [Header("Level Data")]
        public GameColors gameColor;

        private LevelData _currentLevelData;
        private int _rows;
        private int _columns;
       

        private void OnEnable()
        {
            if (LevelData != null)
            {
                SetCurrentLevelData();
            }
        }

        public void GenerateLevelData()
        {
            _columns = LevelData.levelData.Width;
            _rows = LevelData.levelData.Height;
            _currentLevelData = new LevelData
            {
                Width = _columns,
                Height = _rows,
                Grids = new GridData[_rows * _columns]
            };

            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    _currentLevelData.Grids[x * _columns + y] = new GridData
                    {
                        isOccupied = false,
                        position = new Vector2Int(x, y)
                    };
                }
            }

            Debug.Log("Grid generated.");
        }
        
        public Vector3 GridSpaceToWorldSpace(int x, int y)
        {
            return new Vector3(x * _spaceModifier, 0, y * _spaceModifier);
        }

        public Vector2Int WorldSpaceToGridSpace(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / _spaceModifier);
            int y = Mathf.RoundToInt(worldPosition.z / _spaceModifier);
            return new Vector2Int(x, y);
        }

        private void SetCurrentLevelData()
        {
            _currentLevelData = new LevelData
            {
                Width = LevelData.levelData.Width,
                Height = LevelData.levelData.Height,
                Grids = new GridData[LevelData.levelData.Grids.Length]
            };

            for (int i = 0; i < LevelData.levelData.Grids.Length; i++)
            {
                _currentLevelData.Grids[i] = new GridData
                {
                    isOccupied = LevelData.levelData.Grids[i].isOccupied,
                    gridColor = LevelData.levelData.Grids[i].gridColor,
                    position = LevelData.levelData.Grids[i].position
                };
            }

            _rows = _currentLevelData.Height;
            _columns = _currentLevelData.Width;
        }

        public void ToggleGridOccupancy(int x, int y)
        {
            var grid = _currentLevelData.GetGrid(x, y);
            grid.isOccupied = !grid.isOccupied;
            _currentLevelData.SetGrid(x, y, grid);
        }

        public void SetGridColor(int x, int y, Color color)
        {
            var grid = _currentLevelData.GetGrid(x, y);
            grid.gridColor = color;
            _currentLevelData.SetGrid(x, y, grid);
        }

        public LevelData GetCurrentLevelData()
        {
            return _currentLevelData;
        }

        public int GetRows()
        {
            return _rows;
        }

        public int GetColumns()
        {
            return _columns;
        }

        public Color GetSelectedGridColor()
        {
            foreach (var data in colorData.gameColorsData)
            {
                if (data.gameColor == gameColor)
                {
                    return data.color;
                }
            }
           
            return Color.white;
        }

        public void SaveLevelData()
        {
            if (LevelData.levelData.Grids == null || LevelData.levelData.Grids.Length != _currentLevelData.Grids.Length)
            {
                LevelData.levelData.Grids = new GridData[_currentLevelData.Grids.Length];
            }
                
            for (int i = 0; i < _currentLevelData.Grids.Length; i++)
            {
                LevelData.levelData.Grids[i] = new GridData
                {
                    isOccupied = _currentLevelData.Grids[i].isOccupied,
                    gridColor = _currentLevelData.Grids[i].gridColor,
                    position = _currentLevelData.Grids[i].position
                };
            }

            LevelData.levelData.Width = _currentLevelData.Width;
            LevelData.levelData.Height = _currentLevelData.Height;
            Debug.Log("Level data saved.");
        }

        public void LoadLevelData()
        {
            SetCurrentLevelData();
            Debug.Log("Level data loaded.");
        }

        public void ResetGridData()
        {
            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    //ScriptableObject
                     LevelData.levelData.SetGrid(x, y, new GridData
                     {
                         isOccupied = false,
                         gridColor = Color.white,
                         position = new Vector2Int(x, y)
                     });
                    //Editor
                    _currentLevelData.SetGrid(x, y, new GridData
                    {
                        isOccupied = false,
                        gridColor = Color.white,
                        position = new Vector2Int(x, y)
                    });
                }
            }

            Debug.Log("Grid reset.");
        }
    }
}
