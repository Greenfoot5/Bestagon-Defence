using UnityEditor;
using UnityEngine;

namespace Editor.Waves
{
    public class WaveEditor : EditorWindow
    {
        private string _myString = "Hello World";
        private bool _groupEnabled;
        private bool _myBool = true;
        private float _myFloat = 1.23f;

        private LevelData.LevelData _levelWaves;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/WaveEditor")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (WaveEditor)GetWindow(typeof(WaveEditor));
            window.titleContent = new GUIContent("Wave Editor");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            _myString = EditorGUILayout.TextField("Text Field", _myString);

            _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
            _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
            _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
            EditorGUILayout.EndToggleGroup();
        }
    }
}