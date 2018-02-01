using UnityEngine;
using System.Collections;

public class BackgroundSpace : MonoBehaviour {

    public int n;
    public GameObject lattice;
    public float r1;
    public float r2;
	// Use this for initialization
	void Start () {
	
        for(int i=0;i<n;i++)
        {
            GameObject g=Instantiate(lattice,Random.insideUnitSphere*Random.Range(r1,r2), Quaternion.identity)as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
