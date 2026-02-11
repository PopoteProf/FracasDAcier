using System.Collections.Generic;
using UnityEngine;

namespace HugoI.Scripts.Mesh
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralBridge : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _realtimeGeneration;
        
        [Header("MeshData")]
        [SerializeField] private float _width = 1f;
        
        [Header("References")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [Header("Debug")]
        [SerializeField] private bool _displayDebug;
        
        private UnityEngine.Mesh _mesh;
        private List<Vector3> _vertices = new();
        private List<int> _triangles = new();
        private List<Vector2> _uvs = new();

        private void Update()
        {
            if (_realtimeGeneration) GenerateBridgeMesh();
        }

        private void GenerateBridgeMesh()
        {
            // INITIALIZE
            _vertices = new();
            _triangles = new();
            _uvs = new();
            
            _mesh = new UnityEngine.Mesh();
            _mesh.name = "ProceduralMesh";
            
            AddVertices();
            ProceduralMesh.GenerateQuad(0, _triangles);
            
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
            Physics.Raycast(transform.position, transform.forward, out RaycastHit frontHit, Mathf.Infinity);
            Physics.Raycast(transform.position, -transform.forward, out RaycastHit backHit, Mathf.Infinity);
            if (_displayDebug)
            {
                Debug.DrawLine(transform.position, frontHit.point, Color.red);
                Debug.DrawLine(transform.position, backHit.point, Color.red);
            }
            
            Vector3 v1 = backHit.point - transform.position + transform.right * _width;
            Vector3 v2 =  backHit.point - transform.position - transform.right * _width;
            Vector3 v3 =  frontHit.point - transform.position + transform.right * _width;
            Vector3 v4 =  frontHit.point - transform.position - transform.right * _width;
            v1 = transform.InverseTransformVector(v1);
            v2 = transform.InverseTransformVector(v2);
            v3 = transform.InverseTransformVector(v3);
            v4 = transform.InverseTransformVector(v4);

            
            _vertices.Add(v1);
            _vertices.Add(v2);
            _vertices.Add(v3);
            _vertices.Add(v4);
        }
    }
}
