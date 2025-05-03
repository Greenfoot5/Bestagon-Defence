using Godot;

namespace UI
{
    /// <summary>
    /// A UI layout that forms a triangle with its elements
    /// </summary>
    // TODO - Actually size child size & position
    [GlobalClass]
    public partial class TriangleLayout : Container
    {
        /// <summary>
        /// If the layout is flipped along the horizontal axis
        /// </summary>
        [Export]
        private bool isFlippedHorizontally;
        
        /// <summary>
        /// Any padding on the left of the layout
        /// </summary>
        [ExportGroup("Padding")]
        [Export]
        private float left;
        /// <summary>
        /// Any padding on the right of the layout
        /// </summary>
        [Export]
        private float right;
        /// <summary>
        /// Any padding at the top of the layout
        /// </summary>
        [Export]
        private float top;
        /// <summary>
        /// Any padding on the bottom of the layout
        /// </summary>
        [Export]
        private float bottom;
        
        /// <summary>
        /// The size of each cell
        /// </summary>
        [ExportGroup("Sizing")]
        [Export]
        private Vector2 cellSize = new(50f, 50f);
        /// <summary>
        /// Spacing between each cell
        /// </summary>
        [Export]
        public Vector2 spacing;
        
        /// <summary>
        /// The alignment of the sprites
        /// </summary>
        [Export]
        private LayoutPreset alignment;

        public override void _Notification(int notif)
        {
            if (notif == NotificationSortChildren) {
                SetLayoutHorizontal();
                SetLayoutVertical();
            }
        }
        
        /// <summary>
        /// Creates the Horizontal Layout for the group
        /// </summary>
        public void SetLayoutHorizontal()
        {
            if (!isFlippedHorizontally)
            {
                // Loops through each of the children
                for (var i = 0; i < GetChildCount(); i++)
                {
                    var child = GetChild<Control>(i);
                    
                    // Calculate Offset Based on line
                    int lineIndex = i - TriangleCount(i + 1);
                    float inset = lineIndex * cellSize.X; // Offset from line
                    if (lineIndex != 0)
                        inset += lineIndex * spacing.X; // Offset based on spacing
                    
                    // Set the new rects
                    switch (GetHorizontalEdge())
                    {
                        // From the left edge
                        case Edge.Left:
                            inset += left;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Left, inset, cellSize.X);
                            break;
                        // From the right edge
                        case Edge.Right:
                            inset += right;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Right, inset, cellSize.X);
                            break;
                        // Centre based
                        case Edge.Top:
                        case Edge.Bottom:
                        default:
                            float width = Size.Y;
                            inset += (width - left - right) / 2;
                            inset -= CalculateLine(i + 1) * 0.5f * cellSize.X;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Left, inset, cellSize.X);
                            break;
                    }
                }
            }
            // The triangle should be horizontally flipped
            else
            {
                // Loop through all the children, but backwards
                for (int i = GetChildCount() - 1; i >= 0; i--)
                {
                    var child = GetChild<Control>(i);
                    
                    // Calculate Offset Based on line
                    int lineIndex = i - TriangleCount(i + 1);
                    float inset = lineIndex * cellSize.X; // Offset from line
                    if (lineIndex != 0)
                        inset += lineIndex * spacing.X; // Offset based on spacing
                    
                    // Set the new rects
                    switch (GetHorizontalEdge())
                    {
                        // From the left edge
                        case Edge.Left:
                            inset += left;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Left, inset, cellSize.X);
                            break;
                        // From the right edge
                        case Edge.Right:
                            inset += right;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Right, inset, cellSize.X);
                            break;
                        // Centre based
                        case Edge.Top:
                        case Edge.Bottom:
                        default:
                            float width = Size.Y;
                            inset += (width - left - right) / 2; // offset all rects to the centre
                            // Offset to the left based on line number
                            inset -= CalculateLine(i + 1) * 0.5f * cellSize.X;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Left, inset, cellSize.X);
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
            if (!isFlippedHorizontally)
            {
                // Loop through all the children
                for (var i = 0; i < GetChildCount(); i++)
                {
                    var child = GetChild<Control>(i);
                    
                    // Calculate offsets based off line
                    int lineNumber = CalculateLine(i + 1) + 1;
                    float inset = ((lineNumber - 1) * cellSize.Y); // So they don't overlap
                    if (lineNumber != 0)
                        inset += lineNumber * spacing.Y; // Padding offset
                    
                    // Set the new rects
                    switch (GetVerticalEdge())
                    {
                        // From the top edge
                        case Edge.Top:
                            inset += top;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Top, inset, cellSize.Y);
                            break;
                        // From the bottom edge
                        case Edge.Bottom:
                            inset += bottom;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Bottom, inset, cellSize.Y);
                            break;
                        // Centre based
                        case Edge.Left:
                        case Edge.Right:
                        default:
                            inset += top;
                            // The icons should be moved up slightly as they are offset horizontally by .5
                            inset -= ((lineNumber - 1) * cellSize.Y) / 2;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Top, inset, cellSize.Y);
                            break;
                    }
                }
            }
            // The triangle should be horizontally flipped
            else
            {
                // Loop through the children
                for (int i = GetChildCount() - 1; i >= 0; i--)
                {
                    var child = GetChild<Control>(i);
                    
                    // Calculate offsets based off line
                    int lineNumber = CalculateLine(GetChildCount()) - CalculateLine(i + 1) + 1;
                    float inset = ((lineNumber - 1) * cellSize.Y); // So they don't overlap
                    if (lineNumber != 0)
                        inset -= lineNumber * spacing.Y; // Padding offset
                    
                    // Set the new rects
                    switch (GetVerticalEdge())
                    {
                        // From the top edge
                        case Edge.Top:
                            inset += top;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Top, inset, cellSize.Y);
                            break;
                        // From the bottom edge
                        case Edge.Bottom:
                            inset += bottom;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Bottom, inset, cellSize.Y);
                            break;
                        // Centre based
                        case Edge.Left:
                        case Edge.Right:
                        default:
                            inset += top;
                            inset -= ((lineNumber - 1) * cellSize.Y) / 2;
                            // child.SetInsetAndSizeFromParentEdge(Edge.Top, inset, cellSize.Y);
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Obtains which horizontal edge to base the group from
        /// </summary>
        /// <returns>Left/Right for horizontal edge alignments, or Top for centre and custom</returns>
        private Edge GetHorizontalEdge()
        {
            switch (alignment)
            {
                // TODO - Check LayoutPresets
                // We use the left edge
                case LayoutPreset.BottomLeft:
                case LayoutPreset.CenterLeft:
                case LayoutPreset.TopLeft:
                    return Edge.Left;
                // We use the right edge
                case LayoutPreset.BottomRight:
                case LayoutPreset.CenterRight:
                case LayoutPreset.TopRight:
                    return Edge.Right;
                case LayoutPreset.Center:
                case LayoutPreset.CenterTop:
                case LayoutPreset.CenterBottom:
                default:
                    // Centre or custom
                    return Edge.Top;
            }
        }
        
        /// <summary>
        /// Obtains which vertical edge to base the group from
        /// </summary>
        /// <returns>Left/Right for horizontal edge alignments, or Top for centre and custom</returns>
        private Edge GetVerticalEdge()
        {
            switch (alignment)
            {
                case LayoutPreset.BottomLeft:
                case LayoutPreset.CenterBottom:
                case LayoutPreset.BottomRight:
                    return Edge.Bottom;
                case LayoutPreset.TopLeft:
                case LayoutPreset.CenterTop:
                case LayoutPreset.TopRight:
                    return Edge.Top;
                case LayoutPreset.Center:
                case LayoutPreset.CenterLeft:
                case LayoutPreset.CenterRight:
                default:
                    return Edge.Right;
            }
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

        private enum Edge
        {
            Top = 0,
            Right = 1,
            Bottom = 2,
            Left = 3,
        }
    }
}
