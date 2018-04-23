using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMPobj : MonoBehaviour {

    public int temp;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
     
        Ray ray=new Ray();
        RaycastHit hit = new RaycastHit();
        ray.origin = transform.position;
        ray.direction = transform.forward;
        Debug.Log("hello");
        if(Physics.Raycast(ray, out hit,100.0f))
        {
            Debug.Log(hit.transform);
        }

    }
}
