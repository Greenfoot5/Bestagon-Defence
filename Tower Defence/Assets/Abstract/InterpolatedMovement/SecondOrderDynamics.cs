using System;
using UnityEngine;

namespace Abstract.InterpolatedMovement
{
    /// <summary>
    /// Creates a better SmoothDamp function that allows for wobbles in movement
    /// </summary>
    [Serializable]
    public class SecondOrderDynamics
    {
        private Vector3 _xPrevious; // previous input;
        private Vector3 _y, _yDifference; // state variables;
        private readonly float _w, _zeta, _d; // constants
        private float _k, _kVelocity, _kAcceleration; // constants

        /// <summary>
        /// Creates a Second Order Dynamics
        /// </summary>
        /// <param name="frequency">Frequency - How long each wobble takes</param>
        /// <param name="zeta">Damping Coefficient - The speed at which the wobbles die down</param>
        /// <param name="initialResponse">How fast the system responds to change</param>
        /// <param name="x0">Initial Position</param>
        public SecondOrderDynamics(float frequency, float zeta, float initialResponse, Vector3 x0)
        {
            _w = 2 * Mathf.PI * frequency;
            _zeta = zeta;
            _d = _w * Mathf.Sqrt(Mathf.Abs(zeta * zeta - 1));
            _k = zeta / (Mathf.PI * frequency);
            _kVelocity = 1 / (_w * _w);
            _kAcceleration = initialResponse * zeta / _w;

            _xPrevious = x0;
            _y = x0;
            _yDifference = Vector3.zero;
        }
        
        /// <summary>
        /// Updates the position without previous position
        /// </summary>
        /// <param name="deltaT">Change in time</param>
        /// <param name="targetPos">The position to move to</param>
        /// <returns>The new position</returns>
        public Vector3 Update(float deltaT, Vector3 targetPos)
        {
            Vector3 previousPos = (targetPos - _xPrevious) / deltaT;
            _xPrevious = targetPos;
            return Update(deltaT, targetPos, previousPos);
        }
        
        /// <summary>
        /// Updates the position with previous position
        /// </summary>
        /// <param name="deltaT">The change in time</param>
        /// <param name="targetPos">The target position</param>
        /// <param name="previousPosition">The previous position</param>
        /// <returns>The new position</returns>
        public Vector3 Update(float deltaT, Vector3 targetPos, Vector3 previousPosition)
        {
            //float kStable;
            float kVelocityStable;
            if (_w * deltaT < _zeta)
            {
                //kStable = _k;
                kVelocityStable = Mathf.Max(_kVelocity, deltaT * deltaT / 2 + deltaT * _k / 2, deltaT * _k);
            }
            else
            {
                float t1 = Mathf.Exp(-_zeta * _w * deltaT);
                float alpha = 2 * t1 * (_zeta <= 1 ? Mathf.Cos(deltaT * _d) : (float)System.Math.Cosh(deltaT * _d));
                float beta = t1 * t1;
                float t2 = deltaT / (1 + beta - alpha);
                //kStable = (1 - beta) * t2;
                kVelocityStable = deltaT * t2;
            }

            _y += deltaT * _yDifference;
            _yDifference += deltaT * (targetPos + _kAcceleration * previousPosition - _y - _k * _yDifference) / kVelocityStable;
            return _y;
        }
    }
}