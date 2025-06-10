using Godot;

namespace BestagonDefense.Gameplay.Camera;

/// <summary>
/// Allows the player to move the camera during gameplay
/// </summary>
public partial class CameraController : Camera2D
{
    /// <summary>
    /// Keyboard panning multiplier
    /// </summary>
    [ExportGroup("Panning")]
    [Export]
    private float keyboardPanSpeed = 10f;
    /// <summary>
    /// Mouse panning multiplier
    /// </summary>
    [Export]
    private float mouseSensitivity = 1f;
    /// <summary>
    /// Touch panning multiplier
    /// </summary>
    [Export]
    private float touchSensitivity = 1f;
    /// <summary>
    /// Panning multiplier based on zoom
    /// </summary>
    [Export]
    private float zoomInfluence = 3f;
    
    /// <summary>
    /// Multiplier for keyboard scroll speed
    /// </summary>
    [ExportGroup("Zoom")]
    [Export]
    private float keyboardZoomSpeed = 1f;
    /// <summary>
    /// Multiplier for mouse scroll speed
    /// </summary>
    [Export]
    private float mouseScrollSpeed = 10f;
    /// <summary>
    /// Multiplier for the pinch zoom speed
    /// </summary>
    [Export]
    private float pinchSpeed = 1f;
    /// <summary>
    /// The minimum zoom size that can be reached
    /// </summary>
    [Export]
    private float minZoom = 3;
    /// <summary>
    /// The maximum zoom size that can be reached
    /// </summary>
    [Export]
    private float maxZoom = 9;

    // Input System
    // TODO - Input System
    // private InputAction _moveCamera;
    // private InputAction _zoomCamera;
    
    /// <summary>
    /// Called at the start of the level. Sets up internal variables.
    /// </summary>
    public override void _Ready()
    {
        // Yaw
        mouseSensitivity /= 180;
        touchSensitivity /= 180;
    }

    public override void _PhysicsProcess(double delta)
    {
        var fDelta = (float)delta;
            
        Move(fDelta);
        ZoomProcess(fDelta);
    }

    /// <summary>
    /// Handles the camera movement
    /// </summary>
    private void Move(float delta)
    {
        if (Input.IsActionPressed("ui_left"))
            Position -= new Vector2(keyboardPanSpeed * delta, 0);
        if (Input.IsActionPressed("ui_right"))
            Position += new Vector2(keyboardPanSpeed * delta, 0);
        if  (Input.IsActionPressed("ui_up"))
            Position -= new Vector2(0, keyboardPanSpeed * delta);
        if (Input.IsActionPressed("ui_down"))
            Position += new Vector2(0, keyboardPanSpeed * delta);
            
        // Mouse Input
        // if (_moveCamera.activeControl.device == Pointer.current)
        // {
        //     return _moveCamera.ReadValue<Vector2>() * mouseSensitivity;
        // }

        // Mobile Input
        // if (_moveCamera.activeControl.device == Touchscreen.current)
        // {
        //     return _moveCamera.ReadValue<Vector2>() * touchSensitivity;
        // }
    }

    /// <summary>
    /// Handles zooming of the camera
    /// </summary>
    /// <param name="delta"></param>
    private void ZoomProcess(float delta)
    {
        if (Input.IsActionPressed("zoom_in"))
            Zoom *= new Vector2(1 + keyboardZoomSpeed * delta, 1 + keyboardZoomSpeed * delta);
        if (Input.IsActionPressed("zoom_out"))
            Zoom *= new Vector2(1 - keyboardZoomSpeed * delta, 1 - keyboardZoomSpeed * delta);
            
        Zoom = Zoom.Clamp(minZoom, maxZoom);
    }
    
    /// <summary>
    /// Gets the zoom speed based on player input
    /// </summary>
    /// <returns>The speed the camera should zoom in/out during the next frame, may be 0</returns>
    private float Scroll()
    {
        // if (_zoomCamera.activeControl != null && _zoomCamera.activeControl.device == Mouse.current)
        // {
        //     return _zoomCamera.ReadValue<float>() * scrollSpeed;
        // }

        // if (Touch.activeFingers.Count != 2) return 0f;
        
        // Touch touchZero = Touch.activeTouches[0];
        // Touch touchOne = Touch.activeTouches[1];
        
        // Find the position in the previous frame of each touch.
        // Vector2 touchZeroPrevPos = touchZero.screenPosition - touchZero.delta;
        // Vector2 touchOnePrevPos = touchOne.screenPosition - touchOne.delta;
        
        // Find the magnitude of the vector (the distance) between the touches in each frame.
        // float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        // float touchDeltaMag = (touchZero.screenPosition - touchOne.screenPosition).magnitude;
        
        // Find the difference in the distances between each frame.
        // return (touchDeltaMag - prevTouchDeltaMag) * pinchSpeed;
        return 0;

    }
}