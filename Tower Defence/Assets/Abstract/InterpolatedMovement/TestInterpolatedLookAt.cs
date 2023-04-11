using UnityEngine;

namespace Abstract.InterpolatedMovement
{
    public class TestInterpolatedLookAt : MonoBehaviour
    {
        [SerializeField] private Transform targetObject;
    
        [SerializeField] private float responseSpeed = 4.5f;
        [SerializeField] private float damping = 0.35f;
        [SerializeField] private float response = -3.5f; // >1 to overshoot, <1 to anticipate motion

        private SecondOrderDynamics _dynamics;
        private Vector3 _previousPosition;

        private void Awake()
        {
            _dynamics = new SecondOrderDynamics(responseSpeed, damping, response, transform.up);
        }

        private void LateUpdate()
        {
            transform.up = _dynamics.Update(Time.deltaTime, (targetObject.position - transform.position).normalized);
        }
    }
}