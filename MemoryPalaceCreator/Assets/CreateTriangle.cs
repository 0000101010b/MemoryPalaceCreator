using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour {


    public float scale;
    public Vector3[] newVerties;
    public Material material;

	// Use this for initialization
	void Start () {
        Mesh mesh = new Mesh();
        MeshRenderer meshRenderer= gameObject.AddComponent<MeshRenderer>();
        meshRenderer.receiveShadows = false;
        meshRenderer.shadowCastingMode =0;
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVerties;
        //new Vector3[] { new Vector3(-0.5f, 0,-0.5f ), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, -0.5f) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 1, 2 };

        meshRenderer.material = material;
                

        transform.localScale = new Vector3(scale,scale,scale);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
