using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    [Range(0,1)]
    public Vector2 minPos = new Vector2(0, 0);
    public Vector2 maxPos = new Vector2(0, 0);
    
    // Used when changing the camera size
    public float scrollSpeed = 5000f;
    public float minOrthSize = 3;
    public float maxOrthSize = 9;

    private Vector2 _cameraSpeed;
    private float _scrolling;

    void Move()
    {
        // Keyboard & Mouse Input
        _cameraSpeed = new Vector2(Input.GetAxis("Pan Horizontal"), Input.GetAxis("Pan Vertical"));
        _cameraSpeed *= panSpeed;
        // Mobile Input
        if (false)
        {
            
        }
    }

    void Scroll()
    {
        // Keyboard & Mouse input
        _scrolling = Input.mouseScrollDelta.y * scrollSpeed;
        
        // Mobile input
        if (false)
        {
            
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
        
        // TODO - Check if we actually need to call them.
        Move();
        Scroll();

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
