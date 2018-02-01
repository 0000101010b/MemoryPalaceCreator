using UnityEngine;
using System.Collections;

public class CreateRoof : MonoBehaviour {

    public float scale=1f;
    public float max;
	// Use this for initialization
	void Start () {

        GameObject g = new GameObject();
        WallMesh roof = g.AddComponent<WallMesh>();
        roof.floorMeshConstructor(20f, 20f, .5f, .5f);
        Mesh mesh=g.GetComponent<MeshFilter>().mesh;

        Vector3[] baseHeight = mesh.vertices;
        Vector3[] vertices = new Vector3[baseHeight.Length];
        //float noiseStrength = 1f;
        //float noiseWalk = 1f;

        Vector3 left  = transform.position - transform.forward * 5;
        Vector3 right = transform.position + transform.forward * 5;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            //vertex.y += Mathf.Sin(Time.time * 10.0f + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            //vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            float dist = Vector3.Distance(g.transform.position,baseHeight[i]);
            
            //dist= Vector3.Distance(left, baseHeight[i]);
            if (dist < 50f)
            {
                vertex.y += max - (dist);
            }
           /* dist = Vector3.Distance(right, baseHeight[i]);
            if (dist < 50f)
            {
                vertex.y += max - (dist);
            }*/
            //vertex.y +=height;
            //height+=.2f*temp;
            vertices[i] = vertex;
           
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
