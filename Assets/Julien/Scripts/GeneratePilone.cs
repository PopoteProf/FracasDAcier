using System;
using System.Collections.Generic;
using UnityEngine;

namespace Julien.Scripts
{
    public class GeneratePilone : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        [SerializeField] private List<Vector3> _vertices = new List<Vector3>();
        [SerializeField] private List<Vector3> _normals = new List<Vector3>();
        [SerializeField] private List<Vector2> _uvs = new List<Vector2>();
        [SerializeField] private List<Vector4> _tangents = new List<Vector4>();
        [SerializeField] private List<int> _triangles = new List<int>();

        [Header(" parameters")] 
        public int Offset;
        public float Hauteur;
        public float Largeur;

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            GenerateMesh();
        }

        public void GenerateMesh()
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
            
            // AddQuad(a,b,d,c);
            // AddQuad(c,d,h,g);
            // AddQuad(f,b,a,e);
            // AddQuad(h,f,e,g);
            
            //GenerateSquare(a,b,c,d,e,f,g,h);
            for (int i = 0; i < Hauteur; i++)
            {
                GenerateSegment(new Vector3(0,Vector3.zero.y + i * Offset,0));
            }
            
            
            
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.uv = _uvs.ToArray();
        
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        
            _meshFilter.mesh = mesh;
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

        public void RecusiveSquar(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6, Vector3 v7, Vector3 v8)
        {
            for (int i = 0; i < Hauteur; i++)
            {
                GenerateSquare(v1, v2, v3, v4, v5, v6, v7, v8);

                v1 = v2;
                v3 = v4;
                v7 = v8;
                v5 = v6;

                v2 = new Vector3(v2.x, v2.y * i, v2.z);
                v4 = new Vector3(v4.x, v4.y * i, v4.z);
                v6 = new Vector3(v6.x, v6.y * i, v6.z);
                v8 = new Vector3(v8.x, v8.y * i, v8.z);
            }
        }

        public void GenerateSegment(Vector3 origine)
        {
            Vector3 a = new Vector3(origine.x - Largeur / 2,origine.y + 0, origine.z - Largeur / 2); // 0
            Vector3 b = new Vector3(origine.x - Largeur / 2,origine.y + 1, origine.z - Largeur / 2); // 1
            Vector3 c = new Vector3(origine.x + Largeur / 2,origine.y + 0, origine.z - Largeur / 2); // 2
            Vector3 d = new Vector3(origine.x + Largeur / 2,origine.y + 1, origine.z - Largeur / 2); // 3 
            Vector3 e = new Vector3(origine.x - Largeur / 2,origine.y + 0, origine.z + Largeur / 2); // 4
            Vector3 f = new Vector3(origine.x - Largeur / 2,origine.y + 1, origine.z + Largeur / 2); // 5
            Vector3 g = new Vector3(origine.x + Largeur / 2,origine.y + 0, origine.z + Largeur / 2); // 6 
            Vector3 h = new Vector3(origine.x + Largeur / 2,origine.y + 1, origine.z + Largeur / 2); // 7
            
            GenerateSquare(a,b,c,d,e,f,g,h);
        }
        
        public void GenerateSquare(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6, Vector3 v7, Vector3 v8)
        {
            int index = _vertices.Count;
            
            _vertices.Add(v1);
            _vertices.Add(v2);
            _vertices.Add(v3);
            _vertices.Add(v4);
            _vertices.Add(v5);
            _vertices.Add(v6);
            _vertices.Add(v7);
            _vertices.Add(v8);
            
            _triangles.Add(index);
            _triangles.Add(index + 1);
            _triangles.Add(index + 2);
            
            _triangles.Add(index + 2);
            _triangles.Add(index + 1);
            _triangles.Add(index + 3);
            
            _triangles.Add(index + 2);
            _triangles.Add(index + 3);
            _triangles.Add(index + 6);
            
            _triangles.Add(index + 6);
            _triangles.Add(index + 3);
            _triangles.Add(index + 7);
            
            _triangles.Add(index + 6);
            _triangles.Add(index + 7);
            _triangles.Add(index + 4);
            
            _triangles.Add(index + 4);
            _triangles.Add(index + 7);
            _triangles.Add(index + 5);
            
            _triangles.Add(index + 4);
            _triangles.Add(index + 5);
            _triangles.Add(index + 0);
            
            _triangles.Add(index + 0);
            _triangles.Add(index + 5);
            _triangles.Add(index + 1);
            
        }
    }
}
