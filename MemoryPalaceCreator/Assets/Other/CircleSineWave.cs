using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleSineWave : MonoBehaviour {

    LineRenderer l;

    public float speed;
    public float startWidth;
    public float endWidth;

    public float scale;
    public int divisor;
    public float waveRepeatDegree;
    List<Vector2> p;
    List<Vector3> c;

	// Use this for initialization
	void Start () {

        l = gameObject.GetComponent<LineRenderer>();
        l.SetWidth(startWidth,endWidth);
        p = new List<Vector2>();
        for(float theta=0.0f;theta<Mathf.PI*Mathf.Rad2Deg;theta++)
        {
            if(theta > Mathf.PI * Mathf.Rad2Deg)


            p.Add(new Vector2(theta, Mathf.Sin(theta)));
        }



        
        c = new List<Vector3>();
        /*for(int i=0;i<divisor;i++)
        {
            float angle = i * (Mathf.PI * Mathf.Rad2Deg *2)/divisor;
            Vector3 current = Quaternion.AngleAxis(angle, transform.forward) * transform.up;
            Vector3 temp = current;
            current *= scale;
            temp*=Mathf.Sin(angle);
            current = temp + current;
            c.Add(current);
        }
        */
        StartCoroutine("Draw");

	}
	
    void OnDrawGizmos()
    {
        if (p != null)
        {
            for (int i = 0; i < p.Count - 1; i++)
            {
                Gizmos.DrawLine(p[i], p[i + 1]);
            }
        }
        if (c != null)
        {
            for (int i = 0; i < c.Count; i++)
            {
               /* if(Vector2.Distance(c[i],c[0]) <.3f && i>20)
                {
                    Gizmos.DrawLine(c[i], c[0]);
                    break;
                }*/
            
                Gizmos.DrawLine(c[i], c[(i + 1) % c.Count]);
            }

        }

    }
    void CircleSine()
    {
        c.Clear();
        int sign=1;
        l.SetVertexCount(divisor+1);
        for (int i = 0; i < divisor; i++)
        {
            float angle = i * (Mathf.PI * Mathf.Rad2Deg * 2) / divisor;
            Vector3 current = Quaternion.AngleAxis(angle, transform.forward) * transform.up;
            Vector3 temp = current;
            current *= scale;

            if (angle % waveRepeatDegree == 0)
                sign *= -1;

            temp *= angle % waveRepeatDegree*sign;
            current = temp + current;
            c.Add(current+transform.position);
        }
        l.SetPositions(c.ToArray());
        l.SetPosition(c.Count, c[0]);
    }

    void distanceWave()
    {
        c.Clear();
        
        for (int i = 0; i < divisor+1; i++)
        {
            float angle = i * (Mathf.PI * Mathf.Rad2Deg * 2) / divisor;
            Vector3 current = Quaternion.AngleAxis(angle, transform.forward) * transform.up;
            Vector3 temp = current;
            current *= scale;
            

            temp *= Mathf.Sin(angle);
            Debug.Log(angle);
            current = temp + current;
            
            c.Add(current+transform.position);
           
        }

    }
         

    IEnumerator Draw()
    {
        while (true)
        {
            while (scale < 50)
            {
                scale += 1;
                CircleSine();
                yield return new WaitForSeconds(.1f);
            }
            while (scale > 3)
            {
                scale -= 1;
                CircleSine();
                yield return new WaitForSeconds(.1f);
            }
        }
    }
	// Update is called once per frame
	void Update () {

        transform.Rotate(transform.forward, speed * Time.deltaTime);

	}
}
