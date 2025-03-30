using Godot;

namespace Abstract.Data
{
    /// <summary>
    /// Allows the use of a variable or constant Animation Curve.
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    [System.Serializable]
    public class WeightedCurvedReference<T>
    {
        public bool useConstant = true;
        public T item;
        public Curve constantValue;
        public CurvedVariable variable;

        public Curve Value => useConstant ? constantValue : variable.value;
    }
}
