using Godot;

namespace Gameplay.Camera
{
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
        /// The minimum position the camera can reach
        /// </summary>
        [Export]
        private Vector2 minPos = new(0, 0);
        /// <summary>
        /// The maximum position of the camera
        /// </summary>
        [Export]
        private Vector2 maxPos = new(0, 0);
    
        /// <summary>
        /// Multiplier for mouse scroll speed
        /// </summary>
        [ExportGroup("Zoom")]
        [Export]
        private float scrollSpeed = 10f;
        /// <summary>
        /// Multiplier for the pinch zoom speed
        /// </summary>
        [Export]
        private float pinchSpeed = 1f;
        /// <summary>
        /// The minimum Orthographic size that can be reached (maximum zoom)
        /// </summary>
        [Export]
        private float minOrthSize = 3;
        /// <summary>
        /// The maximum Orthographic size that can be reached (minimum zoom)
        /// </summary>
        [Export]
        private float maxOrthSize = 9;

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

        /// <summary>
        /// Gets the camera movement speed based on player input
        /// </summary>
        /// <returns>The speed the camera should move in the next frame, may be 0</returns>
        private Vector2 Move()
        {
            // if (_moveCamera.activeControl == null)
            // {
            //     return new Vector2();
            // }

            // Keyboard Input
            // if (_moveCamera.activeControl.device == Keyboard.current)
            // {
            //     return _moveCamera.ReadValue<Vector2>() * (keyboardPanSpeed * delta);
            // }

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

            return new Vector2();
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
    
        // TODO - Figure out how to actually use Camera2D
        /// <summary>
        /// Called every frame. Moves camera and zooms camera
        /// </summary>
        public override void _Process(double delta)
        {
            // Disable panning if the game is over
            if (GameManager.isGameOver)
            {
                Enabled = false;
                return;
            }
        
            // TODO - Check if the game actually need to call them.
            Vector2 panSpeed = Move();
            float zoomSpeed = Scroll();

            // Gets the current camera transform
            Vector2 transformPosition = Position;
            // float orthSize = _camera.orthographicSize;

            // Moves the camera
            // TODO - Orth
            // panSpeed *= orthSize / zoomInfluence;
            float newPositionX = Mathf.Clamp(transformPosition.X + panSpeed.X, minPos.X, maxPos.X);
            float newPositionY = Mathf.Clamp(transformPosition.Y + panSpeed.Y, minPos.Y, maxPos.Y);
            // TODO - Translate
            // transform.Translate(new Vector2(newPositionX, newPositionY, transformPosition.z) - transformPosition, Space.World);

            // Implement scrolling by changing the Orthographic Size on the camera
            // orthSize -= zoomSpeed * delta;
            // orthSize = Mathf.Clamp(orthSize, minOrthSize, maxOrthSize);
            // _camera.orthographicSize = orthSize;
        }
    }
}
