using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTree : MonoBehaviour {

    public float pdMin;
    public float pdMax;
    public int _r;
    public float f_pv;

    Color c;
    // Use this for initialization
    void Start()
    {
        
        List<Vector3> vertices = new List<Vector3>();
        MeshFilter meshFilter = transform.gameObject.AddComponent<MeshFilter>();
        
        Mesh mesh = meshFilter.mesh;

        for (int r = _r; r > 0; r--)
        {
            float pv = r * f_pv;
            List<Vector3> temp = MyVector3Lib.MakeRandomPolygon(transform.position, pdMin, pdMax, r, pv);

            foreach (Vector3 v in temp)
                vertices.Add(v);
        }

        mesh.vertices = vertices.ToArray();
        MeshCollider collider = transform.gameObject.AddComponent<MeshCollider>();
        collider.convex = true;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        transform.gameObject.AddComponent<MeshRenderer>().material.color=c;
        
    }
}
