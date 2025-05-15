using Abstract.Attributes;
using Abstract.Data;
using Godot;

namespace Turrets
{
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

        protected Turret()
        {
            Stats[AttributeType.Range] = new Attribute(2.5f, min:0f);
            Stats[AttributeType.FireRate] = new Attribute(1f, min:0f);
        }
        
        /// <summary>
        /// Stops the range displaying
        /// </summary>
        public override void _Ready()
        {
            // RangeDisplay.Visible = false;
            // RangeDisplay.ProcessMode = ProcessModeEnum.Disabled;
            UpdateRange();
        }

        protected void Update()
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
        public virtual void UpdateRange()
        {
            // Update the range shader's size
            // Vector2 localScale = GetScale();
            // RangeDisplay.Scale = new Vector2(
            //     Stats[AttributeType.Range].Value / localScale.X * 2,
            //     Stats[AttributeType.Range].Value / localScale.Y * 2);
            // ((CircleShape2D)((CollisionShape2D)Range.GetChild(0)).Shape).Radius = Stats[AttributeType.Range].Value;
        }
        
        /// <summary>
        /// Adds Modules to our turret after checking they're valid.
        /// </summary>
        /// <param name="handler">The ModuleChainHandler to apply to the turret</param>
        /// <returns>true If the Module was applied successfully</returns>
        public override bool AddModule(ModuleChainHandler handler)
        {
            bool value = base.AddModule(handler);

            UpdateRange();
            return value;
        }
        
        /// <summary>
        /// Removes a module from the turret
        /// </summary>
        /// <param name="handler">The handler of the module to remove</param>
        protected override void RemoveModule(ModuleChainHandler handler)
        {
            base.RemoveModule(handler);
            
            UpdateRange();
        }
        
        /// <summary>
        /// Called when the turret is selected, displays the turret's range
        /// </summary>
        public override void Selected()
        {
            UpdateRange();
            // RangeDisplay.Visible = true;
            // RangeDisplay.ProcessMode = ProcessModeEnum.Inherit;
        }
        
        /// <summary>
        /// Called when the turret is deselected, disables the turret's range view.
        /// </summary>
        public override void Deselected()
        {
            // RangeDisplay.Visible = false;
            // RangeDisplay.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}