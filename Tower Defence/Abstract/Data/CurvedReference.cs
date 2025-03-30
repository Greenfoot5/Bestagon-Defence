using Godot;

namespace Abstract.Data
{
    /// <summary>
    /// Allows the use of a variable or constant Animation Curve.
    /// </summary>
    [GlobalClass]
    public partial class CurvedReference : Resource
    {
        public bool useConstant = true;
        public Curve constantValue;
        public CurvedVariable variable;

        public Curve Value => useConstant ? constantValue : variable.value;
    }
}