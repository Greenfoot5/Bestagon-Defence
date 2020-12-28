using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Tracks if we currently want to pan or not
    [SerializeField]
    private bool _doMovement = false;
    
    public float panSpeed = 10f;
    [Range(0,1)]
    [Tooltip("Percentage of the screen from border to start panning")]
    public float panBorderPercentage = 0.95f;
    
    // Used when changing the camera size
    public float scrollSpeed = 5000f;
    public float minOrthSize = 3;
    public float maxOrthSize = 9;
    
    void Update()
    {
        // Enables/disables panning
        // TODO - Fix iffy use
        if (Input.GetKey(KeyCode.Escape))
        {
            _doMovement = !_doMovement;
        }
        
        if (!_doMovement)
        {
            return;
        }
        
        // Each of the panning inputs.
        // Then we move the camera on the x or y to pan
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height * panBorderPercentage)
        {
            transform.Translate(Vector3.up * (panSpeed * Time.deltaTime), Space.World);
        }
        else if (Input.GetKey("s") || Input.mousePosition.y <= Screen.height * (1 - panBorderPercentage))
        {
            transform.Translate(Vector3.down * (panSpeed * Time.deltaTime), Space.World);
        }
        else if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width * panBorderPercentage)
        {
            transform.Translate(Vector3.right * (panSpeed * Time.deltaTime), Space.World);
        }
        else if (Input.GetKey("a") || Input.mousePosition.x <= Screen.width * (1 - panBorderPercentage))
        {
            transform.Translate(Vector3.left * (panSpeed * Time.deltaTime), Space.World);
        }
        
        // Implement scrolling by changing the Orthographic Size on the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        float orthSize = transform.GetComponent<Camera>().orthographicSize;
        orthSize -= scroll * scrollSpeed * Time.deltaTime;
        Mathf.Clamp(orthSize, minOrthSize, maxOrthSize);

        transform.GetComponent<Camera>().orthographicSize = orthSize;
    }
}
