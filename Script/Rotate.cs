using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



[CustomEditor(typeof(MeshFilter))]

public class Rotate : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Fix Rotation"))
        {
            MeshFilter meshFilter = (MeshFilter)target;
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] newVertices = new Vector3[vertices.Length];
            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertex = vertices[i];
                newVertices[i] = rotation * vertex;
            }
            mesh.vertices = newVertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.UploadMeshData(false); // Upload the mesh data

            // Update or add MeshCollider
            MeshCollider meshCollider = meshFilter.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.sharedMesh = null;
                meshCollider.sharedMesh = mesh;
            }
            else
            {
                meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
            }

            EditorUtility.SetDirty(meshCollider);
        }
    }
}
