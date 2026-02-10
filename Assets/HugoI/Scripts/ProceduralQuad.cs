using System.Collections.Generic;
using UnityEngine;

namespace HugoI.Scripts
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralQuad : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _realtimeGeneration;
        
        [Header("MeshData")]
        [SerializeField] [Range(0f, 10f)] private float _meshSize = 1f;
        [SerializeField] private List<Quad> _quads = new();
        
        [Header("References")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private Mesh _mesh;
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
            if (_realtimeGeneration) GenerateMesh();
        }

        [ContextMenu("Generate Mesh")]
        private void GenerateMesh()
        {
            // CLEAR
            _vertices.Clear();
            _triangles.Clear();
            _uvs.Clear();
            
            _mesh = new Mesh();
            _mesh.name = "ProceduralMesh";

            foreach (var quad in _quads)
            {
                GenerateQuad(quad.pos[0] * _meshSize, quad.pos[1] * _meshSize, quad.pos[2] * _meshSize, quad.pos[3] * _meshSize);

                switch (quad.name)
                {
                    case "front":
                        AddQuadUVs(new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.75f), new Vector2(0.5f, 0.75f), new Vector2(0.5f, 0.5f));
                        break;
                    case "left":
                        AddQuadUVs(new Vector2(0f, 0.5f), new Vector2(0f, 0.75f), new Vector2(0.25f, 0.75f), new Vector2(0.25f, 0.5f));
                        break;
                    case "right":
                        AddQuadUVs(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.75f), new Vector2(0.75f, 0.75f), new Vector2(0.75f, 0.5f));
                        break;
                    case "top":
                        AddQuadUVs(new Vector2(0.25f, 0.75f), new Vector2(0.25f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.75f));
                        break;
                    case "bot":
                        AddQuadUVs(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.25f), new Vector2(0.25f, 0.25f), new Vector2(0.25f, 0.5f));
                        break;
                    case "back":
                        AddQuadUVs(new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0f), new Vector2(0.25f, 0f), new Vector2(0.25f, 0.25f));
                        break;
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

        private void GenerateQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int index = _vertices.Count;
            
            // ADD VERTICES
            _vertices.Add(v1);
            _vertices.Add(v2);
            _vertices.Add(v3);
            _vertices.Add(v4);
            
            // ADD TRIANGLES
            _triangles.Add(index);
            _triangles.Add(index + 1);
            _triangles.Add(index + 2);
            
            _triangles.Add(index);
            _triangles.Add(index + 2);
            _triangles.Add(index + 3);
        }

        private void AddQuadUVs(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
        {
            _uvs.Add(uv1);
            _uvs.Add(uv2);
            _uvs.Add(uv3);
            _uvs.Add(uv4);
        }
    }
}