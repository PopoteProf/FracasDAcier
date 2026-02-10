using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class ProceduralPillard : MonoBehaviour
{
    [Header("Mesh Settings")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [Header("General Settings")]
    [SerializeField] private Vector3 _origin;
    [SerializeField] [Range(0.01f,50f)]private float _height;
    [SerializeField] [Range(0.01f,50f)]private float _width;
    [SerializeField][Range(0.01f,50f)] private float _lenght;
    [SerializeField] private float _widthPillard;
    [SerializeField] private float _lenghtPillard;
    [SerializeField] private int _loop;
    
    [Header("Debug Settings")]
    [SerializeField] private bool _isRecalculatedMesh = false;
    
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Vector3> _normals ;
    private List<Vector2> _uvs ;
    private List<Vector4>  _tangents ;
    private Mesh mesh;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    private void Update()
    {
        if (_isRecalculatedMesh)
            GenerateMesh();
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();
        mesh.name = "MonProceduralQuad";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs =  new List<Vector2>();
        float Yoffset = _origin.y;
        float distance = 0;
        bool isPillard = true;
        float width = _width;
        float lenght = _lenght;
        
        RaycastHit hit;

        Vector3 origin = transform.position;
        Vector3 direction = Vector3.up;

        if (Physics.Raycast(origin, direction, out hit, 100f))
        {
            distance = hit.distance;
            while (Yoffset < distance && Yoffset + _height < hit.distance + 5f)
            {
                if (Yoffset + _height > hit.distance)
                {
                    isPillard = true;
                }
                
                if (isPillard)
                {
                    width = _widthPillard;
                    lenght = _lenghtPillard;
                    isPillard = false;
                }
                else
                {
                    width = _width;
                    lenght = _lenght;
                }
                
                float halfLength = lenght * 0.5f;
                float halfWidth = width * 0.5f;

                //Face nb 1
                AddQuad(
                    new Vector3(-halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, -halfWidth)
                );
                AddQuadUV(new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f),new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f));

                //Face nb 4
                AddQuad(
                    new Vector3(-halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, halfWidth),
                    new Vector3(-halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, -halfWidth)
                );
                AddQuadUV(new Vector2(0,0.5f),new Vector2(0,0.75f),new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f));

                //Face nb 3
                AddQuad(
                    new Vector3(halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, halfWidth)
                );
                AddQuadUV(new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f),new Vector2(0.75f,0.5f),new Vector2(0.75f,0.75f));

                //Face nb 5
                AddQuad(
                    new Vector3(-halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, halfWidth)
                );
                AddQuadUV(new Vector2(0.25f,0.75f),new Vector2(0.25f,1),new Vector2(0.5f,0.75f),new Vector2(0.5f,1));

                //Face nb 2 
                AddQuad(
                    new Vector3(-halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(-halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, -halfWidth)
                );
                AddQuadUV(new Vector2(0.25f,0.25f),new Vector2(0.25f,0.5f),new Vector2(0.5f,0.25f),new Vector2(0.5f,0.5f));

                //Face nb 6
                AddQuad(
                    new Vector3(halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, halfWidth),
                    new Vector3(-halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, halfWidth)
                );
                AddQuadUV(new Vector2(0.5f,0.25f),new Vector2(0.5f,0),new Vector2(0.25f,0.25f),new Vector2(0.25f,0));
            
                Yoffset += _height;
            }
        }
        else
        {
            for (int i = 0; i < _loop; i++)
            {
                float halfLength = lenght * 0.5f;
                float halfWidth = width * 0.5f;

                //Face nb 1
                AddQuad(
                    new Vector3(-halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, -halfWidth)
                );
                AddQuadUV(new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f),new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f));

                //Face nb 4
                AddQuad(
                    new Vector3(-halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, halfWidth),
                    new Vector3(-halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, -halfWidth)
                );
                AddQuadUV(new Vector2(0,0.5f),new Vector2(0,0.75f),new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f));

                //Face nb 3
                AddQuad(
                    new Vector3(halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, halfWidth)
                );
                AddQuadUV(new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f),new Vector2(0.75f,0.5f),new Vector2(0.75f,0.75f));

                //Face nb 5
                AddQuad(
                    new Vector3(-halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, -halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, halfWidth)
                );
                AddQuadUV(new Vector2(0.25f,0.75f),new Vector2(0.25f,1),new Vector2(0.5f,0.75f),new Vector2(0.5f,1));

                //Face nb 2 
                AddQuad(
                    new Vector3(-halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(-halfLength, 0 + Yoffset, -halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(halfLength, 0 + Yoffset, -halfWidth)
                );
                AddQuadUV(new Vector2(0.25f,0.25f),new Vector2(0.25f,0.5f),new Vector2(0.5f,0.25f),new Vector2(0.5f,0.5f));

                //Face nb 6
                AddQuad(
                    new Vector3(halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(halfLength, 1 * _height + Yoffset, halfWidth),
                    new Vector3(-halfLength, 0 + Yoffset, halfWidth),
                    new Vector3(-halfLength, 1 * _height + Yoffset, halfWidth)
                );
                AddQuadUV(new Vector2(0.5f,0.25f),new Vector2(0.5f,0),new Vector2(0.25f,0.25f),new Vector2(0.25f,0));

            
                Yoffset += _height;
            }
        }
        
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.uv = _uvs.ToArray();
        
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        _meshFilter.mesh = mesh;
    }
    
    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int index = _vertices.Count;
        
        _vertices.Add(_origin + v1);
        _vertices.Add(_origin +v2);
        _vertices.Add(_origin +v3);
        _vertices.Add(_origin +v4);
        
        _triangles.Add(index);
        _triangles.Add(index + 1);
        _triangles.Add(index + 2);
        
        _triangles.Add(index + 2);
        _triangles.Add(index + 1);
        _triangles.Add(index + 3);
    }
    
    private void AddQuadUV(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
    {
        _uvs.Add(uv1);
        _uvs.Add(uv2);
        _uvs.Add(uv3);
        _uvs.Add(uv4);
    }
}
