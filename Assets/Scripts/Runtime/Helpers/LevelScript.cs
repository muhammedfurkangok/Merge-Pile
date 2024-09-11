using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Entities;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Helpers
{
    [ExecuteInEditMode]
    public class LevelCreatorScript : MonoBehaviour
    {
        [Header("Grid Settings")]
        [Range(0f, 100f)]
        public float _spaceModifier = 50f;
        [Range(50f, 100f)]
        public float _gridSize = 50f;
        
        [Header("References")]
        public CD_LevelData LevelData;
        public CD_GameColor colorData;
        public CD_ItemData itemData;
        public GameObject itemsParentObject;
        public GameColors gridColor;

        private LevelData _currentLevelData;

        public void GenerateLevelData()
        {
            if (itemsParentObject != null)
            {
                DestroyImmediate(itemsParentObject);
            }

            itemsParentObject = new GameObject(this.gameObject.name + "_Items");
            itemsParentObject.transform.SetParent(gameObject.transform);
            itemsParentObject.transform.rotation = Quaternion.Euler(0, 0, 90);

            for (int x = 0; x < LevelData.levelData.Width; x++)
            {
                for (int y = 0; y < LevelData.levelData.Height; y++)
                {
                    if (LevelData.levelData.GetGrid(x, y).isOccupied && LevelData.levelData.GetGrid(x, y).gameColor != GameColors.None)
                    {
                        GameObject item = Instantiate(itemData.itemData[1].itemPrefab, GridSpaceToWorldSpace(x, y), Quaternion.identity, itemsParentObject.transform);
                    }
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
            if (LevelData == null || LevelData.levelData == null)
            {
                Debug.LogError("LevelData is not assigned or initialized.");
                return;
            }

            if (LevelData.levelData.Width <= 0 || LevelData.levelData.Height <= 0)
            {
                Debug.LogError("Invalid level dimensions.");
                return;
            }

            if (_currentLevelData == null)
            {
                _currentLevelData = new LevelData
                {
                    Width = LevelData.levelData.Width,
                    Height = LevelData.levelData.Height,
                    Grids = new GridData[LevelData.levelData.Width * LevelData.levelData.Height]
                };
            }

            if (LevelData.levelData.Grids == null || LevelData.levelData.Grids.Length != _currentLevelData.Grids.Length)
            {
                LevelData.levelData.Grids = new GridData[_currentLevelData.Width * _currentLevelData.Height];
            }

            for (int i = 0; i < LevelData.levelData.Grids.Length; i++)
            {
                _currentLevelData.Grids[i] = new GridData
                {
                    isOccupied = LevelData.levelData.Grids[i].isOccupied,
                    gameColor = LevelData.levelData.Grids[i].gameColor,
                    position = LevelData.levelData.Grids[i].position
                };
            }

            _currentLevelData.Width = LevelData.levelData.Width;
            _currentLevelData.Height = LevelData.levelData.Height;
        }

        public void ToggleGridOccupancy(int x, int y)
        {
            var grid = _currentLevelData.GetGrid(x, y);
            grid.isOccupied = !grid.isOccupied;
            _currentLevelData.SetGrid(x, y, grid);
        }

        public LevelData GetCurrentLevelData()
        {
            return _currentLevelData;
        }

        public int GetRows()
        {
            return LevelData.levelData.Width;
        }

        public int GetColumns()
        {
            return LevelData.levelData.Height;
        }

        public void SaveLevelData()
        {
            LevelData.levelData.Grids = new GridData[_currentLevelData.Grids.Length];

            for (int i = 0; i < _currentLevelData.Grids.Length; i++)
            {
                LevelData.levelData.Grids[i] = new GridData
                {
                    isOccupied = _currentLevelData.Grids[i].isOccupied,
                    gameColor = _currentLevelData.Grids[i].gameColor,
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
            for (int x = 0; x < LevelData.levelData.Width; x++)
            {
                for (int y = 0; y < LevelData.levelData.Height; y++)
                {
                    // ScriptableObject
                    LevelData.levelData.SetGrid(x, y, new GridData
                    {
                        isOccupied = false,
                        gameColor = GameColors.None,
                        position = new Vector2Int(x, y)
                    });
                    // Editor
                    _currentLevelData.SetGrid(x, y, new GridData
                    {
                        isOccupied = false,
                        gameColor = GameColors.None,
                        position = new Vector2Int(x, y)
                    });
                }
            }

            Debug.Log("Grid reset.");
        }

        public Color GetGridColor(Vector2Int position)
        {
            var grid = _currentLevelData.GetGrid(position.x, position.y);
            return grid.isOccupied ? colorData.gameColorsData[(int)grid.gameColor].color : Color.white;
        }

        public Color GetSelectedGridColor()
        {
            foreach (var data in colorData.gameColorsData)
            {
                if (data.gameColor == gridColor)
                {
                    return data.color;
                }
            }
            return Color.white;
        }

        public void SetGridColor(int x, int y)
        {
            var grid = _currentLevelData.GetGrid(x, y);
            grid.gameColor = gridColor;
            _currentLevelData.SetGrid(x, y, grid);
        }
    }
}
