using System.Collections.Generic;
using UnityEngine;

public static class GenerateQuadMesh
{
    public static void AddQuad(List<Vector3> vertices, List<int> triangles,Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int index = vertices.Count;
        
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        
        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
        
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index + 3);
    }
    
    public static void AddQuadByOrigin(List<Vector3> vertices, List<int> triangles,Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 origin)
    {
        int index = vertices.Count;
        
        vertices.Add(origin + v1);
        vertices.Add(origin +v2);
        vertices.Add(origin +v3);
        vertices.Add(origin +v4);
        
        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
        
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index + 3);
    }

    
    public static void AddQuadUV(List<Vector2> uvs,Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
    {
        uvs.Add(uv1);
        uvs.Add(uv2);
        uvs.Add(uv3);
        uvs.Add(uv4);
    }
}
