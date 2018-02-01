using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fractal : MonoBehaviour {



    public float maxRadius;
    public int n;




    void circle(int n,Vector2 pos,float radius)
    {
        if(n>0 && radius != 0 && n<16)
        {

            //Gizmos.DrawWireSphere(pos, radius);
            Gizmos.DrawWireCube(pos, new Vector3(radius, radius, radius));

            n--;

            Vector2 left = new Vector2(-radius,0)+pos;
            circle(n, left, radius / 2);

            Vector2 right = new Vector2(radius,0)+pos;
            circle(n, right, radius / 2);
        }
       
    }

    void OnDrawGizmos()
    {
        circle(n,transform.position,maxRadius);
    }
	
	// Update is called once per frame
	void Update () {

        maxRadius += Time.deltaTime * 200f;
	}
}
