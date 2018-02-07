using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour {


    GameObject left;
    GameObject right;
    GameObject top;

	
    // Use this for initialization
	public void Build () {
     
        //Left
        left = new GameObject("Left");
        left.transform.position = new Vector3(transform.position.x-0.5f,transform.position.y,transform.position.z);

        WallMesh wallMesh = left.AddComponent<WallMesh>();
        wallMesh.invert = false;
        wallMesh.WallMeshContructor(1,2,1,1);

        left.transform.forward = -transform.right;
        left.AddComponent<WallMesh>();

     
        //Right
        right = new GameObject("right");
        right.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        wallMesh = right.AddComponent<WallMesh>();
        wallMesh.invert = false;
        wallMesh.WallMeshContructor(1, 2, 1, 1);

        right.transform.forward = transform.right;
        right.AddComponent<WallMesh>();

        //Up
        top = new GameObject("top");
        top.transform.position = new Vector3(transform.position.x , transform.position.y+1, transform.position.z);

        wallMesh = top.AddComponent<WallMesh>();
        wallMesh.invert = false;
        wallMesh.WallMeshContructor(1 ,1 , 1 , 1);

        top.transform.forward = transform.up;
        top.AddComponent<WallMesh>();

        left.transform.SetParent(transform);
        right.transform.SetParent(transform);
        top.transform.SetParent(transform);



    }

    // Update is called once per frame
    void Update () {
		
	}
}
