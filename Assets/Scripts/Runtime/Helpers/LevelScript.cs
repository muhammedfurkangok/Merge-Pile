// using Runtime.Data.UnityObject;
// using Runtime.Data.ValueObject;
// using Runtime.Entities;
// using Runtime.Enums;
// using Runtime.Managers;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Runtime.Helpers
// {
//     [ExecuteInEditMode]
//     public class LevelCreatorScript : MonoBehaviour
//     {
//         [Header("Grid Settings")]
//         public int Width;
//         public int Height;
//         [Range( 0f, 100f)]
//         public float _spaceModifier = 50f;
//         [Range( 50f, 100f)]
//         public float _gridSize = 50f;
//         
//         
//         [Header("References")]
//         public CD_LevelData LevelData;
//         public CD_GameColor colorData;
//         public CD_GamePrefab itemPrefab;
//         public GameObject itemsParentObject;
//         public GridManager gridManager;
//
//         public GameColors gameColor;
//         
//        
//        
//         private LevelData _currentLevelData;
//        
//        
//
//         private void OnEnable()
//         {
//             if (LevelData == null)
//             {
//                 Debug.LogError("LevelData is not assigned in the inspector!");
//                 return;
//             }
//
//             if (LevelData.levelData == null)
//             {
//                 LevelData.levelData = new LevelData();
//             }
//             
//             if (LevelData != null)
//             {
//                 SetCurrentLevelData();
//             }
//         }
//
//         public void GenerateLevelData()
//         {
//             gridManager.Initialize(Width, Height, _spaceModifier);
//             
//             // Cleanup before generating new level
//             if (itemsParentObject != null)
//             {
//                 DestroyImmediate(itemsParentObject);
//                 gridManager.ClearItems();
//             }
//             
//             itemsParentObject = new GameObject("LevelParent"); // Create new parent object
//
//
//             for (int x = 0; x < Width; x++)
//             {
//                 for (int y = 0; y < Height; y++)
//                 {
//                     if(LevelData.levelData.GetGrid(x,y).isOccupied && LevelData.levelData.GetGrid(x,y).gameColor != GameColor.None)
//                     {
//                         GameObject item = Instantiate(itemPrefab.gamePrefab.prefab.gameObject, GridSpaceToWorldSpace(x, y), Quaternion.identity, itemsParentObject.transform);
//                         item.GetComponent<Item>().Init(new Vector2Int(x, y), LevelData.levelData.GetGrid(x,y).gameColor, gridManager);
//                     }
//                 }
//             }
//             Debug.Log("Grid generated.");
//         }
//
//         
//         public Vector3 GridSpaceToWorldSpace(int x, int y)
//         {
//             return new Vector3(x * _spaceModifier, 0, y * _spaceModifier);
//         }
//
//         public Vector2Int WorldSpaceToGridSpace(Vector3 worldPosition)
//         {
//             int x = Mathf.RoundToInt(worldPosition.x / _spaceModifier);
//             int y = Mathf.RoundToInt(worldPosition.z / _spaceModifier);
//             return new Vector2Int(x, y);
//         }
//
//         private void SetCurrentLevelData()
//         {
//             if (LevelData.levelData.Grids == null || LevelData.levelData.Grids.Length != _currentLevelData.Grids.Length)
//             {
//                 LevelData.levelData.Grids = new GridData[_currentLevelData.Grids.Length];
//             }
//             
//             _currentLevelData = new LevelData
//             {
//                 Width = LevelData.levelData.Width,
//                 Height = LevelData.levelData.Height,
//                 Grids = new GridData[LevelData.levelData.Grids.Length]
//             };
//
//             for (int i = 0; i < LevelData.levelData.Grids.Length; i++)
//             {
//                 _currentLevelData.Grids[i] = new GridData
//                 {
//                     isOccupied = LevelData.levelData.Grids[i].isOccupied,
//                     gameColor = LevelData.levelData.Grids[i].gameColor,
//                     position = LevelData.levelData.Grids[i].position
//                 };
//             }
//
//             Width = _currentLevelData.Height;
//             Height = _currentLevelData.Width;
//         }
//
//         public void ToggleGridOccupancy(int x, int y)
//         {
//             var grid = _currentLevelData.GetGrid(x, y);
//             grid.isOccupied = !grid.isOccupied;
//             _currentLevelData.SetGrid(x, y, grid);
//         }
//
//        
//
//         public LevelData GetCurrentLevelData()
//         {
//             return _currentLevelData;
//         }
//
//         public int GetRows()
//         {
//             return Width;
//         }
//
//         public int GetColumns()
//         {
//             return Height;
//         }
//
//   
//
//         public void SaveLevelData()
//         {
//             if (LevelData.levelData.Grids == null || LevelData.levelData.Grids.Length != _currentLevelData.Grids.Length)
//             {
//                 LevelData.levelData.Grids = new GridData[_currentLevelData.Grids.Length];
//             }
//                 
//             for (int i = 0; i < _currentLevelData.Grids.Length; i++)
//             {
//                 LevelData.levelData.Grids[i] = new GridData
//                 {
//                     isOccupied = _currentLevelData.Grids[i].isOccupied,
//                     gameColor = _currentLevelData.Grids[i].gameColor,
//                     position = _currentLevelData.Grids[i].position
//                 };
//             }
//
//             LevelData.levelData.Width = _currentLevelData.Width;
//             LevelData.levelData.Height = _currentLevelData.Height;
//             Debug.Log("Level data saved.");
//         }
//
//         public void LoadLevelData()
//         {
//             SetCurrentLevelData();
//             Debug.Log("Level data loaded.");
//         }
//
//         public void ResetGridData()
//         {
//             for (int x = 0; x < Width; x++)
//             {
//                 for (int y = 0; y < Height; y++)
//                 {
//                     //ScriptableObject
//                      LevelData.levelData.SetGrid(x, y, new GridData
//                      {
//                          isOccupied = false,
//                          gameColor = GameColor.None,
//                          position = new Vector2Int(x, y)
//                      });
//                     //Editor
//                     _currentLevelData.SetGrid(x, y, new GridData
//                     {
//                         isOccupied = false,
//                          gameColor = GameColor.None,
//                         position = new Vector2Int(x, y)
//                     });
//                 }
//             }
//
//             Debug.Log("Grid reset.");
//         }
//
//         public Color GetGridColor(Vector2Int position)
//        {
//             var grid = _currentLevelData.GetGrid(position.x, position.y);
//             return grid.isOccupied ? colorData.gameColorsData[(int)grid.gameColor].color : Color.white;
//         }
//         public Color GetSelectedGridColor()
//         {
//             foreach (var data in colorData.gameColorsData)
//             {
//                 if (data.gameColor == gameColor)
//                 {
//                     return data.color;
//                 }
//             }
//            
//             return Color.white;
//         }
//         
//         public void SetGridColor(int x, int y)
//         {
//             var grid = _currentLevelData.GetGrid(x, y);
//             grid.gameColor = gameColor;
//             _currentLevelData.SetGrid(x, y, grid);
//         }
//     }
// }