using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Abstract.Data;
using Godot;

namespace BestagonDefense.Turrets;

public abstract partial class Turret : Damager
{
    /// <summary>
    /// The shader that displays the turret's range when clicked
    /// </summary>
    [Export]
    public Node2D RangeDisplay;
    [Export]
    public Area2D Range;
        
    /// <summary>
    /// How long left until the next attack
    /// </summary>
    [Export]
    public double FireCountdown;

    /// <summary>
    /// Creates a new Turret
    /// </summary>
    protected Turret()
    {
        Stats[AttributeType.Range] = new Attribute(AttributeType.Range, 2.5f, min:0f);
        Stats[AttributeType.FireRate] = new Attribute(AttributeType.FireRate, 1f, min:0f);
    }
        
    /// <summary>
    /// Stops the range displaying
    /// </summary>
    public override void _Ready()
    {
        RangeDisplay.Visible = false;
        RangeDisplay.ProcessMode = ProcessModeEnum.Disabled;
        Stats[AttributeType.Range].OnAttributeUpdated += UpdateRange;
        UpdateRange(Stats[AttributeType.Range]);
    }

    /// <summary>
    /// Handles removing listeners when leaving the tree
    /// </summary>
    public override void _ExitTree()
    {
        base._ExitTree();
        Stats[AttributeType.Range].OnAttributeUpdated -= UpdateRange;
    }

    /// <summary>
    /// Handles updating the fire countdown
    /// </summary>
    /// <param name="delta">Time since last frame (in seconds)</param>
    public override void _PhysicsProcess(double delta)
    {
        // If there's no fire rate, the turret shouldn't do anything
        if (Stats[AttributeType.FireRate].Value <= 0)
        {
            return;
        }

        if (FireCountdown > 1 / Stats[AttributeType.FireRate].Value)
        {
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
        }
    }
        
    /// <summary>
    /// Update the range shader's size
    /// </summary>
    protected virtual void UpdateRange(Attribute attribute)
    {
        Range.Scale = new Vector2(attribute.Value, attribute.Value);
    }
        
    /// <summary>
    /// Adds Modules to our turret after checking they're valid.
    /// </summary>
    /// <param name="handler">The ModuleChainHandler to apply to the turret</param>
    /// <returns>true If the Module was applied successfully</returns>
    public override bool AddModule(ModuleChainHandler handler)
    {
        bool value = base.AddModule(handler);
            
        return value;
    }
        
    /// <summary>
    /// Called when the turret is selected, displays the turret's range
    /// </summary>
    public override void Selected()
    {
        RangeDisplay.Visible = true;
        RangeDisplay.ProcessMode = ProcessModeEnum.Inherit;
    }
        
    /// <summary>
    /// Called when the turret is deselected, disables the turret's range view.
    /// </summary>
    public override void Deselected()
    {
        RangeDisplay.Visible = false;
        RangeDisplay.ProcessMode = ProcessModeEnum.Disabled;
    }
}