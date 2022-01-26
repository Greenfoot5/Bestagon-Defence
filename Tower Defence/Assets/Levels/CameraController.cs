using Abstract;
using Abstract.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraController : MonoBehaviour
{
    [Header("Panning")]
    [Tooltip("Keyboard panning multiplier")]
    public float keyboardPanSpeed = 10f;
    [Tooltip("Swipe panning multiplier")]
    public float swipePanSpeed = 1f;
    [Tooltip("The minimum position the camera can reach")]
    public Vector2 minPos = new Vector2(0, 0);
    [Tooltip("The maximum position the camera can reach")]
    public Vector2 maxPos = new Vector2(0, 0);
    
    [Header("Zoom")]
    [Tooltip("Multiplier for the mouse scroll speed")]
    public float scrollSpeed = 10f;
    [Tooltip("Multiplier for the pinch zoom speed")]
    public float pinchSpeed = 1f;
    [Tooltip("The minimum Orthographic size that can be reached (maximum zoom)")]
    public float minOrthSize = 3;
    [Tooltip("The maximum Orthographic size that can be reached (minimum zoom)")]
    public float maxOrthSize = 9;
    private Camera _camera;

    // Input System
    private InputAction _moveCamera;
    private InputAction _zoomCamera;
    
    /// <summary>
    /// Called at the start of the level. Sets up internal variables.
    /// </summary>
    private void Start()
    {
        _camera = transform.GetComponent<Camera>();
        _moveCamera = GameStats.controls.Default.CameraMovement;
        _zoomCamera = GameStats.controls.Default.Zoom;
    }
    
    /// <summary>
    /// Gets the camera movement speed based on player input
    /// </summary>
    /// <returns>The speed the camera should move in the next frame, may be 0</returns>
    private Vector2 Move()
    {
        if (_moveCamera.activeControl == null)
        {
            return new Vector2();
        }
        
        // Keyboard & Mouse Input
        if (_moveCamera.activeControl.device == Keyboard.current)
        {
            return _moveCamera.ReadValue<Vector2>() * keyboardPanSpeed;
        }
        
        // Mobile Input
        if (_moveCamera.activeControl.device == Touchscreen.current)
        {
            return _moveCamera.ReadValue<Vector2>() * swipePanSpeed;
        }

        return new Vector2();
    }
    
    /// <summary>
    /// Gets the zoom speed based on player input
    /// </summary>
    /// <returns>The speed the camera should zoom in/out during the next frame, may be 0</returns>
    private float Scroll()
    {
        if (_zoomCamera.activeControl != null && _zoomCamera.activeControl.device == Mouse.current)
        {
            return _zoomCamera.ReadValue<float>() * scrollSpeed;
        }

        if (Touch.activeFingers.Count != 2) return 0f;
        
        var touchZero = Touch.activeTouches[0];
        var touchOne = Touch.activeTouches[1];
        
        // Find the position in the previous frame of each touch.
        var touchZeroPrevPos = touchZero.screenPosition - touchZero.delta;
        var touchOnePrevPos = touchOne.screenPosition - touchOne.delta;
        
        // Find the magnitude of the vector (the distance) between the touches in each frame.
        var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var touchDeltaMag = (touchZero.screenPosition - touchOne.screenPosition).magnitude;
        // var touchZeroDeltaMag = Touch.activeTouches[0].delta.magnitude;
        // var touchOneDeltaMag = Touch.activeTouches[1].delta.magnitude;
        
        // Find the difference in the distances between each frame.
        return (touchDeltaMag - prevTouchDeltaMag) * pinchSpeed;

    }
    
    /// <summary>
    /// Called every frame. Moves camera and zooms camera
    /// </summary>
    private void Update()
    {
        // Disable panning if the game is over
        if (GameManager.isGameOver)
        {
            enabled = false;
            return;
        }
        
        // TODO - Check if the game actually need to call them.
        var panSpeed = Move();
        var zoomSpeed = Scroll();

        // Move's the camera
        var transformPosition = transform.position;
        var newPositionX = Mathf.Clamp(transformPosition.x + panSpeed.x * Time.deltaTime, minPos.x, maxPos.x);
        var newPositionY = Mathf.Clamp(transformPosition.y + panSpeed.y * Time.deltaTime, minPos.y, maxPos.y);
        transform.Translate(new Vector3(newPositionX, newPositionY, transformPosition.z) - transformPosition, Space.World);

        // Implement scrolling by changing the Orthographic Size on the camera
        var orthSize = _camera.orthographicSize;
        orthSize -= zoomSpeed * Time.deltaTime;
        orthSize = Mathf.Clamp(orthSize, minOrthSize, maxOrthSize);
        _camera.orthographicSize = orthSize;
    }
}
