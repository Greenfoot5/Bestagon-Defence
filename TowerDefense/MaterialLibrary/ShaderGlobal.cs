using Godot;
using Node = Levels._Nodes.Node;

namespace MaterialLibrary
{
    public class ShaderGlobal : Node
    {
        private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");
        
        /// <summary>
        /// Updates the hexagon shader with the new time
        /// </summary>
        public override void _Process(double delta)
        {
            Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
        }
    }
}
