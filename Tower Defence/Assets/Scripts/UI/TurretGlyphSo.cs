using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "NewTurretGlyph", menuName = "TurretGlyph", order=1)]
    public class TurretGlyphSo : ScriptableObject
    {
        public Sprite glyph;
        public Color body = new Color(255, 255, 255, 100);
        public Color shade = new Color(0, 0, 0, 0.25f);
    }
}
