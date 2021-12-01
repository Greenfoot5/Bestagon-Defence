using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TriangleLayout : MonoBehaviour, ILayoutGroup
    {
        public bool flippedHorizontally;
        
        [Header("Padding")]
        public float left;
        public float right;
        public float top;
        public float bottom;
        
        [Space(15)]
        public Vector2 cellSize = new Vector2(50f, 50f);
        public Vector2 spacing;

        public SpriteAlignment alignment;

        public void SetLayoutHorizontal()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = (RectTransform) transform.GetChild(i);
                // Calculate Offset Based on line
                var lineIndex = i - TriangleCount(i + 1);
                var inset = lineIndex * cellSize.x; // Offset from line
                Debug.Log(inset);
                if (lineIndex != 0)
                    inset += lineIndex * spacing.x; // Offset based on spacing
                Debug.Log(inset);
                switch(GetHorizontalEdge())
                {
                    case RectTransform.Edge.Left:
                        inset += left;
                        child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, cellSize.x);
                        break;
                    case RectTransform.Edge.Right:
                        inset += right;
                        child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, inset, cellSize.x);
                        break;
                    default:
                        var width = ((RectTransform)transform).rect.width;
                        inset += (width - left - right) / 2;
                        inset -= CalculateLine(i + 1) * 0.5f * cellSize.x;
                        child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, cellSize.x);
                        break;
                }

                // Set stuff
                
            }
        }

        public void SetLayoutVertical()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = (RectTransform)transform.GetChild(i);

                var lineNumber = CalculateLine(i + 1) + 1;
                //Debug.Log(CalculateLine(i));
                var inset = (lineNumber * cellSize.y / 2); //+ (lineNumber - 1) * spacing.y + top;
                //Debug.Log(inset);
                child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, cellSize.x);
            }
        }

        private RectTransform.Edge GetHorizontalEdge()
        {
            // We use the left edge
            if (alignment == SpriteAlignment.BottomLeft || alignment == SpriteAlignment.LeftCenter ||
                alignment == SpriteAlignment.TopLeft)
                return RectTransform.Edge.Left;
            
            // We use the right edge
            if (alignment == SpriteAlignment.BottomRight || alignment == SpriteAlignment.RightCenter ||
                alignment == SpriteAlignment.TopRight)
                return RectTransform.Edge.Right;
            
            // Centre or custom
            return RectTransform.Edge.Top;
        }
        
        private static int CalculateLine(int i)
        {
            var t = 1;
            while (i > TriangleNumber(t))
            {
                t++;
            }

            return t;
        }

        private static int TriangleNumber(int n)
        {
            return (n * (n + 1)) / 2;
        }

        private static int TriangleCount(int n)
        {
            return TriangleNumber(CalculateLine(n) - 1);
        }
    }
}
