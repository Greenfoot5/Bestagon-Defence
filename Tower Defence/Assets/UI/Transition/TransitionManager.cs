using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Transitions
{
    public class TransitionManager : MonoBehaviour
    {
        private static readonly int ClosingTrigger = Animator.StringToHash("Close");
        private static readonly int OpeningTrigger = Animator.StringToHash("Open");

        private Animator _animator;

        [SerializeField]
        private HexagonSpread _spread;

        [SerializeField]
        private RectTransform _ringInner;
        [SerializeField]
        private RectTransform _ringOuter;

        [SerializeField]
        private Camera _camera;

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
        }

        /// <summary>
        /// The singleton instance of the transition
        /// </summary>
        public static TransitionManager Instance { get; private set; }

        /// <summary>
        /// The duration of the closing transition
        /// </summary>
        public float TransitionDuration => _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        /// <summary>
        /// Runs the closing animation
        /// </summary>
        public void Close()
        {
            _animator.SetTrigger(ClosingTrigger);
        }

        /// <summary>
        /// Runs the opening animation
        /// </summary>
        public void Open()
        {
            _animator.SetTrigger(OpeningTrigger);
        }

        private IEnumerator Animate(string sceneName)
        {
            Close();

            Debug.Log(_spread.GetDuration(State.IN));

            yield return new WaitForSeconds(Mathf.Max(_spread.GetDuration(State.IN), TransitionDuration));

            _loadingScene = sceneName;
            SceneManager.LoadScene(sceneName);
        }

        private void SceneLoadEvent(Scene scene, LoadSceneMode mode)
        {
            if (_loadingScene == scene.name)
            {
                Open();
                _loadingScene = null;
            }
        }

        /// <summary>
        /// Loads a scene and handles transitions
        /// </summary>
        /// <param name="sceneName">The scene to load</param>
        public void LoadScene(string sceneName, Vector2? pointerLocation = null)
        {
            Vector2 pointer = _camera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            _spread.SetOrigin(State.IN, pointer);

            _ringInner.position = pointer;
            _ringOuter.position = pointer;

            StartCoroutine(Animate(sceneName));
        }
    }
}
