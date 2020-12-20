using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;
    
    public float panSpeed = 10f;
    [Range(0,1)]
    [Tooltip("Percentage of the screen from border to start panning")]
    public float panBorderPercentage = 0.95f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            doMovement = !doMovement;
        }
        
        if (!doMovement)
        {
            return;
        }
        
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
    }
}
