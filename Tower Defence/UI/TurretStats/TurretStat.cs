using Godot;

namespace UI.TurretStats
{
    /// <summary>
    /// Displays a single stat on a turret's shop card
    /// </summary>
    public partial class TurretStat : Control
    {
        /// <summary>
        /// The icon of the turret stat to set the colour of
        /// </summary>
        [Export]
        private TextureRect icon;
        /// <summary>
        /// The text to fill with the current turret's stat
        /// </summary>
        [Export]
        private Label text;
        
        /// <summary>
        /// Sets the colour of the icon
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            icon.SelfModulate = color;
        }
        
        /// <summary>
        /// Sets the text's content for the stat
        /// </summary>
        /// <param name="data"></param>
        private void SetData(string data)
        {
            text.Text = data;
        }
        
        /// <summary>
        /// Sets all data for the stat
        /// </summary>
        /// <param name="data">The data to set</param>
        public void SetData(object data) => SetData(data.ToString());
    }
}
