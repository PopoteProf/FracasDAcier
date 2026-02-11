using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace HugoI.Scripts
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralWall : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _segmentCount = 10;
        [SerializeField] private float _width = 1f;
        [SerializeField] private float _height = 5f;
        
        [Header("References")]
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [Header("Debug")]
        [SerializeField] private bool _displayDebug;
        
        private Mesh _mesh;
        private List<Vector3> _vertices = new();
        private List<Vector3> _verticesFront = new();
        private List<Vector3> _verticesTop = new();
        private List<Vector3> _verticesBack = new();
        private List<int> _triangles = new();
        private List<Vector2> _uvs = new();

        private void Update()
        {
            GenerateWall();
        }

        [ContextMenu("Generate Wall")]
        private void GenerateWall()
        {
            // CLEAR
            _vertices = new();
            _verticesFront = new();
            _verticesTop = new();
            _verticesBack = new();
            _triangles = new();
            _uvs = new();
            
            _mesh = new Mesh();
            _mesh.name = "ProceduralMesh";
            
            AddVertices();
            AddTriangles(_verticesFront);
            AddTriangles(_verticesTop);
            AddTriangles(_verticesBack);
            
            // MESH ASSIGNATION
            _mesh.vertices = _vertices.ToArray();
            _mesh.triangles = _triangles.ToArray();
            _mesh.uv = _uvs.ToArray();
            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();
            
            _meshFilter.mesh = _mesh;
        }

        private void AddVertices()
        {
            for (int i = 0; i <= _segmentCount; i++)
            {
                // Front Wall
                Vector3 pos = _splineContainer.EvaluatePosition(i /  (float)_segmentCount);
                Physics.Raycast(pos, Vector3.down, out RaycastHit frontHit, Mathf.Infinity);
                if (_displayDebug) Debug.DrawLine(frontHit.point, frontHit.point + new Vector3(0f, _height, 0f), Color.red);
                
                // Top Wall
                Vector3 tangent = _splineContainer.EvaluateTangent(i /  (float)_segmentCount);
                Vector3 perpendicular = Vector3.Cross(tangent.normalized, Vector3.up);
                if (_displayDebug) Debug.DrawRay(frontHit.point + new Vector3(0f, _height, 0f), perpendicular * _width, Color.green);
                
                // Back Wall
                Physics.Raycast(frontHit.point + new Vector3(0f, _height, 0f) + perpendicular * _width, 
                    Vector3.down, out RaycastHit backHit, Mathf.Infinity);
                if (_displayDebug) Debug.DrawLine(frontHit.point + new Vector3(0f, _height, 0f) + perpendicular * _width,
                    backHit.point, Color.blue);

                
                // GENERATE VERTICES
                _verticesFront.Add(frontHit.point - transform.position);
                _verticesFront.Add(frontHit.point + new Vector3(0f, _height, 0f) - transform.position);
                _verticesTop.Add(frontHit.point + new Vector3(0f, _height, 0f) - transform.position);
                _verticesTop.Add(frontHit.point + new Vector3(0f, _height, 0f) + perpendicular * _width - transform.position);
                _verticesBack.Add(frontHit.point + new Vector3(0f, _height, 0f) + perpendicular * _width - transform.position);
                _verticesBack.Add(backHit.point - transform.position);
            }
        }

        private void AddTriangles(List<Vector3> sourceVertices)
        {
            for (int i = 0; i < _segmentCount; i++)
            {
                int baseIndex = _vertices.Count;

                _vertices.Add(sourceVertices[i * 2]);
                _vertices.Add(sourceVertices[i * 2 + 1]);
                _vertices.Add(sourceVertices[i * 2 + 2]);
                _vertices.Add(sourceVertices[i * 2 + 3]);

                ProceduralMesh.GenerateQuad(baseIndex, _triangles);
                ProceduralMesh.AddQuadUVs(_uvs, new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1));
            }
        }
    }
}
