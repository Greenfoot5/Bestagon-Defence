using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// Allows the use of a variable or constant Animation Curve.
    /// </summary>
    [System.Serializable]
    public class CurvedReference
    {
        public bool useConstant = true;
        public AnimationCurve constantValue;
        public CurvedVariable variable;

        public AnimationCurve Value => useConstant ? constantValue : variable.value;
    }
}