using Godot;

namespace Abstract.Data
{
    [GlobalClass]
    public partial class CurvedVariable : Resource
    {
        [Export]
        public Curve value;
    }
}
