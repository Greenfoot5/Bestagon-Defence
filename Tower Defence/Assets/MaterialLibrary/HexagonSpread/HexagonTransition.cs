using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaterialLibrary.HexagonSpread
{
    /// <summary>
    /// The animation state
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The closing animation
        /// </summary>
        IN,
        /// <summary>
        /// The opening animation
        /// </summary>
        OUT
    }

    /// <summary>
    /// A data container for an RGB color.
    /// Values are in the 0-1 range
    /// </summary>
    public struct ColorRGB
    {
        public float r;
        public float g;
        public float b;

        public ColorRGB(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
        }
    }

    /// <summary>
    /// A data container for a color step in a gradient
    /// </summary>
    public struct ColorStep
    {
        public float step;
        public ColorRGB color;

        public ColorStep(GradientColorKey key)
        {
            step = key.time;
            color = new ColorRGB(key.color);
        }
    }

    /// <summary>
    /// A data container for an alpha step in a gradient
    /// </summary>
    public struct AlphaStep
    {
        public float step;
        public float alpha;

        public AlphaStep(GradientAlphaKey key)
        {
            step = key.time;
            alpha = key.alpha;
        }
    }

    /// <summary>
    /// The Hexagonal transition controller
    /// </summary>
    public class HexagonTransition : MonoBehaviour
    {
        // Magic hexagon numbers
        private static readonly float magicNumber = Mathf.Sqrt(3);
        private static readonly float magicNumberHalf = magicNumber * 0.5f;
        private static readonly float magicNumberQuart = magicNumber * 0.25f;

        // Material properties
        private static readonly int _startID = Shader.PropertyToID("StartTime");
        private static readonly int _timeID = Shader.PropertyToID("UnscaledTime");

        private static readonly int _bufferID = Shader.PropertyToID("Positions");
        private static readonly int _originID = Shader.PropertyToID("Origin");
        private static readonly int _gridMaxID = Shader.PropertyToID("GridMax");

        private static readonly int _hexSizeID = Shader.PropertyToID("_HexagonSize");

        private static readonly int _durID = Shader.PropertyToID("_Duration");
        private static readonly int _appDurID = Shader.PropertyToID("_AppearDuration");

        private static readonly int _colorID = Shader.PropertyToID("Colors");
        private static readonly int _colorCountID = Shader.PropertyToID("ColorCount");
        private static readonly int _alphaID = Shader.PropertyToID("Alphas");
        private static readonly int _alphaCountID = Shader.PropertyToID("AlphaCount");

        [Header("Grid")]

        [Tooltip("The spacing between hexagons automatically based on the hexagon size")]
        [SerializeField]
        [Range(.5f, 1.5f)] private float _spacingMultiplier = 1f;

        [Header("Materials")]

        [Tooltip("The material used for the closing transition")]
        [SerializeField]
        private Material _materialIn;

        [Tooltip("The material used for the opening transition")]
        [SerializeField]
        private Material _materialOut;

        [Tooltip("The color gradient that the hexagons will follow during their morphing animation")]
        [SerializeField]
        private Gradient _gradient;

        [Header("Rendering")]

        [Tooltip("The layer to render to")]
        [SerializeField]
        private int _layer = 0;

        [Tooltip("The camera to render to")]
        [SerializeField]
        private Camera _camera;

        [Header("Animation")]

        [Tooltip("The new state to change to")]
        public bool newState = false;
        private bool _state = true;

        // Data
        private float _viewHeight;
        private float _viewWidth;

        private float _hexagonSize;
        private float _gridHexSize;

        private float _maxSize;
        private int _hexagonCount = 0;

        // Anit-editor flag
        private bool _init;

        // Buffers
        private ComputeBuffer _positionBuffer;

        private ComputeBuffer _colorBuffer;
        private ComputeBuffer _alphaBuffer;

        private Material CurrentMaterial => newState ? _materialIn : _materialOut;

        // Generated hexagon geometry
        private Mesh _hexagonMesh;

        /// <summary>
        /// Generates the grid, scaling and color gradient on load
        /// </summary>
        private void Start()
        {
            UpdateGradient();
            ReloadHexagonSize();
            RegenerateMesh();
            RegenerateGrid();
            _init = true;
        }

        /// <summary>
        /// Clears all the buffers allocated
        /// </summary>
        private void OnDestroy()
        {
            _positionBuffer?.Dispose();
            _colorBuffer?.Dispose();
            _alphaBuffer?.Dispose();
        }

#if UNITY_EDITOR

        /// <summary>
        /// Generates the grid, scaling and color gradient on change in the editor.<br/>
        /// Can cause memory issues, if changed values in Play mode frequently
        /// </summary>
        private void OnValidate()
        {
            if (!_init) return;
            UpdateGradient();
            ReloadHexagonSize();
            RegenerateGrid();
            Debug.LogWarning("Editing buffers is extremely costly and causes more and more memory to be allocated. Please don't edit this component too much!");
        }
#endif

        /// <summary>
        /// Procedurally generates the hexagon mesh/geometry
        /// <para>
        /// The hexagon:
        /// <list type="bullet">
        /// <item>Is oriented so that 2 vertices are directly on the Y axis and are pointing up/down</item>
        /// <item>Has a height of 1, or the distance between the 2 vertices is 1</item>
        /// <item>Has a centre of (0,0)</item>
        /// </list>
        /// </para>
        /// </summary>
        public void RegenerateMesh()
        {
            // Create new mesh
            _hexagonMesh = new Mesh();

            // Create all vertices of a hexagon
            Vector3 
                top = new Vector2(0, 0.5f) * _gridHexSize,
                bottom = new Vector2(0, -0.5f) * _gridHexSize,
                tleft = new Vector2(-magicNumberQuart, 0.25f) * _gridHexSize,
                tright = new Vector2(magicNumberQuart, 0.25f) * _gridHexSize,
                bleft = new Vector2(-magicNumberQuart, -0.25f) * _gridHexSize,
                bright = new Vector2(magicNumberQuart, -0.25f) * _gridHexSize;

            // Append vertices to the mesh
            // Index numbers to help make the triangle array
            _hexagonMesh.vertices = new Vector3[] {
                top,  // 0
                tleft, tright,  // 1, 2
                bleft, bright,  // 3, 4
                bottom  // 5
            };

            // Append triangles (groups of 3 vertex indices)
            // Note: triangles' vertices MUST be ordered in a Clockwise order
            // Counterclockwise would result in the geometry being flipped (aka facing away from the camera <-> invisible)
            // Read more about this on https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
            _hexagonMesh.triangles = new int[] {
                1, 0, 2,
                1, 2, 4,
                3, 1, 4,
                5, 3, 4
            };
        }

        /// <summary>
        /// Gets the bounding world size based on camera parameters
        /// </summary>
        private (float, float) GetCameraWorldSize()
        {
            float h = _camera.orthographicSize * 2;
            return (h * _camera.pixelWidth / _camera.pixelHeight, h);
        }

        /// <summary>
        /// Reloads the size of hexagons and the spacing between them
        /// </summary>
        private void ReloadHexagonSize()
        {
            _hexagonSize = CurrentMaterial.GetFloat(_hexSizeID) * .02f;
            _gridHexSize = _hexagonSize * _spacingMultiplier;
        }

        /// <summary>
        /// Checks if the resolution was changed.
        /// (Useful in windowed mode)
        /// </summary>
        private void RecheckChanges()
        {
            (float w, float h) = GetCameraWorldSize();

            if (!Mathf.Approximately(h, _viewHeight) || !Mathf.Approximately(w, _viewWidth))
            {
                _viewWidth = w;
                _viewHeight = h;
                RegenerateGrid();
                return;
            }
        }

        /// <summary>
        /// Generates the grid for the hexagons to spawn
        /// </summary>
        private void RegenerateGrid()
        {
            // Hexagon base number count on both axis
            float cx = _viewWidth / (_gridHexSize * magicNumber),
                  cy = _viewHeight / (_gridHexSize * 3);

            // iy Rows of ix hexagons (half of the grid, centered)
            int ix = Mathf.CeilToInt(cx) - 1,
                iy = Mathf.CeilToInt(cy) - 1;

            // Corrections
            if (_gridHexSize * 2 < _viewHeight - iy * _gridHexSize * 3)
                iy++;

            if (.5f < cx - ix)
                ix++;

            // jy Rows of jx hexagons (other half of the grid, offset by half a hexagon on the X axis)
            int jx = ix + (cx > ix ? 1 : 0),
                jy = iy + (cy - iy > .25 ? 1 : 0);

            // The count of hexagons total needed for this grid
            _hexagonCount = (iy * 2 + 1) * (ix * 2 + 1) + jy * 2 * jx * 2;

            List<Vector3> vectors = new List<Vector3>();

            // Don't generate if the hexagon spacing is too small
            if (_gridHexSize < .01)
                return;

            // First half of the grid
            for (int y = -iy; y <= iy; y++)
                for (int x = -ix; x <= ix; x++)
                    vectors.Add(new Vector3(x * _gridHexSize * magicNumberHalf, y * _gridHexSize * 1.5f) + transform.position);

            // Second half of the grid (the offset ones)
            for (int y = -jy; y < jy; y++)
                for (int x = -jx; x < jx; x++)
                    vectors.Add(new Vector3((x + .5f) * _gridHexSize * magicNumberHalf, (y + .5f) * _gridHexSize * 1.5f) + transform.position);

            // Clear the old buffer and set a new one
            _positionBuffer?.Release();
            _positionBuffer = new ComputeBuffer(vectors.Count, sizeof(float) * 3);
            _positionBuffer.SetData(vectors.ToArray());
            GC.SuppressFinalize(_positionBuffer);

            // Assing the buffer to the materials
            _materialIn.SetBuffer(_bufferID, _positionBuffer);
            _materialOut.SetBuffer(_bufferID, _positionBuffer);

            // Find the distance of the farthest hexagon from the center
            float mx = (jx > ix ? jx + .5f : ix) * _gridHexSize * magicNumberHalf;
            float my = (jy > iy ? jy + .5f : iy) * _gridHexSize * 1.5f;
            _maxSize = new Vector2(mx, my).magnitude;

            // Save the distance to materials
            _materialIn.SetFloat(_gridMaxID, _maxSize);
            _materialOut.SetFloat(_gridMaxID, _maxSize);
        }

        /// <summary>
        /// Updates the animation state and renders the animated hexagons
        /// </summary>
        private void Update()
        {
            // If the state changed, reset the starting time for the other material
            if (newState != _state)
            {
                CurrentMaterial.SetFloat(_startID, Time.unscaledTime);
                _state = newState;
            }

            // Time update
            CurrentMaterial.SetFloat(_timeID, Time.unscaledTime);

            // Check if the grid needs updating
            RecheckChanges();

            // Queue for drawing this frame
            Graphics.DrawMeshInstancedProcedural(
                _hexagonMesh,
                submeshIndex: 0,
                CurrentMaterial,
                new Bounds(transform.position, new Vector2(_viewWidth, _viewHeight)),
                count: _hexagonCount,
                camera: _camera,
                layer: _layer
                );
        }

        /// <summary>
        /// Updates the origin for one of the materials
        /// </summary>
        /// <param name="state">The state's material to update</param>
        /// <param name="origin">The new origin to animate from</param>
        public void SetOrigin(State state, Vector2 origin)
        {
            switch (state)
            {
                case State.IN:
                    _materialIn.SetVector(_originID, origin);
                    break;

                case State.OUT:
                    _materialOut.SetVector(_originID, origin);
                    break;
            }
        }

        /// <summary>
        /// Returns the duration of the transition based on where the origin is
        /// </summary>
        /// <param name="state"></param>
        public float GetDuration(State state)
        {
            Material mat = state == State.IN ? _materialIn : _materialOut;
            return mat.GetFloat(_durID) * (1 + _materialIn.GetVector(_originID).magnitude / _maxSize) + mat.GetFloat(_appDurID);
        }

        /// <summary>
        /// Clears buffers and sets new ones containing gradient data for the shaders
        /// </summary>
        public void UpdateGradient()
        {
            // Clear buffers
            _colorBuffer?.Release();
            _alphaBuffer?.Release();

            // Initialize buffers
            _colorBuffer = new ComputeBuffer(_gradient.colorKeys.Length, sizeof(float) * 4);
            _alphaBuffer = new ComputeBuffer(_gradient.alphaKeys.Length, sizeof(float) * 2);

            // Write the color data
            List<ColorStep> colors = new List<ColorStep>();
            for (int i = 0; i < _gradient.colorKeys.Length; i++)
                colors.Add(new ColorStep(_gradient.colorKeys[i]));

            _colorBuffer.SetData(colors.ToArray());

            // Write the alpha data
            List<AlphaStep> alphas = new List<AlphaStep>();
            for (int i = 0; i < _gradient.alphaKeys.Length; i++)
                alphas.Add(new AlphaStep(_gradient.alphaKeys[i]));

            _alphaBuffer.SetData(alphas.ToArray());

            // Update both materials
            _materialIn.SetBuffer(_colorID, _colorBuffer);
            _materialOut.SetBuffer(_colorID, _colorBuffer);

            _materialIn.SetBuffer(_alphaID, _alphaBuffer);
            _materialOut.SetBuffer(_alphaID, _alphaBuffer);

            _materialIn.SetInt(_colorCountID, colors.Count);
            _materialOut.SetInt(_colorCountID, colors.Count);

            _materialIn.SetInt(_alphaCountID, alphas.Count);
            _materialOut.SetInt(_alphaCountID, alphas.Count);

            // Supress warnings (spams a lot in the editor if they get changed frequently)
            GC.SuppressFinalize(_colorBuffer);
            GC.SuppressFinalize(_alphaBuffer);
        }
    }
}
