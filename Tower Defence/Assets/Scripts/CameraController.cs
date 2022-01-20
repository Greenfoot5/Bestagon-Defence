using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CameraController : MonoBehaviour
{
    [Tooltip("Keyboard panning multiplier")]
    public float keyboardPanSpeed = 10f;

    [Tooltip("Swipe panning multiplier")]
    public float swipePanSpeed = 1f;
    
    [Tooltip("The minimum position the camera can reach")]
    public Vector2 minPos = new Vector2(0, 0);
    [Tooltip("The maximum position the camera can reach")]
    public Vector2 maxPos = new Vector2(0, 0);
    
    // Used when changing the camera size
    [Tooltip("Multiplier for the mouse scroll speed")]
    public float scrollSpeed = 5000f;
    [Tooltip("The minimum Orthographic size that can be reached (maximum zoom)")]
    public float minOrthSize = 3;
    [Tooltip("The maximum Orthographic size that can be reached (minimum zoom)")]
    public float maxOrthSize = 9;
    private Camera _camera;

    private Vector2 _cameraSpeed;
    private float _scrolling;

    private float _prevPinchMag;
    
    private float _lastMultiTouchDistance;
    
    // Input System
    private InputAction _moveCamera;
    private InputAction _zoomCamera;

    private void Start()
    {
        _camera = transform.GetComponent<Camera>();
        _moveCamera = GameStats.controls.Default.CameraMovement;
        _zoomCamera = GameStats.controls.Default.Zoom;
    }
    
    /// <summary>
    /// Called every update. Used when the player wants to move the camera
    /// </summary>
    private void Move()
    {
        if (_moveCamera.activeControl == null)
        {
            return;
        }
        
        // Keyboard & Mouse Input
        if (_moveCamera.activeControl.device == Keyboard.current)
        {
            _cameraSpeed = _moveCamera.ReadValue<Vector2>() * keyboardPanSpeed;
        }
        
        // Mobile Input
        // TODO - Implement EnhancedTouch
        if (Touch.activeFingers.Count == 2)
        {
            _cameraSpeed = _moveCamera.ReadValue<Vector2>() * swipePanSpeed;
        }
    }

    private void Scroll()
    {
        if (_zoomCamera.activeControl == null)
        {
            return;
        }
        
        if (_zoomCamera.activeControl.device == Mouse.current)
        {
            _scrolling = _zoomCamera.ReadValue<float>() * scrollSpeed;
        }
        
        // TODO - Switch to EnhancedTouch
        // else if (_zoomCamera.activeControl.device == Touchscreen.current)
        // {
        //     var touchZero = Touchscreen.current.touches[0];
        //     var touchOne = Touchscreen.current.touches[1];
        //
        //     // Find the position in the previous frame of each touch.
        //     var touchZeroPrevPos = touchZero.position.ReadValue() - touchZero.delta.ReadValue();
        //     var touchOnePrevPos = touchOne.position.ReadValue() - touchOne.delta.ReadValue();
        //
        //     // Find the magnitude of the vector (the distance) between the touches in each frame.
        //     var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        //     var touchDeltaMag = (touchZero.position.ReadValue() - touchOne.position.ReadValue()).magnitude;
        //
        //     // Find the difference in the distances between each frame.
        //     _scrolling = 0.01f * (touchDeltaMag - prevTouchDeltaMag);
        //     Debug.Log(_scrolling);
        // }
    }

    private void Update()
    {
        // Disable panning if the game is over
        if (GameManager.isGameOver)
        {
            enabled = false;
            return;
        }
        
        // TODO - Check if we actually need to call them.
        Move();
        Scroll();

        // Move's the camera
        var transformPosition = transform.position;
        var newPositionX = Mathf.Clamp(transformPosition.x + _cameraSpeed.x * Time.deltaTime, minPos.x, maxPos.x);
        var newPositionY = Mathf.Clamp(transformPosition.y + _cameraSpeed.y * Time.deltaTime, minPos.y, maxPos.y);
        transform.Translate(new Vector3(newPositionX, newPositionY, transformPosition.z) - transformPosition, Space.World);
        // Reset the camera's speed, in case the user stops moving
        _cameraSpeed = new Vector2();

        // Implement scrolling by changing the Orthographic Size on the camera
        var orthSize = _camera.orthographicSize;
        orthSize -= _scrolling * Time.deltaTime;
        orthSize = Mathf.Clamp(orthSize, minOrthSize, maxOrthSize);
        _camera.orthographicSize = orthSize;
    }
}
