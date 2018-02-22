using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner_Gen : MonoBehaviour {

    GameObject l;
    GameObject r;

	// Use this for initialization
	public void Build () {
        l = new GameObject("l");
        l.transform.position = transform.position+ new Vector3(-0.25f, 1.5f,0);
        WallMesh lm=l.AddComponent<WallMesh>();
        lm.invert = false;
        l.transform.SetParent(transform);
        lm.WallMeshContructor(0.5f, 3, 0.5f, 0.5f);
        l.transform.forward = transform.right;
    

        r = new GameObject("r");
        r.transform.position = transform.position+ new Vector3(0f, 1.5f, -0.25f);
        WallMesh rm = r.AddComponent<WallMesh>();
        rm.invert = false;
        rm.transform.SetParent(transform);

        rm.WallMeshContructor(0.5f, 3, 0.5f, 0.5f);




    }

    // Update is called once per frame
    void Update () {
		
	}
}
