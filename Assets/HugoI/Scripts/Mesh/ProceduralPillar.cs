using System.Collections.Generic;
using UnityEngine;

namespace HugoI.Scripts.Mesh
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralPillar : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _realtimeGeneration;
        [SerializeField] private bool _isAdaptable;
        [SerializeField] private bool _haveBases;

        [Header("MeshData")]
        [SerializeField] private Vector3 _origin;
        [SerializeField] private float _length = 1f;
        [SerializeField] private float _width = 1f;
        [SerializeField] private float _height = 1f;
        [SerializeField] private Vector3 _basesSize = new(1.5f, 1.5f, 1.5f);
        [SerializeField] private int _loop = 1;
        [SerializeField] private List<Quad> _quads = new();
        
        [Header("References")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [Header("Settings")]
        [SerializeField] private bool _displayDebug;
        
        private UnityEngine.Mesh _mesh;
        private List<Vector3> _vertices = new();
        private List<int> _triangles = new();
        private List<Vector2> _uvs = new();

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (_isAdaptable)
            {
                bool hitSomething = Physics.Raycast(transform.position, transform.up, out RaycastHit hit, Mathf.Infinity);
                if (_displayDebug) Debug.DrawLine(transform.position, hit.point, Color.red);
                
                if (hitSomething)
                {
                    float distance = Vector3.Distance(transform.position, hit.point);
                    
                    _loop = Mathf.RoundToInt(distance / _height);
                }
            }
            
            if (_realtimeGeneration) GeneratePillarMesh();
        }

        [ContextMenu("Generate Mesh")]
        private void GeneratePillarMesh()
        {
            // INITIALIZE
            _vertices = new();
            _triangles = new();
            _uvs = new();
            
            _mesh = new UnityEngine.Mesh();
            _mesh.name = "ProceduralMesh";

            float yOffset = 0f;
            
            for (int i = 0; i < _loop; i++)
            {
                yOffset = i * _height;
                
                foreach (var quad in _quads)
                {
                    Vector3 v1 = new Vector3(quad.pos[0].x * _length, quad.pos[0].y * _height + yOffset, quad.pos[0].z * _width) + _origin - new Vector3(_length / 2f, 0f, _width / 2f);
                    Vector3 v2 = new Vector3(quad.pos[1].x * _length, quad.pos[1].y * _height + yOffset, quad.pos[1].z * _width) + _origin - new Vector3(_length / 2f, 0f, _width / 2f);
                    Vector3 v3 = new Vector3(quad.pos[2].x * _length, quad.pos[2].y * _height + yOffset, quad.pos[2].z * _width) + _origin - new Vector3(_length / 2f, 0f, _width / 2f);
                    Vector3 v4 = new Vector3(quad.pos[3].x * _length, quad.pos[3].y * _height + yOffset, quad.pos[3].z * _width) + _origin - new Vector3(_length / 2f, 0f, _width / 2f);
                
                    ProceduralMesh.GenerateQuadAndAddVertices(_vertices, _triangles, v1, v2, v3, v4);
                    ProceduralMesh.AddQuadUVs(_uvs, quad.uvs[0], quad.uvs[1], quad.uvs[2], quad.uvs[3]);

                    if (_haveBases)
                    {
                        if (i == 0)
                        {
                            GenerateBase(quad, yOffset);
                        }
                        else if (i == _loop - 1)
                        {
                            GenerateBase(quad, yOffset + _height);
                        }
                    }
                }
            }
            
            // MESH ASSIGNATION
            _mesh.vertices = _vertices.ToArray();
            _mesh.triangles = _triangles.ToArray();
            _mesh.uv = _uvs.ToArray();
            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();
            
            _meshFilter.mesh = _mesh;
        }

        private void GenerateBase(Quad quad, float yOffset)
        {
            Vector3 v1 = new Vector3(quad.pos[0].x * _length * _basesSize.x, quad.pos[0].y * _height * _basesSize.y + yOffset, quad.pos[0].z * _width * _basesSize.z) + _origin - new Vector3(_length * _basesSize.x / 2f, _height * _basesSize.y / 2f, _width * _basesSize.z / 2f);
            Vector3 v2 = new Vector3(quad.pos[1].x * _length * _basesSize.x, quad.pos[1].y * _height * _basesSize.y + yOffset, quad.pos[1].z * _width * _basesSize.z) + _origin - new Vector3(_length * _basesSize.x / 2f, _height * _basesSize.y / 2f, _width * _basesSize.z / 2f);
            Vector3 v3 = new Vector3(quad.pos[2].x * _length * _basesSize.x, quad.pos[2].y * _height * _basesSize.y + yOffset, quad.pos[2].z * _width * _basesSize.z) + _origin - new Vector3(_length * _basesSize.x / 2f, _height * _basesSize.y / 2f, _width * _basesSize.z / 2f);
            Vector3 v4 = new Vector3(quad.pos[3].x * _length * _basesSize.x, quad.pos[3].y * _height * _basesSize.y + yOffset, quad.pos[3].z * _width * _basesSize.z) + _origin - new Vector3(_length * _basesSize.x / 2f, _height * _basesSize.y / 2f, _width * _basesSize.z / 2f);
                
            ProceduralMesh.GenerateQuadAndAddVertices(_vertices, _triangles, v1, v2, v3, v4);
            ProceduralMesh.AddQuadUVs(_uvs, quad.uvs[0], quad.uvs[1], quad.uvs[2], quad.uvs[3]);
        }
    }
}