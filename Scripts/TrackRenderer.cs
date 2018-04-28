using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TrackRenderer : MonoBehaviour
{
    public float width = 2.0f;
    public float length = 2.0f;

    public Mesh CreateSectionMesh(TrackSection section)
    {
        int subdivisions = Mathf.CeilToInt(section.length / length);
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[2 + subdivisions * 2];
        Vector2[] uvs = new Vector2[2 + subdivisions * 2];
        int[] triangles = new int[subdivisions * 2 * 6];

        int vertexIndex = 0;
        int triangleIndex = 0;

        float distance = 0.0f;
        for(int i = 0; i < subdivisions + 1; i++)
        {
            if(i == subdivisions)
                distance = section.length;

            Vector3 center = section.GetPositionOnTrack(distance).Vector3() - section.position.Vector3() + Vector3.up * 0.01f;
            Quaternion angle = Quaternion.AngleAxis(270f - (float)section.GetRotationOnTrack(distance).Degrees, Vector3.up);
            Vector3 tangent = angle * Vector3.forward;

            Vector3 right = Vector3.Cross(tangent, Vector3.up) * width * 0.5f;
            Vector3 left = -right;

            vertices[vertexIndex] = left + center;
            vertices[vertexIndex + 1] = right + center;

            uvs[vertexIndex] = new Vector2(0, i); 
            uvs[vertexIndex + 1] = new Vector2(1, i);

            //Debug.Log("i: " + i + " center: " + center.ToString() + " left: " + left.ToString() + " right " + right.ToString());
            if(i > 0.0f)
            {
                triangles[triangleIndex] = vertexIndex - 2;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex - 1;

                triangles[triangleIndex + 3] = vertexIndex - 2;
                triangles[triangleIndex + 4] = vertexIndex;
                triangles[triangleIndex + 5] = vertexIndex + 1;
                triangleIndex += 6;
            }
            vertexIndex += 2;

            distance += section.length / (float)subdivisions;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        //mesh.RecalculateNormals();


        return mesh;
    }
}
