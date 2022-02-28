using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace UI.Transitions
{
    public enum State
    {
        IN,
        OUT
    }

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

    public class HexagonTransition : MonoBehaviour
    {
        private const float magicNumber = 1.7320508f;
        private const float magicNumberHalf = 0.8660254f;

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

        private float _viewHeight;
        private float _viewWidth;

        private float _hexagonSize;
        private float _gridHexSize;

        private float _maxSize;
        private int _vertexCount = 0;

        private bool _init;

        private ComputeBuffer _positionBuffer;

        private ComputeBuffer _colorBuffer;
        private ComputeBuffer _alphaBuffer;

        private Material CurrentMaterial => newState ? _materialIn : _materialOut;

        private void Start()
        {
            UpdateGradient();
            ReloadHexagonSize();
            RegenerateGrid();
            _init = true;
        }

        private void OnDestroy()
        {
            _positionBuffer?.Dispose();
            _colorBuffer?.Dispose();
            _alphaBuffer?.Dispose();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!_init) return;
            UpdateGradient();
            ReloadHexagonSize();
            RegenerateGrid();
            Debug.LogWarning("Editing buffers is extremely costly and causes more and more memory to be allocated. Please don't edit this component too much!");
        }
#endif

        private (float, float) GetCameraWorldSize()
        {
            float h = _camera.orthographicSize * 2;
            return (h * _camera.pixelWidth / _camera.pixelHeight, h);
        }

        private void ReloadHexagonSize()
        {
            _hexagonSize = CurrentMaterial.GetFloat(_hexSizeID) * .02f;
            _gridHexSize = _hexagonSize * _spacingMultiplier;
        }

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

        private void RegenerateGrid()
        {
            float cx = _viewWidth / (_gridHexSize * magicNumber),
                  cy = _viewHeight / (_gridHexSize * 3);

            int ix, iy;

            ix = Mathf.CeilToInt(cx) - 1;
            iy = Mathf.CeilToInt(cy) - 1;

            if (_gridHexSize * 2 < _viewHeight - iy * _gridHexSize * 3)
                iy++;

            if (.5f < cx - ix)
                ix++;

            int jx, jy;

            jy = iy + (cy - iy > .25 ? 1 : 0);
            jx = ix + (cx > ix ? 1 : 0);

            _vertexCount = (iy * 2 + 1) * (ix * 2 + 1) + jy * 2 * jx * 2;

            List<Vector3> vectors = new List<Vector3>();

            if (_gridHexSize < .01)
                return;

            for (int y = -iy; y <= iy; y++)
                for (int x = -ix; x <= ix; x++)
                    vectors.Add(new Vector3(x * _gridHexSize * magicNumberHalf, y * _gridHexSize * 1.5f) + transform.position);

            for (int y = -jy; y < jy; y++)
                for (int x = -jx; x < jx; x++)
                    vectors.Add(new Vector3((x + .5f) * _gridHexSize * magicNumberHalf, (y + .5f) * _gridHexSize * 1.5f) + transform.position);

            _positionBuffer?.Release();
            _positionBuffer = new ComputeBuffer(vectors.Count, sizeof(float) * 3);
            _positionBuffer.SetData(vectors.ToArray());
            GC.SuppressFinalize(_positionBuffer);

            _materialIn.SetBuffer(_bufferID, _positionBuffer);
            _materialOut.SetBuffer(_bufferID, _positionBuffer);

            float mx = (jx > ix ? jx + .5f : ix) * _gridHexSize * magicNumberHalf;
            float my = (jy > iy ? jy + .5f : iy) * _gridHexSize * 1.5f;
            _maxSize = new Vector2(mx, my).magnitude;
            _materialIn.SetFloat(_gridMaxID, _maxSize);
            _materialOut.SetFloat(_gridMaxID, _maxSize);
        }

        private void Update()
        {
            if (newState != _state)
            {
                CurrentMaterial.SetFloat(_startID, Time.unscaledTime);
                _state = newState;
            }

            CurrentMaterial.SetFloat(_timeID, Time.unscaledTime);

            RecheckChanges();

            Graphics.DrawProcedural(CurrentMaterial, new Bounds(transform.position, new Vector2(_viewWidth, _viewHeight)), MeshTopology.Points, 1, _vertexCount, _camera, layer: _layer);
        }

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

        public float GetDuration(State state)
        {
            Material mat = state == State.IN ? _materialIn : _materialOut;
            return mat.GetFloat(_durID) * (1 + _materialIn.GetVector(_originID).magnitude / _maxSize) + mat.GetFloat(_appDurID);
        }

        public void UpdateGradient()
        {
            _colorBuffer?.Release();
            _alphaBuffer?.Release();

            _colorBuffer = new ComputeBuffer(_gradient.colorKeys.Length, sizeof(float) * 4);
            _alphaBuffer = new ComputeBuffer(_gradient.alphaKeys.Length, sizeof(float) * 2);

            List<ColorStep> colors = new List<ColorStep>();
            for (int i = 0; i < _gradient.colorKeys.Length; i++)
                colors.Add(new ColorStep(_gradient.colorKeys[i]));

            _colorBuffer.SetData(colors.ToArray());

            List<AlphaStep> alphas = new List<AlphaStep>();
            for (int i = 0; i < _gradient.alphaKeys.Length; i++)
                alphas.Add(new AlphaStep(_gradient.alphaKeys[i]));

            _alphaBuffer.SetData(alphas.ToArray());

            _materialIn.SetBuffer(_colorID, _colorBuffer);
            _materialOut.SetBuffer(_colorID, _colorBuffer);

            _materialIn.SetBuffer(_alphaID, _alphaBuffer);
            _materialOut.SetBuffer(_alphaID, _alphaBuffer);

            _materialIn.SetInt(_colorCountID, colors.Count);
            _materialOut.SetInt(_colorCountID, colors.Count);

            _materialIn.SetInt(_alphaCountID, alphas.Count);
            _materialOut.SetInt(_alphaCountID, alphas.Count);

            GC.SuppressFinalize(_colorBuffer);
            GC.SuppressFinalize(_alphaBuffer);
        }
    }
}
