using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMapPosition : MonoBehaviour {


    public Transform player;
    public float height;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(player.position.x, height, player.position.z);
        transform.position = pos;
        transform.forward = new Vector3(player.transform.forward.x,0.0f,player.transform.forward.z);
    }
}
