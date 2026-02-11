using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralCube : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    [Range(1f, 50f)]
    public float SizeMultiplier = 1f;

    public bool GeneratingUpdate = true;
    
    public Vector3 A = new Vector3(0, 0, 0);
    public Vector3 B = new Vector3(0, 1, 0);
    public Vector3 C = new Vector3(1, 0, 0);
    public Vector3 D = new Vector3(1, 1, 0);
    
    public Vector3 E = new Vector3(1, 0, 0);
    public Vector3 F = new Vector3(1, 1, 0);
    public Vector3 G = new Vector3(1, 0, 1);
    public Vector3 H = new Vector3(1, 1, 1);
    
    public Vector3 I = new Vector3(1, 0, 1);
    public Vector3 J = new Vector3(1, 1, 1);
    public Vector3 K = new Vector3(0, 0, 1);
    public Vector3 L = new Vector3(0, 1, 1);
    
    public Vector3 M = new Vector3(0, 0, 1);
    public Vector3 N = new Vector3(0, 1, 1);
    public Vector3 O = new Vector3(0, 0, 0);
    public Vector3 P = new Vector3(0, 1, 0);
    
    public Vector3 Q = new Vector3(0, 1, 0);
    public Vector3 R = new Vector3(0, 1, 1);
    public Vector3 S = new Vector3(1, 1, 0);
    public Vector3 T = new Vector3(1, 1, 1);
    
    public Vector3 U = new Vector3(0, 0, 1);
    public Vector3 V = new Vector3(0, 0, 0);
    public Vector3 W = new Vector3(1, 0, 1);
    public Vector3 X = new Vector3(1, 0, 0);
    
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private List<Vector2> _uvs = new List<Vector2>();
    private Mesh _mesh;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        GenerateMesh();
    }

    private void Update()
    {
        if (GeneratingUpdate)
        {
            GenerateMesh();
        }
    }

    [ContextMenu("Generate Mesh")]
    private void GenerateMesh()
    {
        _mesh = new Mesh();
        _mesh.name = "ProceduralCube";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs = new List<Vector2>();
        
        AddQuad(A,B,C,D);
        AddQuadUVs(new Vector2(0.25f,0.5f), new Vector2(0.25f,0.75f), new Vector2(0.5f,0.5f), new Vector2(0.5f,0.75f));
        AddQuad(E,F,G,H);
        AddQuadUVs(new Vector2(0.5f,0.5f), new Vector2(0.5f,0.75f), new Vector2(0.75f,0.5f), new Vector2(0.75f,0.75f));
        AddQuad(I,J,K,L);
        AddQuadUVs(new Vector2(0.25f, 0f), new Vector2(0.25f, 0.25f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.25f));
        AddQuad(M,N,O,P);
        AddQuadUVs(new Vector2(0f,0.5f), new Vector2(0f,0.75f), new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.75f));
        AddQuad(Q,R,S,T);
        AddQuadUVs(new Vector2(0.25f, 0.75f), new Vector2(0.25f, 1f), new Vector2(0.5f, 0.75f), new Vector2(0.5f, 1f));
        AddQuad(U,V,W,X);
        AddQuadUVs(new Vector2(0.25f, 0.25f), new Vector2(0.25f, 0.5f), new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.5f));
        
        for (int i = 0; i < _vertices.Count; i++)
        {
            _vertices[i] = _vertices[i] * SizeMultiplier;
        }
        // Generate mesh
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.RecalculateNormals();
        _meshFilter.mesh = _mesh;

    }

    private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        int index = _vertices.Count;
        _vertices.Add(a);
        _vertices.Add(b);
        _vertices.Add(c);
        _vertices.Add(d);
        
        _triangles.Add(index);
        _triangles.Add(index + 1);
        _triangles.Add(index + 2);
        
        _triangles.Add(index + 1);
        _triangles.Add(index + 3);
        _triangles.Add(index + 2);
    }

    private void AddQuadUVs(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        _uvs.Add(a);
        _uvs.Add(b);
        _uvs.Add(c);
        _uvs.Add(d);
    }
    
}
