using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Transitions
{
    public class TransitionManager : MonoBehaviour
    {
        private static readonly int ClosingTrigger = Animator.StringToHash("Close");
        private static readonly int OpeningTrigger = Animator.StringToHash("Open");

        private Animator _animator;

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

            yield return new WaitForSeconds(TransitionDuration);

            SceneManager.LoadScene(sceneName);

            Open();
        }

        /// <summary>
        /// Loads a scene and handles transitions
        /// </summary>
        /// <param name="sceneName">The scene to load</param>
        public void LoadScene(string sceneName)
        {
            StartCoroutine(Animate(sceneName));
        }
    }
}
