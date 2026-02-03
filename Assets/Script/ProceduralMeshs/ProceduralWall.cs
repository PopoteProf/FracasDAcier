using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralWall : MonoBehaviour {
    [SerializeField] private SplineContainer _spline;
    [SerializeField] private float _segmentsLength =1;
    [SerializeField] private float _segmentsHeigth =5;
    [SerializeField] private float _segmentsDepth =1;
    [SerializeField]private MeshFilter _meshFilter;
    [SerializeField] private bool _generateInUpdate;
    [SerializeField] private bool _topOfWallIsFlate =true;
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<int>  _triangles;
    private List<Vector2> _uvs;
    void Update() {
        if( _generateInUpdate) Generate();
    }

    [ContextMenu("Generate")]
    private void Generate()
    {
        
        _mesh = new Mesh();
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs = new List<Vector2>();
        int segmentCount = Mathf.FloorToInt(_spline.CalculateLength() / _segmentsLength); 
        for (int i = 1; i < segmentCount; i++) {
            float t1 = ((float)i - 1) / segmentCount;
            float t2 = ((float)i ) / segmentCount;
            GenerateSegments(t1, t2);
            //Vector3 pos =_spline.EvaluatePosition((float)i / segmentCount);
            //Vector3 tangant = _spline.EvaluateTangent((float)i / segmentCount);
            //Vector3 offset = Vector3.Cross(tangant.normalized, Vector3.up);
            //if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit)) {
            //    Debug.DrawLine(hit.point, hit.point+new Vector3(0,_segmentsHeigth), Color.crimson);
            //    Debug.DrawLine(hit.point+new Vector3(0,_segmentsHeigth)+offset, hit.point+new Vector3(0,_segmentsHeigth), Color.chartreuse);
            //}
        }
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
        _meshFilter.mesh = _mesh;
        
    }

    private void GenerateSegments(float t1, float t2) {
        Vector3 pos1 =GetWallPoints(t1,out Vector3 pos3 );
        Vector3 pos2 =GetWallPoints(t2,out Vector3 pos4 );
        Vector3 pos5 = GetTangentWallPoints(t1, out Vector3 pos7);
        Vector3 pos6 = GetTangentWallPoints(t2, out Vector3 pos8);

        if (_topOfWallIsFlate) {
            pos5.y = pos1.y;
            pos6.y = pos2.y;
        }

        AddQuad(pos1,pos2, pos3, pos4 );
        AddQuadUVs(new Vector2(0,1), new Vector2(1,1), new Vector2(0,0), new Vector2(1,0));
        AddQuad(pos5,pos6, pos1, pos2 );
        AddQuadUVs(new Vector2(0,1), new Vector2(1,0), new Vector2(1,1), new Vector2(0,0));
        AddQuad(pos7,pos8, pos5, pos6 );
        AddQuadUVs(new Vector2(0,1), new Vector2(1,1), new Vector2(0,0), new Vector2(1,0));
    }

    private Vector3 GetWallPoints(float t, out Vector3 downPoint) {
        Vector3 pos = (Vector3)_spline.EvaluatePosition(t) ;
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit)) {
            downPoint = hit.point- transform.position;
            return hit.point+new Vector3(0,_segmentsHeigth,0)- transform.position;
        }
        downPoint = pos +new Vector3(0,-_segmentsHeigth,0);
        return pos;
    }

    private Vector3 GetTangentWallPoints(float t, out Vector3 downPoint) {
        Vector3 pos = (Vector3.Cross(((Vector3)_spline.EvaluateTangent(t)).normalized,Vector3.up)*_segmentsDepth)+(Vector3)_spline.EvaluatePosition(t);
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit)) {
            downPoint = hit.point- transform.position;
            return hit.point+new Vector3(0,_segmentsHeigth,0)- transform.position;
        }
        downPoint = pos +new Vector3(0,-_segmentsHeigth,0);
        return pos;
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
