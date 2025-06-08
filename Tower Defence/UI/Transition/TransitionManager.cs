using System.Collections;
using Godot;

namespace UI.Transition;

/// <summary>
/// Automatically animates the transition and manages the loading of new scenes
/// </summary>
public partial class TransitionManager : Control
{
    // Triggers
    // private static readonly int ClosingTrigger = Animator.StringToHash("Close");
    // private static readonly int OpeningTrigger = Animator.StringToHash("Open");

    // private Animator _animator;
        
    [ExportGroup("Visuals")]
    /// <summary>
    /// The hexagon transition that fills the screen")]
    // [Export]
    // private HexagonTransition transition;
        
    /// <summary>
    /// The text object that displays the scene name that's being loaded")]
    [Export]
    private Label sceneName;
    // [Export]
    /// <summary>
    /// The Localisation TableReference to find scene name localisations in.")]
    // public TableReference tableReference;

    // [ExportGroup("Rings")]
    /// <summary>
    /// The inner ring that appears on the press location")]
    // [Export]
    // private RectTransform innerRing;
    /// <summary>
    /// The outer ring that appears on the press location")]
    // [Export]
    // private RectTransform outerRing;

    [ExportGroup("System")]
    /// <summary>
    /// The camera used to calculate the press location")]
    [Export]
    // ReSharper disable once InconsistentNaming
    private Camera2D _camera;

    private string _loadingScene = string.Empty;

    /// <summary>
    /// Singleton pattern and finds its own animator
    /// </summary>
    private TransitionManager()
    {
        if (Instance != null)
        {
            Free();
            return;
        }

        Instance = this;

        // _animator = GetComponentInChildren<Animator>();

        // SceneManager.sceneLoaded += SceneLoadEvent;
    }

    /// <summary>
    /// The singleton instance of the transition
    /// </summary>
    public static TransitionManager Instance { get; private set; }

    /// <summary>
    /// The duration of the closing transition
    // </summary>
    // private float TransitionDuration => Mathf.Max(transition.GetDuration(State.IN), _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

    /// <summary>
    /// Runs the closing animation
    /// </summary>
    private void Close()
    {
        // _animator.SetTrigger(ClosingTrigger);
    }

    /// <summary>
    /// Runs the opening animation
    /// </summary>
    private void Open()
    {
        // _animator.SetTrigger(OpeningTrigger);
    }

    /// <summary>
    /// Handles the scene switch by playing the animation and waiting for it to finish
    /// </summary>
    /// <param name="newSceneName">The new scene to load</param>
    private IEnumerator Animate(string newSceneName)
    {
        Close();

        // yield return new WaitForSecondsRealtime(TransitionDuration);
            
        _loadingScene = newSceneName;
        // SceneManager.LoadScene(newSceneName);
        yield break;
    }

    /// <summary>
    /// The event when the scene has finished loading.<br/>
    // Only runs the opening animation
    // </summary>
    // private void SceneLoadEvent(Scene scene, LoadSceneMode mode)
    // {
    //     if (_loadingScene != scene.Name) return;
    //     
    //     GodotObject.FindGodotObjectsWithTag("MainCamera")[0].GetComponent<Camera>().GetUniversalAdditionalCameraData().cameraStack.Add(_camera);
    //     Open();
    //     _loadingScene = null;
    // }

    /// <summary>
    /// Loads a scene and handles transitions
    /// </summary>
    /// <param name="newSceneName">The scene to load</param>
    public void LoadScene(string newSceneName)
    {
        // // Make sure time is back to normal
        // Engine.TimeScale = 1f;
        //
        // // Get the location of the press that started the scene load
        // Vector2 pointer = _camera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        //
        // // Update visuals to start at the click location
        // transition.SetOrigin(State.IN, pointer);
        //
        // innerRing.position = pointer;
        // outerRing.position = pointer;
        //
        // // Update the bottom text of the transition to match the new scene name
        // var entryReference = (TableEntryReference)newSceneName;
        // sceneName.Text =
        //     new stringDatabase().Getstring(tableReference, entryReference, LocalizationSettings.SelectedLocale);
        //
        // StartCoroutine(Animate(newSceneName));
    }
}