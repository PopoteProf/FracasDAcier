using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PopoteProceduraleBridge2 : MonoBehaviour
{
    [SerializeField]private MeshFilter _meshFilter;
    [SerializeField] private bool _generateInUpdate = true; 
    [SerializeField] private Vector3 _brigdeBorderOffset =new Vector3(0,10,5);
    [SerializeField] private int _segmentCount = 2;
    [SerializeField] private Vector3 _bezierOffset = new Vector3(0, 10, 0);
    [SerializeField] private float _bridgeWidth = 3;

    [SerializeField] private bool _showGizmos = true;
    [SerializeField] private bool _generatMesh = true;

    
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Vector2> _uvs;


    private Vector3 GetRelatifBridgeBorderOffset () =>  transform.forward*_brigdeBorderOffset.z+transform.up*_brigdeBorderOffset.y+transform.right*_brigdeBorderOffset.x; 
    private Vector3 GetRevertRelatifBridgeBorderOffset () =>  -transform.forward*_brigdeBorderOffset.z+transform.up*_brigdeBorderOffset.y+transform.right*_brigdeBorderOffset.x; 
    private Vector3 GetBezierOffset ()=> (transform.forward*_bezierOffset.z+transform.up*_bezierOffset.y+transform.right*_bezierOffset.x)+transform.position; 
    private Vector3 GetWidthOffest()=> -transform.right * _bridgeWidth;

    private void Update()
    {
        if (_generateInUpdate) {
            if( _showGizmos)GizmosGeneration();
            if (_generatMesh)Generate();
        }
    }

    private void Generate() {
        
        _mesh = new Mesh();
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs = new List<Vector2>();
        
        Vector3 A,B,C,D = Vector3.zero;
        A=B=C=D;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitA)) A = hitA.point;
        if (Physics.Raycast(new Ray(transform.position, -transform.forward), out RaycastHit hitB)) B = hitB.point;
        if (Physics.Raycast(new Ray(A+GetRelatifBridgeBorderOffset(), -transform.up), out RaycastHit hitC)) C = hitC.point;
        if (Physics.Raycast(new Ray(B+GetRevertRelatifBridgeBorderOffset(), -transform.up), out RaycastHit hitD)) D = hitD.point;
        
        for (int i = 1; i <= _segmentCount; i++) {
            float t1 = (i - 1f) / _segmentCount;
            float t2 = ((float)i) / _segmentCount;
            GenerateSegment(A,B,C,D,t1,t2);
        }
        //GenerateSegment(A,B,C,D,,1);
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        _meshFilter.mesh = _mesh;
    }

    private void GenerateSegment(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t1, float t2) {
        Vector3 pos1 = GetBezierPos(A,B,GetBezierOffset(),t1);
        Vector3 pos2 = GetBezierPos(A,B,GetBezierOffset(),t2);
        Vector3 pos3 = GetBezierPos(C,D,GetBezierOffset(),t1);
        Vector3 pos4 = GetBezierPos(C,D,GetBezierOffset(),t2);
        Vector3 pos5 = pos1 +GetWidthOffest();
        Vector3 pos6 = pos2 +GetWidthOffest();
        Vector3 pos7 = pos3 +GetWidthOffest();
        Vector3 pos8 = pos4 +GetWidthOffest();
        
        pos1 -=transform.position;
        pos2 -=transform.position;
        pos3 -=transform.position;
        pos4 -=transform.position;
        pos5 -=transform.position;
        pos6 -=transform.position;
        pos7 -=transform.position;
        pos8 -=transform.position;
        pos1 = transform.InverseTransformVector( pos1);
        pos2 = transform.InverseTransformVector( pos2);
        pos3 = transform.InverseTransformVector( pos3);
        pos4 = transform.InverseTransformVector( pos4);
        pos5 = transform.InverseTransformVector( pos5);
        pos6 = transform.InverseTransformVector( pos6);
        pos7 = transform.InverseTransformVector( pos7);
        pos8 = transform.InverseTransformVector( pos8);
       
        AddQuad( pos2, pos4, pos1, pos3);
        AddQuadUVs(new Vector2(0,0),new Vector2(0,0.5f),new Vector2(0.5f,0), new Vector2(0.5f,0.5f));
        
        AddQuad( pos5, pos7, pos6, pos8);
        AddQuadUVs(new Vector2(0,0),new Vector2(0,0.5f),new Vector2(0.5f,0), new Vector2(0.5f,0.5f));
        
        AddQuad(pos4, pos8, pos3, pos7);
        AddQuadUVs(new Vector2(0,0.5f),new Vector2(0,1),new Vector2(0.5f,0.5f), new Vector2(0.5f,1f));
    }

    private void GizmosGeneration()
    {
        Vector3 A,B,C,D = Vector3.zero;
        A=B=C=D;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hitA)) {
            A = hitA.point;
        }
        Debug.DrawRay(transform.position, -transform.forward, Color.red);
        if (Physics.Raycast(new Ray(transform.position, -transform.forward), out RaycastHit hitB)) {
            B = hitB.point;
        }
        Debug.DrawRay(A+GetRelatifBridgeBorderOffset(), -transform.up, Color.red);
        Debug.DrawLine(A+GetRelatifBridgeBorderOffset(), A, Color.green);
        if (Physics.Raycast(new Ray(A+GetRelatifBridgeBorderOffset(), -transform.up), out RaycastHit hitC)) {
            Debug.DrawLine(A+GetRelatifBridgeBorderOffset(),  hitC.point, Color.red);
            C = hitC.point;
        }
        Debug.DrawRay(B+GetRevertRelatifBridgeBorderOffset(), -transform.up, Color.red);
        Debug.DrawLine(B+GetRevertRelatifBridgeBorderOffset(), B, Color.green);
        if (Physics.Raycast(new Ray(B+GetRevertRelatifBridgeBorderOffset(), -transform.up), out RaycastHit hitD)) {
            Debug.DrawLine(B+GetRevertRelatifBridgeBorderOffset(),  hitD.point, Color.red);
            D = hitD.point;
        }
        Debug.DrawLine(transform.position, GetBezierOffset(), Color.green);

        for (int i = 1; i < _segmentCount; i++) {
            float t1 = (i - 1f) / _segmentCount;
            float t2 = ((float)i) / _segmentCount;
            Vector3 pos1 = GetBezierPos(A,B,GetBezierOffset(),t1);
            Vector3 pos2 = GetBezierPos(A,B,GetBezierOffset(),t2);
            Vector3 pos3 = GetBezierPos(C,D,GetBezierOffset(),t1);
            Vector3 pos4 = GetBezierPos(C,D,GetBezierOffset(),t2);
            Debug.DrawLine(pos1,pos2,Color.blue);
            Debug.DrawLine(pos4,pos2,Color.blue);
            Debug.DrawLine(pos3,pos4,Color.blue);
            Debug.DrawLine(pos1,pos3,Color.blue);
        }
    }

    private void GenerateSegment()
    {
        
    }

    /// <summary>
    /// Courbe de Bézier avec un point de control
    /// </summary>
    /// <param name="a">Position de Départ</param>
    /// <param name="b">Position de Fin</param>
    /// <param name="c">Position de Control</param>
    /// <param name="t">Valeur Normalizer</param>
    /// <returns>Position en Vector3 en fonction de t</returns>
    private Vector3 GetBezierPos(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 ac = Vector3.Lerp(a, c, t);
        Vector3 cb = Vector3.Lerp(c, b, t);
        return Vector3.Lerp(ac, cb, t);
    }
    
    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int index = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _vertices.Add(v4);
        
        _triangles.Add(index);
        _triangles.Add(index+1);
        _triangles.Add(index+2);
        
        _triangles.Add(index+1);
        _triangles.Add(index+3);
        _triangles.Add(index+2);
    }
    private void AddQuadUVs(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
        _uvs.Add(uv1);
        _uvs.Add(uv2);
        _uvs.Add(uv3);
        _uvs.Add(uv4);
    }
}