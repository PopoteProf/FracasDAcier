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
        GenrateMesh();
    }

    [ContextMenu("GenrateMesh")]
    private void GenrateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "MonNouveauMesh";
        
        _meshFilter.mesh = mesh;
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector4> tangents = new List<Vector4>();
        
        Vector3 a = new Vector3(0, 0, 0); // 0
        Vector3 b = new Vector3(0, 1, 0); // 1
        Vector3 c = new Vector3(1, 0, 0); // 2
        Vector3 d = new Vector3(1, 1, 0); // 3 
        Vector3 e = new Vector3(0, 0, 1); // 4
        Vector3 f = new Vector3(0, 1, 1); // 5
        Vector3 g = new Vector3(1, 0, 1); // 6 
        Vector3 h = new Vector3(1, 1, 1); // 7
        
        AddTriangle(a,b,c);
        AddTriangle(b,d,c);
        AddTriangle(d,b,f);
        AddTriangle(f,h,d);
        AddTriangle(a,e,f);
        AddTriangle(f,b,a);
        AddTriangle(h,f,e);
        AddTriangle(g,h,e);
        AddTriangle(c,d,g);
        AddTriangle(g,d,h);
        AddTriangle(e,a,g);
        AddTriangle(g,a,c);
        
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1)); 
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.uv = uvs.ToArray();
        
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
        AddTriangle(v1, v2, v4);
        AddTriangle(v4, v2, v3);
    }
}
