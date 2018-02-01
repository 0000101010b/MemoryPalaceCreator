using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SineWave : MonoBehaviour {

    List<Vector3> p;
    LineRenderer l;
    public float startWidth;
    public float endWidth;

    // Use this for initialization
    void Start() {

        l = gameObject.GetComponent<LineRenderer>();
        l.SetWidth(startWidth, endWidth);
        p = new List<Vector3>();
        for (float theta = 0.0f; theta < Mathf.PI * Mathf.Rad2Deg; theta++)
        {
            p.Add(new Vector3(theta, Mathf.Sin(theta)));
        }
        l.SetVertexCount(180);
        l.SetPositions(p.ToArray());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
