using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shaders
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
            V6_T4,
            V7_T6,
            V18_T6
        }

        public MeshType meshType;

        public bool useTriUniqueColors;
        public Color[] colors;

        private Rect m_scaleReference = Rect.zero;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            bool colorsFlag = meshType == MeshType.V18_T6 && useTriUniqueColors;
            Array.Resize(ref colors, colorsFlag ? 6 : 0);
            UpdateGeometry();
        }
#endif


        // SCALE REFERENCE

        private void UpdateScaleReference()
        {
            float w = 1.1547f * rectTransform.rect.width,
                h = rectTransform.rect.height;

            switch (scalingType)
            {
                case ScaleType.Regular:
                    if (w > h)
                        m_scaleReference.size = new Vector2(h, h);
                    else
                        m_scaleReference.size = new Vector2(w, w);
                    break;

                case ScaleType.FitToWidth:
                    m_scaleReference.size = new Vector2(w, w);
                    break;

                case ScaleType.FitToHeight:
                    m_scaleReference.size = new Vector2(h, h);
                    break;

                case ScaleType.Fill:
                    m_scaleReference.size = new Vector2(w, h);
                    break;

                default:
                    m_scaleReference = rectTransform.rect;
                    break;
            }
        }


        // VERTEX GENERATORS

        private void AddVertexOfPositions(VertexHelper vh, UIVertex vert, int color, params Vector2[] positions)
        {
            if (useTriUniqueColors)
                vert.color = colors[color];
            AddVertexOfPositions(vh, vert, positions);
        }
        private void AddVertexOfPositions(VertexHelper vh, UIVertex vert, params Vector2[] positions)
        {
            foreach (Vector2 pos in positions)
            {
                vert.position = pos;
                vh.AddVert(vert);
            }
        }

        private void GenerateV6T4(VertexHelper vh, Vector2 apex, Vector2 side)
        {
            float flipVer = -.866f * m_scaleReference.width;


            // VERTICES

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            vert.position = new Vector2(apex.x, apex.y); // Top
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y); // Top Right
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y - m_scaleReference.height / 2); // Bottom Right
            vh.AddVert(vert);

            vert.position = new Vector2(apex.x, apex.y - m_scaleReference.height); // Bottom
            vh.AddVert(vert);

            vert.position = new Vector2(side.x + flipVer, side.y - m_scaleReference.height / 2); // Bottom Left
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
            float flipVer = -.866f * m_scaleReference.width;


            // VERTICES

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            vert.position = new Vector2(apex.x, apex.y - m_scaleReference.height / 2);  // Middle
            vh.AddVert(vert);

            vert.position = new Vector2(apex.x, apex.y); // Top
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y); // Top Right
            vh.AddVert(vert);

            vert.position = new Vector2(side.x, side.y - m_scaleReference.height / 2); // Bottom Right
            vh.AddVert(vert);

            vert.position = new Vector2(apex.x, apex.y - m_scaleReference.height); // Bottom
            vh.AddVert(vert);

            vert.position = new Vector2(side.x + flipVer, side.y - m_scaleReference.height / 2); // Bottom Left
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
            float flipVer = -.866f * m_scaleReference.width;


            // VERTICES

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            Vector2 v0 = new Vector2(apex.x, apex.y),
                v1 = new Vector2(side.x, side.y),
                v2 = new Vector2(side.x, side.y - m_scaleReference.height / 2),
                v3 = new Vector2(apex.x, apex.y - m_scaleReference.height),
                v4 = new Vector2(side.x + flipVer, side.y - m_scaleReference.height / 2),
                v5 = new Vector2(side.x + flipVer, side.y),
                o  = new Vector2(apex.x, apex.y - m_scaleReference.height / 2);

            AddVertexOfPositions(vh, vert, 0, o, v0, v1);
            AddVertexOfPositions(vh, vert, 1, o, v1, v2);
            AddVertexOfPositions(vh, vert, 2, o, v2, v3);
            AddVertexOfPositions(vh, vert, 3, o, v3, v4);
            AddVertexOfPositions(vh, vert, 4, o, v4, v5);
            AddVertexOfPositions(vh, vert, 5, o, v5, v0);


            // TRIANGLES

            for (int i = 0; i < 6; i++)
                vh.AddTriangle(3*i, 3*i+1, 3*i+2);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            UpdateScaleReference();


            // GENERATIVE POINTS

            Vector2 apex = new Vector2( .5f, 1f );
            Vector2 side = new Vector2( .933f, .75f );


            // OFFSET

            apex.x -= rectTransform.pivot.x;
            apex.y -= rectTransform.pivot.y;
            side.x -= rectTransform.pivot.x;
            side.y -= rectTransform.pivot.y;

            apex.x *= m_scaleReference.width;
            apex.y *= m_scaleReference.height;
            side.x *= m_scaleReference.width;
            side.y *= m_scaleReference.height;


            // GENERATOR

            vh.Clear();

            switch (meshType)
            {
                case MeshType.V6_T4:
                    GenerateV6T4(vh, apex, side);
                    break;

                case MeshType.V7_T6:
                    GenerateV7T6(vh, apex, side);
                    break;

                case MeshType.V18_T6:
                    GenerateV18T6(vh, apex, side);
                    break;
            }
        }
    }
}