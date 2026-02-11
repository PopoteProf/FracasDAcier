using System;
using System.Collections.Generic;
using UnityEngine;

    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
public class ProceduralQuad : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [SerializeField] private Vector3[] _verts;
    [SerializeField] private Vector3[] _verts1;
    [SerializeField] private Vector3[] _verts2;
    [SerializeField] private Vector3[] _verts3;
    [SerializeField] private Vector3[] _verts4;
    [SerializeField] private Vector3[] _verts5;
    
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Vector3> _normals;
    private List<Vector2> _uvs;
    private List<Vector4> _tangents;

    [SerializeField] private float _rayDistance = 100f;
    private readonly LayerMask _hitLayers = -1; // Default to all layers
    [SerializeField] private float _pillarWidth = 1f;
    [SerializeField] private float _blockHeight = 0.5f; // Height of each block segment
    
    private readonly Dictionary<string, Vector3> _directions = new()
    {
        { "left", Vector3.left },
        { "right", Vector3.right },
        { "up", Vector3.up },
        { "down", Vector3.down },

    };
    

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        _mesh = new Mesh() ;
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _normals = new List<Vector3>();
        _uvs = new List<Vector2>();
        _tangents = new List<Vector4>();
        
        _mesh.name = "Procedural Pillar";
        
        //Pillar(new Vector3(1,1,2), 2, 5, 10);
        
        // AddQuad(_verts);
        // AddQuad(_verts1);
        // AddQuad(_verts2);
        // AddQuad(_verts3);
        // AddQuad(_verts4);
        // AddQuad(_verts5);
        
        //GenerateQuad(_mesh, Vector3.up, 3);
        //GenerateTriangles(_mesh, Vector3.right, 1);
        //GenerateMesh();
    }

    [ContextMenu("Generate Mesh")]
    private void GenerateMesh()
    {
        Mesh mesh = new()
        {
            name = "Procedural Quad"
        };

        // We add vertices to prepare to make the triangles (UVs)
        List<Vector3> vertices = new()
        {
            // Front
            new Vector3(0, 0, 0), // 0 BL
            new Vector3(0, 1, 0), // 1 TL
            new Vector3(1, 0, 0), // 2 BR
            
            new Vector3(1, 1, 0), // 3 TR
            
            // Back
            new Vector3(0, 0, 1), // 4 BR
            new Vector3(0, 1, 1), // 5 TR
            new Vector3(1, 0, 1), // 6 BL

            new Vector3(1, 1, 1), // 7 TL
        };

        // Index of the vertices, 
        List<int> triangles = new()
        {
            // Front
            0,
            1,
            2,
            
            3,
            2,
            1,
            
            // Back
            4,
            6,
            5,

            7,
            5,
            6,
            
            // L Side
            0,
            4,
            1,

            5,
            1,
            4,
            
            // R Side
            2,
            3,
            6,

            7,
            6,
            3,
            
            // Top
            3,
            1,
            7,

            5,
            7,
            1,
            
            // Bottom
            2,
            6,
            0,

            4,
            0,
            6,
        };

        // Normals allow for light to affect the object
        List<Vector3> normals = new()
        {

            Vector3.back,
            Vector3.back,
            Vector3.back,
            
            Vector3.back,
            Vector3.back,
            Vector3.back,

            Vector3.back,
            Vector3.back,
            Vector3.back,

            Vector3.back,
            Vector3.back,
            Vector3.back,

        };

        List<Vector2> uvs = new()
        {

            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),
            
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),

            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),

            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),

        };

        List<Vector4> tangents = new()
        {
            // Front
            new Vector4(1, 0, 0, 1),
            new Vector4(1, 0, 0, -1),
            new Vector4(1, 0, 0, 1),

            new Vector4(1, 0, 0, 1),
            new Vector4(1, 0, 0, -1),
            new Vector4(1, 0, 0, 1),
            // Back
            new Vector4(1, 0, 0, 1),
            new Vector4(1, 0, 0, -1),
            new Vector4(1, 0, 0, 1),

            new Vector4(1, 0, 0, 1),
            new Vector4(1, 0, 0, -1),
            new Vector4(1, 0, 0, 1),

        };
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.tangents = tangents.ToArray();

        _meshFilter.mesh = mesh;
        
    }

    [ContextMenu("Generate Triangle")]
    private void GenerateTriangles(Mesh mesh, Vector3 direction, int length)
    {
        // If I add a triangle depending on a normalized direction vector so we can pick a side to iterate upon.
        // L,RT,B,B (no front since the triangle would be flipped to the origin).
        
        _vertices.Add(new Vector3(0, 0, 0));

        for (int i = 0; i <= length; i++)
        {
            _vertices.Add(new Vector3(0, 1, 0) + direction * i);
            _vertices.Add(new Vector3(1, 0, 0) + direction * i);
            
            // Up
            _triangles.Add(1 + 2*i);
            _triangles.Add(3 + 2*i);
            _triangles.Add(2 + 2*i);
            // Down
            _triangles.Add(0 + 2*i);
            _triangles.Add(1 + 2*i);
            _triangles.Add(2 + 2*i);
            
            _normals.Add(Vector3.back);
            _normals.Add(Vector3.back);
            _normals.Add(Vector3.back);
        
            _uvs.Add(new Vector2(0, 0));
            _uvs.Add(new Vector2(0, 1));
            _uvs.Add(new Vector2(1, 0));
        
            _tangents.Add(new Vector4(1, 0, 0, 1));
            _tangents.Add(new Vector4(1, 0, 0, -1));
            _tangents.Add(new Vector4(1, 0, 0, 1));
        
        }
        
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.normals = _normals.ToArray();
        mesh.uv = _uvs.ToArray();
        mesh.tangents = _tangents.ToArray();

        _meshFilter.mesh = mesh;

    }

    [ContextMenu("Generate Quad")]
    private void GenerateQuad(Mesh mesh, Vector3 direction, int length)
    {
        for (int i = 0; i < length; i++)
        {
            _vertices.Add(new Vector3(0, 0, 0) + direction * i); // 0 BL
            _vertices.Add(new Vector3(0, 1, 0) + direction * i); // 1 TL
            _vertices.Add(new Vector3(1, 1, 0) + direction * i); // 2 TR
            _vertices.Add(new Vector3(1, 0, 0) + direction * i); // 3 BR
            
            _triangles.Add(0 + 4*i);
            _triangles.Add(1 + 4*i);
            _triangles.Add(3 + 4*i);
            
            _triangles.Add(2 + 4*i);
            _triangles.Add(3 + 4*i);
            _triangles.Add(1 + 4*i);
            
            _normals.Add(Vector3.back);
            _normals.Add(Vector3.back);
            _normals.Add(Vector3.back);
            _normals.Add(Vector3.back);
            
            _uvs.Add(new Vector2(0, 0));
            _uvs.Add(new Vector2(0, 1));
            _uvs.Add(new Vector2(1, 0));
            _uvs.Add(new Vector2(1, 1));
            
            _tangents.Add(new Vector4(1, 0, 0, 1));
            _tangents.Add(new Vector4(1, 0, 0, -1));
            _tangents.Add(new Vector4(1, 0, 0, 1));
            _tangents.Add(new Vector4(1, 0, 0, 1));
            
        }
        
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.normals = _normals.ToArray();
        mesh.uv = _uvs.ToArray();
        mesh.tangents = _tangents.ToArray();

        _meshFilter.mesh = mesh;
    }

    /// <summary>
    /// Method adds a quad to the list of the mesh
    /// </summary>
    /// <param name="v">4 Vector3 awaited in clockwise order from the BL</param>
    [ContextMenu("Add Quad")]
    private void AddQuad(Vector3[] v, int scaleFactor = 1)
    {
        int index = _vertices.Count;
        _vertices.Add(v[0]*scaleFactor);
        _vertices.Add(v[1]*scaleFactor);
        _vertices.Add(v[2]*scaleFactor);
        _vertices.Add(v[3]*scaleFactor);
        
        _triangles.Add(index);
        _triangles.Add(index + 1);
        _triangles.Add(index + 2);
        
        _triangles.Add(index);
        _triangles.Add(index + 2);
        _triangles.Add(index + 3);
        
        _uvs.Add(new Vector2(0, 0));
        _uvs.Add(new Vector2(0, 1));
        _uvs.Add(new Vector2(1, 0));
        _uvs.Add(new Vector2(1, 1));
        
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        
        _meshFilter.mesh = _mesh;
    }

    [ContextMenu("Pillar")]
    public void Pillar(Vector3 origin, float length, float height, int blockNumber = 0)
    {
        // Facing the cube the bottom left corner
        Vector3 bottomLeft = origin - (length/2) *  Vector3.right;

        for (int i = 0; i < blockNumber; i++)
        {
            Vector3[] face =
            {
                bottomLeft + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.right * length + Vector3.up * i * height,
                bottomLeft + Vector3.right * length + Vector3.up * i * height
            };
            Vector3[] face1 =
            {
                bottomLeft + Vector3.right * length + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.right * length + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.right * length + Vector3.back * length + Vector3.up * i * height,
                bottomLeft + Vector3.right * length + Vector3.back * length + Vector3.up * i * height
            };
            Vector3[] face2 =
            {
                bottomLeft + Vector3.right * length + Vector3.back * length + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.right * length + Vector3.back * length + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.back * length + Vector3.up * i * height,
                bottomLeft + Vector3.back * length + Vector3.up * i * height
            };
            Vector3[] face3 =
            {
                bottomLeft + Vector3.back * length + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.back * length + Vector3.up * i * height,
                bottomLeft + Vector3.up * height + Vector3.up * i * height,
                bottomLeft + Vector3.up * i * height
            };
            
            
            AddQuad(face);
            AddQuad(face1);
            AddQuad(face2);
            AddQuad(face3);
            
        }

    }
    
    [ContextMenu("Clear mesh")]
    private void ClearMesh()
    {
        _mesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
        _normals.Clear();
        _uvs.Clear();
        _tangents.Clear();
        
        _meshFilter.mesh.Clear();
    }

    /// <summary>
    /// Casts rays up and down from the object's position and returns hit positions
    /// </summary>
    /// <returns>Vector3 array containing bottom and top hit positions</returns>
    private Vector3[] GetTopAndBottomPositions()
    {
        Vector3 topPosition;
        Vector3 bottomPosition;
        
        // Cast ray downward
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit bottomHit, _rayDistance, _hitLayers))
        {
            bottomPosition = bottomHit.point;
        }
        else
        {
            throw new System.Exception("No bottom surface detected!");
        }
        
        // Cast ray upward
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit topHit, _rayDistance, _hitLayers))
        {
            topPosition = topHit.point;
        }
        else
        {
            // If no top hit
            topPosition = bottomPosition + Vector3.up * _rayDistance;
        }
        
        return new[] {bottomPosition, topPosition};
    }

    [ContextMenu("Raycast")]
    private void GeneratePillarFromRaycast()
    {
        Vector3[] bottomTop = GetTopAndBottomPositions();
    
        // Convert world space positions to local space
        Vector3 localBottom = transform.InverseTransformPoint(bottomTop[0]);
        Vector3 localTop = transform.InverseTransformPoint(bottomTop[1]);
    
        // Calculate distance between top and bottom
        float totalHeight = localTop.y - localBottom.y;
    
        // Calculate how many segments we need to reach the top
        // Round up to ensure we reach or exceed the top
        int pillarSegments = Mathf.CeilToInt(totalHeight / _blockHeight);
    
        // Optional: Adjust the top position to exactly match the last block
        // This ensures the pillar reaches exactly to the top hit point
        float adjustedHeight = totalHeight / pillarSegments;
    
        Pillar(localBottom, _pillarWidth, adjustedHeight, pillarSegments);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.up * _rayDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * _rayDistance);
    }
}
