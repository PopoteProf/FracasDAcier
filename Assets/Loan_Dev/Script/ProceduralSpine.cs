using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class ProceduralSpine : MonoBehaviour
{
   [Header("Mesh Settings")]
   [SerializeField] private MeshFilter _meshFilter;
   [SerializeField] private MeshRenderer _meshRenderer;
   [SerializeField] [Range(0.1f,20f)] private  float _height;
   [SerializeField] [Range(0.1f,20f)] private float _depht;
   
   [Header("Spline Settings")]
   [SerializeField] private SplineContainer _spline;
   [SerializeField] private int _numberOfPoints;
   [SerializeField] private Color _colorDebug;
   [SerializeField] private Color _colorPerpendicular;

   [Header("Debug Settings")]
   [SerializeField] private bool _isGeneratePoints;
   [SerializeField] private bool _isGenerateMesh;
   
   private List<Vector3> _vertices;
   private List<int> _triangles;
   private List<Vector2> _uvs ;
   private Mesh mesh;
   private List<Vector3> _hitPoints;
   private List<Vector3> _perpendicularPoints;


   private void Update()
   {
      
      if (_isGeneratePoints)
         GeneratePoints();
      
      if (_isGenerateMesh)
         GenerateMesh();
         
   }

   private void GeneratePoints()
   {
      _hitPoints = new List<Vector3>();
      _perpendicularPoints = new List<Vector3>();
      int segmentCount = _numberOfPoints;

      for (int i = 0; i <= segmentCount; i++)
      {
         float t = (float)i / segmentCount;

         Vector3 pos = _spline.EvaluatePosition(t);
         Vector3 tangent = _spline.EvaluateTangent(t);

         Vector3 perpendicular = Vector3.Cross(tangent.normalized * _depht, Vector3.up);
         _perpendicularPoints.Add(perpendicular);

         RaycastHit hit;

         Vector3 origin = pos;
         Vector3 direction = Vector3.down;

         if (Physics.Raycast(origin, direction, out hit, 100f))
         {
            float calculOffset = hit.point.y + _height;
            Vector3 yOffset = new Vector3(hit.point.x, calculOffset, hit.point.z);

            Debug.DrawLine(hit.point, yOffset, _colorDebug);
            Debug.DrawRay(yOffset, perpendicular, _colorPerpendicular);
            _hitPoints.Add(hit.point - transform.position);
            _hitPoints.Add(yOffset - transform.position);
               
         }
      }
   }

   private void GenerateMesh()
   {
      mesh = new Mesh();
      mesh.name = "MonProceduralQuad";
      _vertices = new List<Vector3>();
      _triangles = new List<int>();
      _uvs =  new List<Vector2>();
      int currentPerpendicular = 0;

      for (int i = 0; i + 3 < _hitPoints.Count; i += 2)
      {
         if (_hitPoints.Count < 4)
            break;
         
         Vector3 a = _hitPoints[i];
         Vector3 b = _hitPoints[i + 1];
         Vector3 c = _hitPoints[i + 2];
         Vector3 d = _hitPoints[i + 3];
         
         //Face exterieur
         GenerateQuadMesh.AddQuad(
            _vertices, _triangles, a, b,c, d
         );
         GenerateQuadMesh.AddQuadUV(_uvs,new Vector2(0.25f,0.25f),new Vector2(0.25f,0.5f),new Vector2(0.5f,0.25f),new Vector2(0.5f,0.5f));
         
         Vector3 bBis = b + _perpendicularPoints[currentPerpendicular];
         currentPerpendicular+=1;
         Vector3 dBis = d + _perpendicularPoints[currentPerpendicular];
         
         //Face Dessus
         GenerateQuadMesh.AddQuad(
            _vertices,_triangles,b,bBis,d,dBis
            );
         GenerateQuadMesh.AddQuadUV(_uvs, new Vector2(0.25f,0.5f),new Vector2(0.25f,0.75f),new Vector2(0.5f,0.5f),new Vector2(0.5f,0.75f));
         
         currentPerpendicular -=1;
         Vector3 aBis = a + _perpendicularPoints[currentPerpendicular];
         currentPerpendicular+=1;
         Vector3 cBis = c + _perpendicularPoints[currentPerpendicular];
         
         //Face Interieur
         GenerateQuadMesh.AddQuad(
            _vertices,_triangles,aBis, cBis,bBis,dBis
            );
         GenerateQuadMesh.AddQuadUV(_uvs,new Vector2(0.5f,0.75f),new Vector2(0.25f,0.75f),new Vector2(0.5f,1),new Vector2(0.25f,1));
      }
         
      mesh.vertices = _vertices.ToArray();
      mesh.triangles = _triangles.ToArray();
      mesh.uv = _uvs.ToArray();
        
      mesh.RecalculateNormals();
      mesh.RecalculateTangents();
        
      _meshFilter.mesh = mesh;
   }
}
