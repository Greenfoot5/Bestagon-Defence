using System.Collections.Generic;
using Godot;

namespace Abstract.EnvironmentVariables
{
    /// <summary>
    /// An SO that allows us to easily store and reference some environment variables
    /// </summary>
    [GlobalClass]
    public partial class EnvironmentVariables : Resource
    {
        public List<EnvironmentVariable> variables;
    }
}
