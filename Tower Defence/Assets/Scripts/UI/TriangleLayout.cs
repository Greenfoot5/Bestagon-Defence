using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// A layout in the form of a triangle
    /// </summary>
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
        
        /// <summary>
        /// Creates the Horizontal Layout for the group
        /// </summary>
        public void SetLayoutHorizontal()
        {
            if (!flippedHorizontally)
            {
                // Loops through each of the children
                for (var i = 0; i < transform.childCount; i++)
                {
                    var child = (RectTransform)transform.GetChild(i);
                    
                    // Calculate Offset Based on line
                    var lineIndex = i - TriangleCount(i + 1);
                    var inset = lineIndex * cellSize.x; // Offset from line
                    if (lineIndex != 0)
                        inset += lineIndex * spacing.x; // Offset based on spacing
                    
                    // Set the new rects
                    switch (GetHorizontalEdge())
                    {
                        // From the left edge
                        case RectTransform.Edge.Left:
                            inset += left;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, cellSize.x);
                            break;
                        // From the right edge
                        case RectTransform.Edge.Right:
                            inset += right;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, inset, cellSize.x);
                            break;
                        // Centre based
                        default:
                            var width = ((RectTransform)transform).rect.width;
                            inset += (width - left - right) / 2;
                            inset -= CalculateLine(i + 1) * 0.5f * cellSize.x;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, cellSize.x);
                            break;
                    }
                }
            }
            // The triangle should be horizontally flipped
            else
            {
                // Loop through all the children, but backwards
                for (var i = transform.childCount - 1; i >= 0; i--)
                {
                    var child = (RectTransform)transform.GetChild(i);
                    
                    // Calculate Offset Based on line
                    var lineIndex = i - TriangleCount(i + 1);
                    var inset = lineIndex * cellSize.x; // Offset from line
                    if (lineIndex != 0)
                        inset += lineIndex * spacing.x; // Offset based on spacing
                    
                    // Set the new rects
                    switch (GetHorizontalEdge())
                    {
                        // From the left edge
                        case RectTransform.Edge.Left:
                            inset += left;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, cellSize.x);
                            break;
                        // From the right edge
                        case RectTransform.Edge.Right:
                            inset += right;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, inset, cellSize.x);
                            break;
                        // Centre based
                        default:
                            var width = ((RectTransform)transform).rect.width;
                            inset += (width - left - right) / 2; // offset all rects to the centre
                            // Offset to the left based on line number
                            inset -= CalculateLine(i + 1) * 0.5f * cellSize.x;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, cellSize.x);
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Creates the vertical element of the layout group
        /// </summary>
        public void SetLayoutVertical()
        {
            if (!flippedHorizontally)
            {
                // Loop through all the children
                for (var i = 0; i < transform.childCount; i++)
                {
                    var child = (RectTransform)transform.GetChild(i);
                    
                    // Calculate offsets based off line
                    var lineNumber = CalculateLine(i + 1) + 1;
                    var inset = ((lineNumber - 1) * cellSize.y); // So they don't overlap
                    if (lineNumber != 0)
                        inset += lineNumber * spacing.y; // Padding offset
                    
                    // Set the new rects
                    switch (GetVerticalEdge())
                    {
                        // From the top edge
                        case RectTransform.Edge.Top:
                            inset += top;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, cellSize.y);
                            break;
                        // From the bottom edge
                        case RectTransform.Edge.Bottom:
                            inset += bottom;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, inset, cellSize.y);
                            break;
                        // Centre based
                        default:
                            inset += top;
                            // We can move them up slightly as they are offset horizontally by .5
                            inset -= ((lineNumber - 1) * cellSize.y) / 2;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, cellSize.y);
                            break;
                    }
                }
            }
            // The triangle should be horizontally flipped
            else
            {
                // Loop through the children
                for (var i = transform.childCount - 1; i >= 0; i--)
                {
                    var child = (RectTransform)transform.GetChild(i);
                    
                    // Calculate offsets based off line
                    var lineNumber = CalculateLine(transform.childCount) - CalculateLine(i + 1) + 1;
                    var inset = ((lineNumber - 1) * cellSize.y); // So they don't overlap
                    if (lineNumber != 0)
                        inset -= lineNumber * spacing.y; // Padding offset
                    
                    // Set the new rects
                    switch (GetVerticalEdge())
                    {
                        // From the top edge
                        case RectTransform.Edge.Top:
                            inset += top;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, cellSize.y);
                            break;
                        // From the bottom edge
                        case RectTransform.Edge.Bottom:
                            inset += bottom;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, inset, cellSize.y);
                            break;
                        // Centre based
                        default:
                            inset += top;
                            inset -= ((lineNumber - 1) * cellSize.y) / 2;
                            child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, cellSize.y);
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Obtains which horizontal edge to base the group from
        /// </summary>
        /// <returns>Left/Right for horizontal edge alignments, or Top for centre and custom</returns>
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
        
        /// <summary>
        /// Obtains which vertical edge to base the group from
        /// </summary>
        /// <returns>Left/Right for horizontal edge alignments, or Top for centre and custom</returns>
        private RectTransform.Edge GetVerticalEdge()
        {
            if (alignment == SpriteAlignment.BottomLeft || alignment == SpriteAlignment.BottomCenter ||
                alignment == SpriteAlignment.BottomRight)
                return RectTransform.Edge.Bottom;

            if (alignment == SpriteAlignment.TopLeft || alignment == SpriteAlignment.TopCenter ||
                alignment == SpriteAlignment.TopRight)
                return RectTransform.Edge.Top;

            return RectTransform.Edge.Right;
        }
        
        /// <summary>
        /// Calculates the line number an index is on. This is the value as the next triangular number in the sequence
        /// </summary>
        /// <param name="i">The index to check</param>
        /// <returns>The line number</returns>
        private static int CalculateLine(int i)
        {
            var t = 1;
            while (i > TriangleNumber(t))
            {
                t++;
            }

            return t;
        }
        
        /// <summary>
        /// Calculates the triangular number based on an index for the sequence
        /// </summary>
        /// <param name="n">The sequence's index</param>
        /// <returns>The value at the specified index</returns>
        private static int TriangleNumber(int n)
        {
            return (n * (n + 1)) / 2;
        }
        
        /// <summary>
        /// Gets the amount of triangles prior to an index
        /// </summary>
        /// <param name="n">The index on the triangle</param>
        /// <returns>The amount of elements before the index</returns>
        private static int TriangleCount(int n)
        {
            return TriangleNumber(CalculateLine(n) - 1);
        }
    }
}
