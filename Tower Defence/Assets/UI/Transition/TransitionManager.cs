using System.Collections;
using Abstract;
using TMPro;
using UI.Transitions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Transition
{
    /// <summary>
    /// Automatically animates the transition and manages the loading of new scenes
    /// </summary>
    public class TransitionManager : MonoBehaviour
    {
        // Triggers
        private static readonly int ClosingTrigger = Animator.StringToHash("Close");
        private static readonly int OpeningTrigger = Animator.StringToHash("Open");

        private Animator _animator;
        
        [Header("Visuals")]
        [Tooltip("The hexagon transition that fills the screen")]
        [SerializeField]
        private HexagonTransition _transition;

        [Tooltip("The text object that displays the scene name that's being loaded")]
        [SerializeField]
        private TextMeshProUGUI _sceneName;

        [Header("Rings")]
        [Tooltip("The inner ring that appears on the press location")]
        [SerializeField]
        private RectTransform _innerRing;
        [Tooltip("The outer ring that appears on the press location")]
        [SerializeField]
        private RectTransform _outerRing;

        [Header("System")]
        [Tooltip("The camera used to calculate the press location")]
        [SerializeField]
        private Camera _camera;

        [Header("Children")]
        [Tooltip("The background to disable on OSX builds")] [SerializeField]
        private GameObject _background;

        private string _loadingScene = string.Empty;

        /// <summary>
        /// Singleton pattern and finds its own animator
        /// </summary>
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _animator = GetComponentInChildren<Animator>();

            SceneManager.sceneLoaded += SceneLoadEvent;
            
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                _background.SetActive(false);
#endif
        }

        /// <summary>
        /// The singleton instance of the transition
        /// </summary>
        public static TransitionManager Instance { get; private set; }

        /// <summary>
        /// The duration of the closing transition
        /// </summary>
        private float TransitionDuration => Mathf.Max(_transition.GetDuration(State.IN), _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        /// <summary>
        /// Runs the closing animation
        /// </summary>
        private void Close()
        {
            _animator.SetTrigger(ClosingTrigger);
        }

        /// <summary>
        /// Runs the opening animation
        /// </summary>
        private void Open()
        {
            _animator.SetTrigger(OpeningTrigger);
        }

        /// <summary>
        /// Handles the scene switch by playing the animation and waiting for it to finish
        /// </summary>
        /// <param name="sceneName">The new scene to load</param>
        private IEnumerator Animate(string sceneName)
        {
            Close();

            yield return new WaitForSeconds(TransitionDuration);

            _loadingScene = sceneName;
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// The event when the scene has finished loading.<br/>
        /// Only runs the opening animation
        /// </summary>
        private void SceneLoadEvent(Scene scene, LoadSceneMode mode)
        {
            if (_loadingScene != scene.name) return;
            
            Open();
            _loadingScene = null;
        }

        /// <summary>
        /// Loads a scene and handles transitions
        /// </summary>
        /// <param name="sceneName">The scene to load</param>
        public void LoadScene(string sceneName)
        {
            // Get the location of the press that started the scene load
            Vector2 pointer = _camera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            // Update visuals to start at the click location
            _transition.SetOrigin(State.IN, pointer);

            _innerRing.position = pointer;
            _outerRing.position = pointer;

            // Update the bottom text of the transition to match the new scene name
            _sceneName.text = sceneName switch
            {
                "MainMenu" => "Main Menu",
                "LevelSelect" => "Level Select",
                _ when sceneName.EndsWith("Level") => Utils.AddSpacesToSentence(sceneName.Substring(0, sceneName.Length - 5)),
                _ => sceneName
            };

            StartCoroutine(Animate(sceneName));
        }
    }
}
