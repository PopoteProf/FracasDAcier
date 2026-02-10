using System;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralQuad : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    [SerializeField] private List<Vector3> _vertices = new List<Vector3>();
    [SerializeField] private List<Vector3> _normals = new List<Vector3>();
    [SerializeField] private List<Vector2> _uvs = new List<Vector2>();
    [SerializeField] private List<Vector4> _tangents = new List<Vector4>();
    [SerializeField] private List<int> _triangles = new List<int>();
    
    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        GenerateMesh();
    }

    [ContextMenu("GenrateMesh")]
    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "MonNouveauMesh";
        
        _meshFilter.mesh = mesh;
        
        
        Vector3 a = new Vector3(0, 0, 0); // 0
        Vector3 b = new Vector3(0, 1, 0); // 1
        Vector3 c = new Vector3(1, 0, 0); // 2
        Vector3 d = new Vector3(1, 1, 0); // 3 
        Vector3 e = new Vector3(0, 0, 1); // 4
        Vector3 f = new Vector3(0, 1, 1); // 5
        Vector3 g = new Vector3(1, 0, 1); // 6 
        Vector3 h = new Vector3(1, 1, 1); // 7
        
        // avec triangle
        // AddTriangle(a,b,c);
        // AddTriangle(b,d,c);
        // AddTriangle(d,b,f);
        // AddTriangle(f,h,d);
        // AddTriangle(a,e,f);
        // AddTriangle(f,b,a);
        // AddTriangle(h,f,e);
        // AddTriangle(g,h,e);
        // AddTriangle(c,d,g);
        // AddTriangle(g,d,h);
        // AddTriangle(e,a,g);
        // AddTriangle(g,a,c);
        
        // avec caré
        AddQuad(a,b,d,c);
        AddUvToQuad(new Vector2(0.25f, 0.25f), new Vector2(0.25f, 0.5f), new Vector2(0.5f,0.5f), new Vector2(0.5f, 0.25f));
        AddQuad(b,f,h,d);
        AddUvToQuad(new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.75f), new Vector2(0.5f,0.75f), new Vector2(0.5f, 0.5f));
        AddQuad(c,d,h,g);
        AddUvToQuad(new Vector2(0.75f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f,0.75f), new Vector2(0.75f, 0.75f));
        AddQuad(f,b,a,e);
        AddUvToQuad(new Vector2(0.25f, 0.75f), new Vector2(0.25f, 0.5f), new Vector2(0f,0.5f), new Vector2(0f, 0.75f));
        AddQuad(h,f,e,g);
        AddUvToQuad(new Vector2(0.5f, 0.75f), new Vector2(0.25f, 0.75f), new Vector2(0.25f,1f), new Vector2(0.5f, 1f));
        AddQuad(a,c,g,e);
        AddUvToQuad(new Vector2(0.25f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f,0.25f), new Vector2(0.25f, 0.25f));
        
      
       
        
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.uv = _uvs.ToArray();
        
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        _meshFilter.mesh = mesh;
    }

    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int index = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        
        _triangles.Add(index);
        _triangles.Add(index + 1);
        _triangles.Add(index + 2);
    }

    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int index = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _vertices.Add(v4);
        
        _triangles.Add(index);
        _triangles.Add(index + 1);
        _triangles.Add(index + 2);
        
        _triangles.Add(index);
        _triangles.Add(index + 2);
        _triangles.Add(index + 3);
    }

    public void AddUvToQuad(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
    {
        _uvs.Add(v1);
        _uvs.Add(v2);
        _uvs.Add(v3);
        _uvs.Add(v4);
    }
}
