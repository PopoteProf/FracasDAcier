using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
[RequireComponent ( typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    [SerializeField] private bool _generateInUpdate;
    [SerializeField] private Mesh _meshToExport ;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    
    private List<Vector3> _vertivces = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private List<Vector2> _uvs = new List<Vector2>();
    private Mesh _mesh ;
    void Start() {
        _meshFilter =  GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        GenerateMesh();
    }

    public void Update() {
        if( _generateInUpdate) GenerateMesh();
    }

    [ContextMenu("Generate Mesh")]
    private void GenerateMesh() 
    {
        _mesh = new Mesh();
        _mesh.name = "MonProceduralCube";
        _vertivces = new List<Vector3>();
        _triangles = new List<int>();
        _uvs = new List<Vector2>();
        
        //---------------------------------------------------
        // Ma Logique de générations du mesh
        //---------------------------------------------------
        
        _mesh.vertices = _vertivces.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        
        _mesh.RecalculateNormals();
        
        _meshFilter.mesh = _mesh;
        
        
        
        Vector3 a =new Vector3(0,0,0);
        Vector3 b =new Vector3(0,1,0);
        Vector3 c =new Vector3(1,0,0);
        Vector3 d =new Vector3(1,1,0);
        Vector3 e =new Vector3(0,0,1);
        Vector3 f =new Vector3(0,1,1);
        Vector3 g =new Vector3(1,0,1);
        Vector3 h =new Vector3(1,1,1);
        
        AddQuad(a,b,c,d);
        AddQuadUVs(new Vector2(0.25f, 0.75f), new Vector2(0.25f,1), new Vector2(0.5f, 0.75f), new Vector2(0.5f,1));
        AddQuad(c,d,g,h);
        AddQuadUVs(new Vector2(0.5f, 0.75f), new Vector2(0.75f,0.75f),new Vector2(0.5f,0.5f ),new Vector2(0.75f, 0.5f));
        AddQuad(g,h,e,f);
        AddQuadUVs(new Vector2(0.25f, 0.25f), new Vector2(0.25f,0.5f),  new Vector2(0.5f,0.25f),new Vector2(0.5f, 0.5f));
        AddQuad(e,f,a,b);
        AddQuadUVs(new Vector2(0.25f, 0.5f), new Vector2(0,0.5f),  new Vector2(0.25f,0.75f),new Vector2(0f, 0.75f));
        AddQuad(f,h,b,d);
        AddQuadUVs(new Vector2(0.25f, 0.25f), new Vector2(0.5f,0.25f),  new Vector2(0.25f,0),new Vector2(0.5f, 0));
        AddQuad(c,g,a,e);
        AddQuadUVs(new Vector2(0.25f, 0.75f), new Vector2(0.5f,0.75f),  new Vector2(0.25f,0.5f) ,new Vector2(0.5f, 0.5f));
        
        
        _mesh.vertices = _vertivces.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        
        _meshFilter.mesh = _mesh;
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        int index = _triangles.Count;
        _vertivces.Add(v1);
        _vertivces.Add(v2);
        _vertivces.Add(v3);
        
        _triangles.Add(index);
        _triangles.Add(index+1);
        _triangles.Add(index+2);
    }

    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int index = _vertivces.Count;
        _vertivces.Add(v1);
        _vertivces.Add(v2);
        _vertivces.Add(v3);
        _vertivces.Add(v4);
        
        _triangles.Add(index);
        _triangles.Add(index+1);
        _triangles.Add(index+2);
        
        _triangles.Add(index+1);
        _triangles.Add(index+3);
        _triangles.Add(index+2);
    }

    private void AddTriangleUVs(Vector2 uv1, Vector2 uv2, Vector2 uv3) {
        _uvs.Add(uv1);
        _uvs.Add(uv2);
        _uvs.Add(uv3);
    }
    private void AddQuadUVs(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
        _uvs.Add(uv1);
        _uvs.Add(uv2);
        _uvs.Add(uv3);
        _uvs.Add(uv4);
    }

    [ContextMenu("SaveMesh")]
    private void SaveMesh() {
        Mesh mesh = Instantiate(_meshFilter.sharedMesh);
        mesh.name = "ProceduralMesh";
        AssetDatabase.CreateAsset(mesh, "Assets/"+mesh.name+".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [ContextMenu("ExportMesh")]
    private void ExportMesh()
    {
        string assetPath = AssetDatabase.GetAssetPath(_meshToExport);

        Debug.Log(AssetDatabase.ExtractAsset(_meshToExport, "Assets/ExportedMesh"));
        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        Debug.Log("Mesh Exported "+assetPath+ " at Assets/ExportedMesh.obj");
    }
    
    public static class ObjExporter
    {
        public static void ExportMesh(Mesh mesh, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (Vector3 v in mesh.vertices)
                    sw.WriteLine($"v {v.x} {v.y} {v.z}");

                foreach (Vector3 n in mesh.normals)
                    sw.WriteLine($"vn {n.x} {n.y} {n.z}");

                foreach (Vector2 uv in mesh.uv)
                    sw.WriteLine($"vt {uv.x} {uv.y}");

                for (int i = 0; i < mesh.triangles.Length; i += 3)
                {
                    int a = mesh.triangles[i] + 1;
                    int b = mesh.triangles[i + 1] + 1;
                    int c = mesh.triangles[i + 2] + 1;
                    sw.WriteLine($"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}");
                }
            }
        }
    }
}
