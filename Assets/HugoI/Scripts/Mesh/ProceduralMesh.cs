using System.Collections.Generic;
using UnityEngine;

namespace HugoI.Scripts.Mesh
{
    public static class ProceduralMesh
    {
        public static void GenerateQuad(int startingIndex, List<int> triangles)
        {
            // ADD TRIANGLES
            triangles.Add(startingIndex);
            triangles.Add(startingIndex + 1);
            triangles.Add(startingIndex + 2);
            
            triangles.Add(startingIndex + 1);
            triangles.Add(startingIndex + 3);
            triangles.Add(startingIndex + 2);
        }
        
        public static void GenerateQuadAndAddVertices(List<Vector3> vertices, List<int> triangles, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int index = vertices.Count;
            
            // ADD VERTICES
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            
            // ADD TRIANGLES
            triangles.Add(index);
            triangles.Add(index + 1);
            triangles.Add(index + 2);
            
            triangles.Add(index);
            triangles.Add(index + 2);
            triangles.Add(index + 3);
        }

        public static void AddQuadUVs(List<Vector2> uvs, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
        {
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
            uvs.Add(uv4);
        }
    }
}
