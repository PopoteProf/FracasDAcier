using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralPillar : MonoBehaviour
{
    public bool GeneratingUpdate = true;
    public float Width = 1f;
    public float Height = 1f;
    public int Segments = 1;
    
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;

    private Vector3 _origin;
    
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private List<Vector2> _uvs = new List<Vector2>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _origin = Vector3.zero;

        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        
        GenerateMesh();
    }


    void Update()
    {
        if (GeneratingUpdate)GenerateMesh();
    }
    private void GenerateMesh()
    {
        
        _mesh = new Mesh();
        _mesh.name = "ProceduralPillar";
        _meshFilter.mesh = _mesh;
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        Debug.DrawRay(transform.position, Vector3.up, Color.red);
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, Mathf.Infinity))
        {
            
            Debug.DrawLine(transform.position,hit.point, Color.green);
            
            float distance = hit.distance;
            int segmentCount = Mathf.CeilToInt(distance / Height);
            
            //Segments = segmentCount;
            for (int i = 0; i < segmentCount; i++)
            {
                GenerateSegment(_origin + new Vector3(0, i * Height, 0));
            }
        }
        else
        {
            for (int i = 0; i < Segments; i++)
            {
                GenerateSegment(_origin + new Vector3(0, i * Height, 0));
            }
        }
       // GenerateSegment(_origin+offset);
       
       _mesh.vertices = _vertices.ToArray();
       _mesh.triangles = _triangles.ToArray();
       _mesh.RecalculateNormals();
        
    }

    private void GenerateSegment(Vector3 origin)
    {
        Vector3 A = origin + new Vector3(-Width / 2, 0, -Width / 2);
        Vector3 B = origin + new Vector3(-Width / 2, 0, Width / 2);
        Vector3 C = origin + new Vector3(Width / 2, 0, -Width / 2);
        Vector3 D = origin + new Vector3(Width / 2, 0, Width / 2);
        
        
        Vector3 E = A + new Vector3(0, Height, 0);
        Vector3 F = B + new Vector3(0, Height, 0);
        Vector3 G = C + new Vector3(0, Height, 0);
        Vector3 H = D + new Vector3(0, Height, 0);
        
        AddQuad(A,E,C,G);
        AddQuad(C,G,D,H);
        AddQuad(D,H,B,F);
        AddQuad(B,F,A,E);
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
}
