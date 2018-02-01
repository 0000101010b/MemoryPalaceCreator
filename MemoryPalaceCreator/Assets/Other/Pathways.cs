using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathways : MonoBehaviour {


    public Color c1;
    public Color c2;
    public Material m;
    public float width;
    public float height;

    public float divisor;
    public GameObject center;
    public GameObject player;

    List<GameObject> rings;
    List<Vector3> ring; 
	// Use this for initialization
	void Start () {
        ring = new List<Vector3>();

        GameObject g=new GameObject("Side Ring");
        LineRenderer l=g.AddComponent<LineRenderer>();
        l.material = m;
        l.SetColors(c1, c1);
        l.SetWidth(width, height);
        l.SetVertexCount((int)divisor+1);
        float div = divisor;
        Vector3 toPlayer = player.transform.position - center.transform.position;
        divisor = (Mathf.PI*Mathf.Rad2Deg * 2.0f )/ divisor;
        
        for(float theta=0.0f;theta<Mathf.PI*2*Mathf.Rad2Deg;theta+=divisor)
        {
            Vector3 v=Quaternion.AngleAxis(theta, player.transform.up)*toPlayer.normalized;
            v=center.transform.position + toPlayer.magnitude * v;
            ring.Add(v);

            List<Vector3> ring2 = new List<Vector3>();
            GameObject g2 = new GameObject("Over Ring");
            LineRenderer l2 = g2.AddComponent<LineRenderer>();
            l2.material = m;
            l2.SetColors(c1, c1);
            l2.SetWidth(.1f, .1f);
            l2.SetVertexCount((int)div + 1);

            Vector3 toPlayer2 = v - center.transform.position;
           // divisor = (Mathf.PI * Mathf.Rad2Deg * 2.0f) / divisor;

            for (float theta2 = 0.0f; theta2 < Mathf.PI * 2 * Mathf.Rad2Deg; theta2 += divisor)
            {
                Vector3 v2 = Quaternion.AngleAxis(theta2, Quaternion.AngleAxis(90, Vector3.up)*(center.transform.position - v).normalized ) * toPlayer2.normalized;
                v2 = center.transform.position + toPlayer.magnitude * v2;
                ring2.Add(v2);
            }
            ring2.Add(ring2[0]);
            l2.SetPositions(ring2.ToArray());


        }
        ring.Add(ring[0]);
        l.SetPositions(ring.ToArray());

        ring = new List<Vector3>();

        g = new GameObject("Over Ring");
        l = g.AddComponent<LineRenderer>();
        l.material = m;
        l.SetColors(c1, c1);
        l.SetWidth(width, height);
        l.SetVertexCount((int)div + 1);

        toPlayer = player.transform.position - center.transform.position;
       // divisor = (Mathf.PI * Mathf.Rad2Deg * 2.0f) / divisor;

        for (float theta = 0.0f; theta < Mathf.PI * 2 * Mathf.Rad2Deg; theta += divisor)
        {
            Vector3 v = Quaternion.AngleAxis(theta, player.transform.right) * toPlayer.normalized;
            v = center.transform.position + toPlayer.magnitude * v;
            ring.Add(v);
        }
        ring.Add(ring[0]);
        l.SetPositions(ring.ToArray());

    }
    void OnDrawGizmos()
    {
        if(ring!=null)
        {
            Gizmos.color = Color.cyan;
            for(int i=0;i<ring.Count;i++)
            {
                Gizmos.DrawLine(ring[i], ring[(i + 1) % ring.Count]);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
