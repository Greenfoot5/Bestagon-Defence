using Godot;

namespace BestagonDefence.Abstract;

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
        Emitting = true;
        freeTimer.Start();
    }
}