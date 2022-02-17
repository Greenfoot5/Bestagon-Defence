using System;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialLibrary
{
    [ExecuteInEditMode]
    public class HexagonSprite : Graphic
    {
        public enum ScaleType
        {
            Regular,
            FitToWidth,
            FitToHeight,
            Fill
        }

        public ScaleType scalingType;

        public enum MeshType
        {
            V6T4,
            V7T6,
            V18T6
        }

        public MeshType meshType;

        public bool useTriUniqueColors;
        public Color[] colors;

        private Rect _mScaleReference = Rect.zero;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            bool colorsFlag = meshType == MeshType.V18T6 && useTriUniqueColors;
            Array.Resize(ref colors, colorsFlag ? 6 : 0);
            UpdateGeometry();
        }
#endif


        // SCALE REFERENCE

        private void UpdateScaleReference()
        {
            Rect rect = rectTransform.rect;
            float w = 1.1547f * rect.width,
                h = rect.height;

            switch (scalingType)
            {
                case ScaleType.Regular:
                    _mScaleReference.size = w > h ? new Vector2(h, h) : new Vector2(w, w);
                    break;

                case ScaleType.FitToWidth:
                    _mScaleReference.size = new Vector2(w, w);
                    break;

                case ScaleType.FitToHeight:
                    _mScaleReference.size = new Vector2(h, h);
                    break;

                case ScaleType.Fill:
                    _mScaleReference.size = new Vector2(w, h);
                    break;

                default:
                    _mScaleReference = rectTransform.rect;
                    break;
            }
        }


        // VERTEX GENERATORS

        private void AddVertexOfPositions(VertexHelper vh, UIVertex vert, int colour, params Vector2[] positions)
        {
            if (useTriUniqueColors)
                vert.color = colors[colour];
            AddVertexOfPositions(vh, vert, positions);
        }
        private static void AddVertexOfPositions(VertexHelper vh, UIVertex vert, params Vector2[] positions)
        {
            foreach (Vector2 pos in positions)
            {
                vert.position = pos;
                vh.AddVert(vert);
            }
        }

        private void GenerateV6T4(VertexHelper vh, Vector2 apex, Vector2 side)
        {
            float flipVer = -.866f * _mScaleReference.width;


            // VERTICES

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            vert.position = new Vector2(apex.x, apex.y); // Top
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y); // Top Right
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y - _mScaleReference.height / 2); // Bottom Right
            vh.AddVert(vert);

            vert.position = new Vector2(apex.x, apex.y - _mScaleReference.height); // Bottom
            vh.AddVert(vert);

            vert.position = new Vector2(side.x + flipVer, side.y - _mScaleReference.height / 2); // Bottom Left
            vh.AddVert(vert);

            vert.position = new Vector2(side.x + flipVer, side.y); // Top Left
            vh.AddVert(vert);


            // TRIANGLES

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 2, 3);
            vh.AddTriangle(5, 0, 3);
            vh.AddTriangle(5, 3, 4);
        }

        private void GenerateV7T6(VertexHelper vh, Vector2 apex, Vector2 side)
        {
            float flipVer = -.866f * _mScaleReference.width;


            // VERTICES

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            vert.position = new Vector2(apex.x, apex.y - _mScaleReference.height / 2);  // Middle
            vh.AddVert(vert);

            vert.position = new Vector2(apex.x, apex.y); // Top
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y); // Top Right
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y - _mScaleReference.height / 2); // Bottom Right
            vh.AddVert(vert);

            vert.position = new Vector2(apex.x, apex.y - _mScaleReference.height); // Bottom
            vh.AddVert(vert);

            vert.position = new Vector2(side.x + flipVer, side.y - _mScaleReference.height / 2); // Bottom Left
            vh.AddVert(vert);

            vert.position = new Vector2(side.x + flipVer, side.y); // Top Left
            vh.AddVert(vert);


            // TRIANGLES

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 2, 3);
            vh.AddTriangle(0, 3, 4);
            vh.AddTriangle(0, 4, 5);
            vh.AddTriangle(0, 5, 6);
            vh.AddTriangle(0, 6, 1);
        }

        private void GenerateV18T6(VertexHelper vh, Vector2 apex, Vector2 side)
        {
            float flipVer = -.866f * _mScaleReference.width;


            // VERTICES

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            Vector2 v0 = new Vector2(apex.x, apex.y),
                v1 = new Vector2(side.x, side.y),
                v2 = new Vector2(side.x, side.y - _mScaleReference.height / 2),
                v3 = new Vector2(apex.x, apex.y - _mScaleReference.height),
                v4 = new Vector2(side.x + flipVer, side.y - _mScaleReference.height / 2),
                v5 = new Vector2(side.x + flipVer, side.y),
                o  = new Vector2(apex.x, apex.y - _mScaleReference.height / 2);

            AddVertexOfPositions(vh, vert, 0, o, v0, v1);
            AddVertexOfPositions(vh, vert, 1, o, v1, v2);
            AddVertexOfPositions(vh, vert, 2, o, v2, v3);
            AddVertexOfPositions(vh, vert, 3, o, v3, v4);
            AddVertexOfPositions(vh, vert, 4, o, v4, v5);
            AddVertexOfPositions(vh, vert, 5, o, v5, v0);


            // TRIANGLES

            for (var i = 0; i < 6; i++)
                vh.AddTriangle(3*i, 3*i+1, 3*i+2);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            UpdateScaleReference();


            // GENERATIVE POINTS

            var apex = new Vector2( .5f, 1f );
            var side = new Vector2( .933f, .75f );


            // OFFSET

            Vector2 pivot = rectTransform.pivot;
            apex.x -= pivot.x;
            apex.y -= pivot.y;
            side.x -= pivot.x;
            side.y -= pivot.y;

            apex.x *= _mScaleReference.width;
            apex.y *= _mScaleReference.height;
            side.x *= _mScaleReference.width;
            side.y *= _mScaleReference.height;


            // GENERATOR

            vh.Clear();

            switch (meshType)
            {
                case MeshType.V6T4:
                    GenerateV6T4(vh, apex, side);
                    break;

                case MeshType.V7T6:
                    GenerateV7T6(vh, apex, side);
                    break;

                case MeshType.V18T6:
                    GenerateV18T6(vh, apex, side);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}