using UnityEngine;

namespace Abstract.InterpolatedMovement
{
    public class TestInterpolatedMovement : MonoBehaviour
    {
        [SerializeField] private float responseSpeed = 4.5f;
        [SerializeField] private float damping = 0.35f;
        [SerializeField] private float response = -3.5f; // >1 to overshoot, <1 to anticipate motion

        [SerializeField] private Transform objectToMove;
    
        private SecondOrderDynamics _dynamics;
        private Vector3 _previousPosition;

        private void Awake()
        {
            Vector3 position = objectToMove.position;
            _previousPosition = position;
            _dynamics = new SecondOrderDynamics(responseSpeed, damping, response, position);
        }

        private void LateUpdate()
        {
            objectToMove.position = _dynamics.Update(Time.deltaTime, transform.position);
            _previousPosition = objectToMove.position;
        }
    }
}