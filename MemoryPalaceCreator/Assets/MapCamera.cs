using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour {

    public Transform player;
    public float height;
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = new Vector3(player.position.x,height ,player.position.z);
        transform.position = pos;
	}
}
