using Godot;

namespace BestagonDefense.Abstract;

/// <summary>
/// Used to manage particles spawned when a node "dies"
/// </summary>
public partial class DeathVfx : GpuParticles2D
{
    /// <summary>
    /// Timer to clear the VFX
    /// </summary>
    [Export]
    private Timer freeTimer;

    public override void _Ready()
    {
        freeTimer.OneShot = true;
        freeTimer.Timeout += QueueFree;
    }
    
    /// <summary>
    /// Spawns with required attributes
    /// </summary>
    /// <param name="scale">Scale in terms of turret range</param>
    public void Init(float scale)
    {
        Scale *= scale;
        Emitting = true;
        freeTimer.Start();
    }
}