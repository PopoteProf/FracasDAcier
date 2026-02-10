using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class ProceduralQuad : MonoBehaviour
{
    [Header("Mesh Settings")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [Header("General Settings")]
    [SerializeField] [Range(0.1f, 20f)]private float _scaleMesh;
    
    [Header("Debug Settings")]
    [SerializeField] private bool _isRecalculated = false;
    
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

        // GenerateMesh();
    }

    private void Update()
    {
        if (_isRecalculated)
            GenerateQuadMesh();
    }

    [ContextMenu("Generate Mesh")]
    private void GenerateMesh()
    {
        mesh = new Mesh();
        mesh.name = "MonProceduralQuad";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs =  new List<Vector2>();
        
        //0
        _vertices.Add(new Vector3(0, 0, 0));
        //1
        _vertices.Add(new Vector3(0, 1, 0));
        //2
        _vertices.Add(new Vector3(1, 0, 0));
        //3
        _vertices.Add(new Vector3(1, 1, 0));
        //4
        _vertices.Add(new Vector3(0, 0, 1));
        //5
        _vertices.Add(new Vector3(0, 1, 1));
        //6
        _vertices.Add(new Vector3(1, 0, 1));
        //7
        _vertices.Add(new Vector3(1, 1, 1));
        
        // _triangles.Add(0);
        // _triangles.Add(1);
        // _triangles.Add(2);
        //
        // _triangles.Add(2);
        // _triangles.Add(1);
        // _triangles.Add(3);
        
        _triangles.Add(4);
        _triangles.Add(5);
        _triangles.Add(0);
        
        _triangles.Add(0);
        _triangles.Add(5);
        _triangles.Add(1);
        
        _triangles.Add(4);
        _triangles.Add(7);
        _triangles.Add(5);
        
        _triangles.Add(6);
        _triangles.Add(7);
        _triangles.Add(4);
        
        _triangles.Add(2);
        _triangles.Add(3);
        _triangles.Add(6);
        
        _triangles.Add(6);
        _triangles.Add(3);
        _triangles.Add(7);
        
        _triangles.Add(3);
        _triangles.Add(1);
        _triangles.Add(7);
        
        _triangles.Add(7);
        _triangles.Add(1);
        _triangles.Add(5);

        _triangles.Add(0);
        _triangles.Add(2);
        _triangles.Add(4);
        
        _triangles.Add(4);
        _triangles.Add(2);
        _triangles.Add(6);
        
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        _normals.Add(Vector3.back);
        
        _uvs.Add(new Vector2(0, 0));
        _uvs.Add(new Vector2(0, 1));
        _uvs.Add(new Vector2(1, 0));
        _uvs.Add(new Vector2(1, 1));
        
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        _tangents.Add(new Vector4(1, 0, 0, -1));
        
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.uv = _uvs.ToArray();
        
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        _meshFilter.mesh = mesh;
    }

    [ContextMenu("Generate Quad Mesh")]
    private void GenerateQuadMesh()
    {
        mesh = new Mesh();
        mesh.name = "MonProceduralQuad";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs =  new List<Vector2>();
        
        //Face nb 1
        AddQuad(new Vector3(0, 0, 0),new Vector3(0, 1, 0),new Vector3(1, 0, 0),new Vector3(1, 1, 0));
        AddQuadUV(new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f),new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f));
        
        //Face nb 4
        AddQuad(new Vector3(0, 0, 1),new Vector3(0, 1, 1),new Vector3(0, 0, 0),new Vector3(0, 1, 0));
        AddQuadUV(new Vector2(0,0.5f),new Vector2(0,0.75f),new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f));
        
        //Face nb 3
        AddQuad(new Vector3(1, 0, 0),new Vector3(1, 1, 0),new Vector3(1, 0, 1),new Vector3(1, 1, 1));
        AddQuadUV(new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f),new Vector2(0.75f,0.5f),new Vector2(0.75f,0.75f));
        
        //Face nb 5
        AddQuad(new Vector3(0, 1, 0),new Vector3(0, 1, 1),new Vector3(1, 1, 0),new Vector3(1, 1, 1));
        AddQuadUV(new Vector2(0.25f,0.75f),new Vector2(0.25f,1),new Vector2(0.5f,0.75f),new Vector2(0.5f,1));
        
        //Face nb 2 
        AddQuad(new Vector3(0, 0, 1),new Vector3(0, 0, 0),new Vector3(1, 0, 1),new Vector3(1, 0, 0));
        AddQuadUV(new Vector2(0.25f,0.25f),new Vector2(0.25f,0.5f),new Vector2(0.5f,0.25f),new Vector2(0.5f,0.5f));
        
        //Face nb 6
        AddQuad(new Vector3(1, 0, 1),new Vector3(1, 1, 1),new Vector3(0, 0, 1),new Vector3(0, 1, 1));
        AddQuadUV(new Vector2(0.5f,0.25f),new Vector2(0.5f,0),new Vector2(0.25f,0.25f),new Vector2(0.25f,0));
        
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
        
        _vertices.Add(v1 * _scaleMesh);
        _vertices.Add(v2* _scaleMesh);
        _vertices.Add(v3* _scaleMesh);
        _vertices.Add(v4* _scaleMesh);
        
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
