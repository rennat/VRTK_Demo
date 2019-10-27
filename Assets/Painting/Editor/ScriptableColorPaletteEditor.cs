using UnityEditor;
using UnityEngine;
#if UNITY_WINRT
using File = UnityEngine.Windows.File;
#else
using File = System.IO.File;
#endif

namespace Tannern
{
    [CustomEditor(typeof(ScriptableColorPalette))]
    public class ScriptableColorPaletteEditor : Editor
    {
        SerializedProperty gridSize;
        SerializedProperty emptyColor;
        SerializedProperty colors;
        SerializedProperty textureSize;
        bool _optionsOpen = false;

        private void OnEnable()
        {
            gridSize = serializedObject.FindProperty("gridSize");
            emptyColor = serializedObject.FindProperty("emptyColor");
            colors = serializedObject.FindProperty("colors");
            textureSize = serializedObject.FindProperty("textureSize");
        }

        public override void OnInspectorGUI()
        {
            float usableWidth = Screen.width - 64f;
            serializedObject.Update();
            int oldGridSize = (int)Mathf.Sqrt(colors.arraySize);
            int targetCount = gridSize.intValue * gridSize.intValue;
            int i;
            string assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
            Texture2D tex = null;

            if (oldGridSize != gridSize.intValue)
            {
                int oldArraySize = colors.arraySize;
                int gridSizeDelta = gridSize.intValue - oldGridSize;
                int arrayLengthDelta = targetCount - colors.arraySize;
                if (gridSizeDelta > 0)
                {
                    for (i = 0; i < targetCount; i++)
                    {
                        int y = i / gridSize.intValue;
                        int x = i % gridSize.intValue;
                        if (y > oldGridSize - 1 || x > oldGridSize - 1)
                        {
                            colors.InsertArrayElementAtIndex(i);
                            colors.GetArrayElementAtIndex(i).colorValue = emptyColor.colorValue;
                        }
                    }
                }
                else if (arrayLengthDelta < 0)
                {
                    for (i = colors.arraySize - 1; i > 0; i--)
                    {
                        int y = i / oldGridSize;
                        int x = i % oldGridSize;
                        if (y > gridSize.intValue - 1 || x > gridSize.intValue - 1)
                        {
                            colors.DeleteArrayElementAtIndex(i);
                        }
                    }
                }
            }

            float cellSize = Mathf.Min(512f, usableWidth) / gridSize.intValue;
            i = 0;
            for (int y = 0; y < gridSize.intValue; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < gridSize.intValue; x++)
                {
                    SerializedProperty p = colors.GetArrayElementAtIndex(i);
                    p.colorValue = EditorGUILayout.ColorField(
                        GUIContent.none,
                        p.colorValue,
                        false,
                        true,
                        false,
                        GUILayout.Width(cellSize),
                        GUILayout.Height(cellSize)
                    );
                    i += 1;
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(12f);

            _optionsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_optionsOpen, "Options");
            if (_optionsOpen)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(string.Format("Grid Size: {0} ({1} cells)",
                    gridSize.intValue,
                    colors.arraySize));
                GUI.enabled = gridSize.intValue > 1;
                if (GUILayout.Button("-")) gridSize.intValue -= 1;
                GUI.enabled = gridSize.intValue < 12;  // an arbitrary choice because of the extra padding between cells
                if (GUILayout.Button("+")) gridSize.intValue += 1;
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(emptyColor);
                EditorGUILayout.PropertyField(textureSize);

                GUILayout.Space(24f);
                if (GUILayout.Button("Clear Colors"))
                {
                    colors.ClearArray();
                    for (i = 0; i < targetCount; i++)
                    {
                        colors.InsertArrayElementAtIndex(i);
                        colors.GetArrayElementAtIndex(i).colorValue = emptyColor.colorValue;
                    }
                }

                GUILayout.Space(12f);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (tex == null)
                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            GUILayout.Space(12f);
            if (GUILayout.Button(tex == null ? "Create Texture Asset" : "Update Texture Asset"))
            {
                if (assetPath != null)
                {
                    if (colors.arraySize == 0)
                    {
                        for (i = 0; i < targetCount; i++)
                        {
                            colors.InsertArrayElementAtIndex(i);
                            colors.GetArrayElementAtIndex(i).colorValue = emptyColor.colorValue;
                        }
                    }

                    if (tex == null)
                    {
                        tex = new Texture2D(textureSize.intValue, textureSize.intValue);
                        tex.filterMode = FilterMode.Point;
                        AssetDatabase.AddObjectToAsset(tex, assetPath);
                    }
                    tex.name = serializedObject.targetObject.name + " Texture";
                    ((ScriptableColorPalette)serializedObject.targetObject).ApplyToTexture2D(tex);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.ImportAsset(assetPath);
                }
            }

            serializedObject.ApplyModifiedProperties();

            if (tex != null)
            {
                GUILayout.Space(12f);
                GUILayout.BeginVertical(GUILayout.Width(usableWidth));
                GUILayout.Space(usableWidth);
                GUILayout.EndVertical();
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), tex);

                GUILayout.Space(12f);
                GUILayout.Label(tex.name);
                GUILayout.Label(string.Format("{0}px by {1}px", tex.width, tex.height));

                GUILayout.Space(12f);
                if (GUILayout.Button("Show in Project"))
                    EditorGUIUtility.PingObject(tex);

                if (GUILayout.Button("Export PNG")) {
                    string savePath = EditorUtility.SaveFilePanel(
                        string.Format("Export {0} as PNG", tex.name),
                        "",
                        string.Format("{0}.png", tex.name),
                        "png"
                    );

                    if (savePath.Length != 0)
                    {
                        var pngData = tex.EncodeToPNG();
                        if (pngData != null)
                            File.WriteAllBytes(savePath, pngData);
                    }
                }

            }
        }
    }
}
