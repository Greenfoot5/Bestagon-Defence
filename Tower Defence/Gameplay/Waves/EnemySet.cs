using Enemies;
using Godot;

namespace Gameplay.Waves;

/// <summary>
/// A set of a single enemy type making up part of a wave
/// </summary>
[GlobalClass]
public partial class EnemySet : Resource
{
    [Export]
    public EnemyStats Enemy;
    [Export]
    public int Count;
    [Export]
    public float Rate;
}