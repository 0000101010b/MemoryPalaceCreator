using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    public float speed;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float movement=Input.GetAxis("Vertical");
        transform.Translate(movement*Vector3.forward*Time.deltaTime*speed);
        

	}
}
