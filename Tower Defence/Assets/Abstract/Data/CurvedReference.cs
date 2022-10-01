using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// Allows the use of a variable or constant Animation Curve.
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    [System.Serializable]
    public class CurvedReference<T>
    {
        public bool useConstant = true;
        public T item;
        public AnimationCurve constantValue;
        public CurvedVariable variable;

        public AnimationCurve Value => useConstant ? constantValue : variable.value;
    }
}
