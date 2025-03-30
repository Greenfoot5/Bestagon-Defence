using Godot;


namespace UI
{
    /// <summary>
    /// Allows a piece of text to have a random hue when loading the scene
    /// </summary>
    public partial class RandomHueOnLoad : RichTextLabel
    {
        [Export]
        private RichTextLabel[] texts;
        
        /// <summary>
        /// Generates a random hue for the text
        /// </summary>
        public override void _Ready()
        {
            float hue = GD.Randf();
            foreach (RichTextLabel text in texts)
            {
                // Load the old colours
                text.SelfModulate.ToHsv(out float h, out float s, out float v);

                // Randomise the hue and assign
                h = hue;
                text.SelfModulate = Color.FromHsv(h, s, v);
            }
        }
    }
}
