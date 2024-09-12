using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Runtime.Helpers
{
    [ExecuteInEditMode]
    public class LevelCreatorScript : MonoBehaviour
    {
        
        [Header("Grid Settings")]
        [Range(0f, 100f)]
        public float _XspaceModifier = 0.75f;
        [Range(0f, 100f)]
        public float _YspaceModifier = 0.75f;
        [Range(50f, 100f)]
        public float _gridSize = 50f;
        
        
        public CD_LevelData levelData;
        public CD_GameColor itemTypes;
        public CD_ItemData itemData;
        public GameObject itemsParentObject;
        public ItemTypes itemDataColor;
        
        public Vector3 itemPosition;

        private LevelData _currentLevelData;

        public void GenerateLevelData()
        {
            if (itemsParentObject != null)
            {
                DestroyImmediate(itemsParentObject);
            }

            itemsParentObject = new GameObject(this.gameObject.name + "_Items");
            itemsParentObject.transform.position = itemPosition;
            itemsParentObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            for (int x = 0; x < levelData.levelData.Width; x++)
            {
                for (int y = 0; y < levelData.levelData.Height; y++)
                {
                    if (levelData.levelData.GetGrid(x, y).isOccupied && levelData.levelData.GetGrid(x, y).ıtemType != ItemTypes.None)
                    {
                        
                        var randomQuaternion = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                        var GetItemPrefab = itemData.itemData[(int)levelData.levelData.GetGrid(x, y).ıtemType - 1].itemPrefab;
                        #if UNITY_EDITOR
                        GameObject item = PrefabUtility.InstantiatePrefab(GetItemPrefab) as GameObject;
                        item.transform.position = GridSpaceToWorldSpace(x, y) + itemPosition;
                        item.transform.rotation = randomQuaternion;
                        item.transform.SetParent(itemsParentObject.transform);
                        #endif
                    }
                }
            }
            LoadLevelData();
            Debug.Log("Grid generated.");
        }
        

        public Vector3 GridSpaceToWorldSpace(int x, int y)
        {
            return new Vector3(x * _XspaceModifier, y * _YspaceModifier, 0 );
        }

        public Vector2Int WorldSpaceToGridSpace(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / _XspaceModifier);
            int y = Mathf.RoundToInt(worldPosition.y / _YspaceModifier);
            return new Vector2Int(x, y);
        }

        private void SetCurrentLevelData()
        {
            if (levelData == null || levelData.levelData == null)
            {
                Debug.LogError("LevelData is not assigned or initialized.");
                return;
            }

            if (levelData.levelData.Width <= 0 || levelData.levelData.Height <= 0)
            {
                Debug.LogError("Invalid level dimensions.");
                return;
            }

            if (_currentLevelData == null)
            {
                _currentLevelData = new LevelData
                {
                    Width = levelData.levelData.Width,
                    Height = levelData.levelData.Height,
                    Grids = new GridData[levelData.levelData.Width * levelData.levelData.Height]
                };
            }

            if (levelData.levelData.Grids == null || levelData.levelData.Grids.Length != _currentLevelData.Grids.Length)
            {
                levelData.levelData.Grids = new GridData[_currentLevelData.Width * _currentLevelData.Height];
            }

            for (int i = 0; i < levelData.levelData.Grids.Length; i++)
            {
                _currentLevelData.Grids[i] = new GridData
                {
                    isOccupied = levelData.levelData.Grids[i].isOccupied,
                    ıtemType = levelData.levelData.Grids[i].ıtemType,
                    position = levelData.levelData.Grids[i].position
                };
            }

            _currentLevelData.Width = levelData.levelData.Width;
            _currentLevelData.Height = levelData.levelData.Height;
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
            return levelData.levelData.Height;
        }

        public int GetColumns()
        {
            return levelData.levelData.Width;
        }

        public void SaveLevelData()
        {
            levelData.levelData.Grids = new GridData[_currentLevelData.Grids.Length];

            for (int i = 0; i < _currentLevelData.Grids.Length; i++)
            {
                levelData.levelData.Grids[i] = new GridData
                {
                    isOccupied = _currentLevelData.Grids[i].isOccupied,
                    ıtemType = _currentLevelData.Grids[i].ıtemType,
                    position = _currentLevelData.Grids[i].position
                };
            }

            levelData.levelData.Width = _currentLevelData.Width;
            levelData.levelData.Height = _currentLevelData.Height;
            Debug.Log("Level data saved.");
        }

        public void LoadLevelData()
        {
            SetCurrentLevelData();
            Debug.Log("Level data loaded.");
        }

        public void ResetGridData()
        {
            for (int x = 0; x < levelData.levelData.Width; x++)
            {
                for (int y = 0; y < levelData.levelData.Height; y++)
                {
                    // ScriptableObject
                    levelData.levelData.SetGrid(x, y, new GridData
                    {
                        isOccupied = false,
                        ıtemType = ItemTypes.None,
                        position = new Vector2Int(x, y)
                    });
                    // Editor
                    _currentLevelData.SetGrid(x, y, new GridData
                    {
                        isOccupied = false,
                        ıtemType = ItemTypes.None,
                        position = new Vector2Int(x, y)
                    });
                }
            }

            Debug.Log("Grid reset.");
        }

        public Color GetGridColor(Vector2Int position)
        {
            var grid = _currentLevelData.GetGrid(position.x, position.y);
            return grid.isOccupied ? itemTypes.gameColorsData[(int)grid.ıtemType].color : Color.white;
        }
        
        

        public void SetGridColor(int x, int y)
        {
            var grid = _currentLevelData.GetGrid(x, y);
            grid.ıtemType = itemDataColor;
            _currentLevelData.SetGrid(x, y, grid);
        }
    }
}
