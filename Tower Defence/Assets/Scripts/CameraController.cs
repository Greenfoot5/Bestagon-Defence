using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    [Range(0,1)]
    [Tooltip("Percentage of the screen from border to start panning")]
    public float panBorderPercentage = 0.95f;
    public Vector2 minPos = new Vector2(0, 0);
    public Vector2 maxPos = new Vector2(0, 0);
    
    // Used when changing the camera size
    public float scrollSpeed = 5000f;
    public float minOrthSize = 3;
    public float maxOrthSize = 9;

    private Vector2 _cameraSpeed;
    private float _scrolling;

    void Start()
    {
        GameStats.controls.CameraMovement.Movement.performed += Move;
        GameStats.controls.CameraMovement.Zoom.performed += Scroll;
    }

    void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device == Keyboard.current)
        {
            _cameraSpeed = ctx.ReadValue<Vector2>() * panSpeed;
        }
        else if (ctx.control.device == Touchscreen.current)
        {
            _cameraSpeed = ctx.ReadValue<Vector2>() * panSpeed;
        }
    }

    void Scroll(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device == Keyboard.current)
        {
            _scrolling = ctx.ReadValue<float>() * scrollSpeed;
        }
        else if (ctx.control.device == Touchscreen.current)
        {
            // var touchZero = Touchscreen.current.primaryTouch;
            // var touchOne = Touchscreen.current.touches[0];
            //
            // // Find the position in the previous frame of each touch.
            // var touchZeroPrevPos = touchZero.position.ReadValue() - touchZero.delta.ReadValue();
            // var touchOnePrevPos = touchOne.position.ReadValue() - touchOne.delta.ReadValue();
            //
            // // Find the magnitude of the vector (the distance) between the touches in each frame.
            // var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            // var touchDeltaMag = (touchZero.position.ReadValue() - touchOne.position.ReadValue()).magnitude;
            //
            // // Find the difference in the distances between each frame.
            // _scrolling = 0.01f * (touchDeltaMag - prevTouchDeltaMag);
            // Debug.Log(_scrolling);
        }
    }
    
    void Update()
    {
        // Disable panning if the game is over
        if (GameManager.isGameOver)
        {
            enabled = false;
            return;
        }

        var transformPosition = transform.position;
        float newPositionX = Mathf.Clamp(transformPosition.x + _cameraSpeed.x * Time.deltaTime, minPos.x, maxPos.x);
        float newPositionY = Mathf.Clamp(transformPosition.y + _cameraSpeed.y * Time.deltaTime, minPos.y, maxPos.y);
        transform.Translate(new Vector3(newPositionX, newPositionY, transformPosition.z) - transformPosition, Space.World);
        _cameraSpeed = new Vector2();

        // Implement scrolling by changing the Orthographic Size on the camera
        float orthSize = transform.GetComponent<Camera>().orthographicSize;
        orthSize -= _scrolling * Time.deltaTime;
        orthSize = Mathf.Clamp(orthSize, minOrthSize, maxOrthSize);

        transform.GetComponent<Camera>().orthographicSize = orthSize;
    }
}
