using UnityEditor;
using UnityEngine;

namespace Editor.Waves
{
    public class WaveEditor : EditorWindow
    {
        // The Scriptable Object storing the data
        private static LevelData.LevelData _levelData;
        private SerializedObject _serializedObject;
        
        // A property from the SO
        private SerializedProperty _initialTurretSelection;
        
        // Used for the toolbar to change between the different sections of the Level Data
        private int _selectedDataPart;
        private readonly string[] _levelDataParts = {"Shop Selection", "Waves"};
        
        
        /// <summary>
        /// Create out window
        /// </summary>
        [MenuItem("Window/Wave Editor")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (WaveEditor)GetWindow(typeof(WaveEditor));
            window.titleContent = new GUIContent("Wave Editor");
            window.Show();
        }
        
        /// <summary>
        /// Called when the window is enabled
        /// </summary>
        private void OnEnable()
        {
            _serializedObject = new SerializedObject(_levelData);
            _initialTurretSelection = _serializedObject.FindProperty("initialTurretSelection");
        }
        
        /// <summary>
        /// Used to actually build the GUI for the window
        /// </summary>
        private void OnGUI()
        {
            // Displays the currently selected LevelData's name, and allows the removal of it
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label(_levelData != null ? _levelData.name : "No Level Data selected.", EditorStyles.largeLabel);
            // Allows us to clear the _levelData in case we need to
            if (GUILayout.Button("Clear Level Data"))
            {
                _levelData = null;
            }
            GUILayout.EndHorizontal();

            if (_levelData == null)
            {
                return;
            }
            
            // The toolbar for the separation between the Level Data sections
            GUILayout.Space(5);
            _selectedDataPart = GUILayout.Toolbar(_selectedDataPart, _levelDataParts);
            GUILayout.Space(5);

            switch (_selectedDataPart)
            {
                // We want to edit the LevelData Selections
                case 0:
                    GUILayout.Label("The selection the user gets when they open the shop. Initial Selection " +
                                    "overwrites the base chances.", EditorStyles.wordWrappedMiniLabel);
                    EditorGUILayout.PropertyField(_initialTurretSelection);
                    break;
                // We want to edit the waves
                case 1:
                    GUILayout.Label("Waves!");
                    break;
                default:
                    EditorGUILayout.HelpBox(new GUIContent("It broke :'("));
                    break;
            }
            
            // Make sure we save the changes to the properties
            _serializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        /// Updates the _levelData if we select a new one
        /// </summary>
        private void Update()
        {
            // Gets the selection and checks if it's a different Level Data
            var selected = Selection.GetFiltered<LevelData.LevelData>(0);
            if (selected.Length == 0) return;
            if (selected[0] == _levelData) return;
            
            // Set the data to the new select Level Data and resets menu selections
            _levelData = selected[0];
            _selectedDataPart = 0;
            _serializedObject = new SerializedObject(_levelData);
            _initialTurretSelection = _serializedObject.FindProperty("initialTurretSelection");
        }
    }
}