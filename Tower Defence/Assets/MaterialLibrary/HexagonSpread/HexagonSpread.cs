using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum State
{
    IN,
    OUT
}

public class HexagonSpread : MonoBehaviour
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

    public Material materialIn;
    public Material materialOut;
    [Range(.5f, 1.5f)] public float spacingMultiplier = 1f;

    public int layer = 0;

    private bool _state = true;
    public bool newState = false;

    private float _viewHeight;
    private float _viewWidth;
    
    private float _hexagonSize;
    private float _gridHexSize;

    private float _maxSize;
    private int _vertexCount = 0;

    private ComputeBuffer _positionBuffer;

    private Material CurrentMaterial => newState ? materialIn : materialOut;

    private void Start()
    {
        ReloadHexagonSize();
        RegenerateGrid();
    }

    private void OnDestroy()
    {
        _positionBuffer?.Release();
        _positionBuffer?.Dispose();
    }

    private (float, float) GetCameraWorldSize()
    {
        float h = 10;
        return (h * Camera.main.pixelWidth / Camera.main.pixelHeight, h);
    }

    private void ReloadHexagonSize()
    {
        _hexagonSize = CurrentMaterial.GetFloat(_hexSizeID) * .02f;
        _gridHexSize = _hexagonSize * spacingMultiplier;
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
        _positionBuffer?.Dispose();
        _positionBuffer = new ComputeBuffer(vectors.Count, sizeof(float) * 3);
        _positionBuffer.SetData(vectors.ToArray());

        materialIn.SetBuffer(_bufferID, _positionBuffer);
        materialOut.SetBuffer(_bufferID, _positionBuffer);

        float mx = (jx > ix ? jx + .5f : ix) * _gridHexSize * magicNumberHalf;
        float my = (jy > iy ? jy + .5f : iy) * _gridHexSize * 1.5f;
        _maxSize = new Vector2(mx, my).magnitude;
        materialIn.SetFloat(_gridMaxID, _maxSize);
        materialOut.SetFloat(_gridMaxID, _maxSize);
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

        Graphics.DrawProcedural(CurrentMaterial, new Bounds(transform.position, new Vector2(_viewWidth, _viewHeight)), MeshTopology.Points, 1, _vertexCount, layer: layer);

        //GetComponent<CanvasRenderer>().SetMaterial(CurrentMaterial, 0);
    }

    public void SetOrigin(State state, Vector2 origin)
    {
        switch (state)
        {
            case State.IN:
                materialIn.SetVector(_originID, origin);
                break;

            case State.OUT:
                materialOut.SetVector(_originID, origin);
                break;
        }
    }

    public float GetDuration(State state)
    {
        Material mat = state == State.IN ? materialIn : materialOut;
        return mat.GetFloat(_durID) * (1 + materialIn.GetVector(_originID).magnitude / _maxSize) + mat.GetFloat(_appDurID);
    }

    //private void OnPostRender()
    //{
    //    Graphics.DrawProceduralNow(MeshTopology.Points, 1, _vertexCount, );
    //}

    //private void OnGUI()
    //{
    //    CurrentMaterial.SetFloat(_timeID, Time.unscaledTime);

    //    RecheckChanges();

    //    //CurrentMaterial.SetPass(0);
    //    //Graphics.DrawProcedural(MeshTopology.Points, 1, _vertexCount);

    //    Graphics.DrawProcedural(CurrentMaterial, new Bounds(transform.position, new Vector2(_viewWidth, _viewHeight)), MeshTopology.Points, 1, _vertexCount);
    //}

    //public void Rebuild(CanvasUpdate executing)
    //{
    //    switch (executing)
    //    {
    //        case CanvasUpdate.PreRender:
    //            Debug.Log("HIHI");
    //            break;
    //    }
    //}

    //public void LayoutComplete()
    //{
    //    return;
    //}

    //public void GraphicUpdateComplete()
    //{
    //    return;
    //}

    //public bool IsDestroyed()
    //{
    //    return false;
    //}
}
