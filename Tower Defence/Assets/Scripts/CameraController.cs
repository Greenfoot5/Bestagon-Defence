using UnityEngine;

public class CameraController : MonoBehaviour
{
    #if UNITY_STANDALONE
    [Tooltip("Keyboard panning multiplier")]
    public float keyboardPanSpeed = 10f;
    
    #elif UNITY_ANDROID || UNITY_IOS
    [Tooltip("Swipe panning multiplier")]
    public float swipePanSpeed = 1f;
    #endif
    
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

    private void Start()
    {
        _camera = transform.GetComponent<Camera>();
    }
    
    /// <summary>
    /// Called every update. Used when the player wants to move the camera
    /// </summary>
    private void Move()
    {
        #if UNITY_STANDALONE
        // Keyboard & Mouse Input
        _cameraSpeed = new Vector2(Input.GetAxis("Pan Horizontal"), Input.GetAxis("Pan Vertical"));
        _cameraSpeed *= keyboardPanSpeed;
        
        
        #elif UNITY_ANDROID || UNITY_IOS
        // Mobile Input
        if (Input.touches.Length != 1) return;
        var touch = Input.touches[0];
        if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        {
            _cameraSpeed = -touch.deltaPosition * swipePanSpeed;
        }
        #endif
    }
    
    /// <summary>
    /// Called every update. Used when the player wants the camera to scroll in or out
    /// </summary>
    private void Scroll()
    {
        #if UNITY_STANDALONE
        // Keyboard & Mouse input
        _scrolling = Input.mouseScrollDelta.y * scrollSpeed;
        
        #elif UNITY_ANDROID || UNITY_IOS
        // Mobile input
        if (Input.touches.Length == 2)
        {
            var touch0Pos = Input.touches[0].position;
            var touch1Pos = Input.touches[1].position;

            var pinchLength = touch0Pos - touch1Pos;

            if (_prevPinchMag != 0)
            {
                _scrolling = pinchLength.magnitude - _prevPinchMag;
            }

            _prevPinchMag = pinchLength.magnitude;
        }
        else
        {
            _prevPinchMag = 0f;
        }
        #endif
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
