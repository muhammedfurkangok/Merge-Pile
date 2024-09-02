using Runtime.Helpers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelCreatorScript))]
    public class LevelCreatorEditor : UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            LevelCreatorScript levelCreatorScript = (LevelCreatorScript)target;

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16,
                alignment = TextAnchor.UpperLeft
            };
            
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Grid"))
            {
                levelCreatorScript.GenerateLevelData();
            }

            EditorGUILayout.LabelField("Save/Load/Reset Grid", titleStyle);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Grid"))
            {
                levelCreatorScript.SaveLevelData();
            }

            if (GUILayout.Button("Load Grid"))
            {
                levelCreatorScript.LoadLevelData();
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Reset Grid"))
            {
                levelCreatorScript.ResetGridData();
            }

            EditorGUILayout.LabelField("Grid", titleStyle);

            if (levelCreatorScript.GetCurrentLevelData() != null && levelCreatorScript.GetCurrentLevelData().Grids != null)
            {
                DrawGrid(levelCreatorScript);
            }
        }

        private void DrawGrid(LevelCreatorScript levelCreatorScript)
        {
            int rows = levelCreatorScript.GetRows();
            int columns = levelCreatorScript.GetColumns();

            for (int x = 0; x < rows; x++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                for (int y = 0; y < columns; y++)
                {
                    Color originalColor = GUI.backgroundColor;
                    GUI.backgroundColor = levelCreatorScript.GetCurrentLevelData().GetGrid(x, y).isOccupied ? Color.green : Color.gray;

                    if (GUILayout.Button($"{y}x{rows - 1 - x}", GUILayout.Width(levelCreatorScript._gridSize), GUILayout.Height(levelCreatorScript._gridSize)))
                    {
                        levelCreatorScript.ToggleGridOccupancy(x, y);
                        levelCreatorScript.SetGridColor(x, y, levelCreatorScript.GetSelectedGridColor());
                    }

                    GUI.backgroundColor = originalColor;
                   GUILayout.Space(5);
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
