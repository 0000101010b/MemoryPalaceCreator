using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    public Vector3 center;
    float area;
    float angle;

    public float betaDegree;
    public float alphaDegree;

    public Node(Vector3 _center,float _area,float _angle)
    {
        center = _center;
        area   = _area;
        angle  = _angle;
    }

    public void createTree(int n,float s,float alphaVal, float betaVal)
    {
        if(n>0)
        {
            alphaDegree = alphaVal;
            betaDegree = betaVal;

            n--;
            float s1 = s * Mathf.Sin(Mathf.Deg2Rad * alphaDegree);
            Vector3 c1 =ComputeCenter(center,s,s1,alphaDegree,angle,true);
            alpha = new Node(c1, s1, (angle-alphaDegree) % 360);
            alpha.createTree(n, s1,alphaVal,betaVal);
            

            float s2 = s * Mathf.Sin(Mathf.Deg2Rad * betaDegree);
            Vector3 c2=ComputeCenter(center, s, s2, betaDegree , angle,false);
            beta = new Node(c2, s2, ( angle+betaDegree) % 360);
            beta.createTree(n, s2,alphaVal,betaVal);

        }
            
    }

   



    public Node beta;
    public Node alpha;
    
    public Vector3 ComputeCenter(Vector3 center,float s,float s1,float degree,float cDegree, bool isAlpha)
    {
        int isAlphaInt = 1;
        if (isAlpha)
            isAlphaInt *= -1;


        Vector3 dir;
        Vector3 result;

        //initial step
        dir=new Vector3(
            (float)Mathf.Cos(cDegree * Mathf.Deg2Rad),
            (float)Mathf.Sin(cDegree * Mathf.Deg2Rad)
            ,0);
        result = center + (s / 2 * dir);

        //side step
        dir=Quaternion.AngleAxis(90*isAlphaInt,Vector3.forward)*dir;
        result += (s / 2) * dir;


        //degree step
        dir = new Vector3(
              (float)Mathf.Cos(degree * Mathf.Deg2Rad),
              (float)Mathf.Sin(degree * Mathf.Deg2Rad)
              , 0);
        result += (s1 / 2 * dir);
        
        //forward step 2
        dir = Quaternion.AngleAxis(90, Vector3.forward) * dir;
        result += (s1 / 2 * dir);

        return result;

    }   
       
}



public class PythagorousTree : MonoBehaviour {

    public float timeSpeed;
    public float alphaVal;
    public float betaVal;
    public Node _start;
    public int n;
    public float s;
    public Color c1;
    public Color c2;

    void OnDrawGizmos()
    {
        Node temp = _start;

       if(temp!=null)
        {
            drawTree(temp);
        }
       
    }

    void drawTree(Node temp)
    {
        if (temp.alpha!=null)
        {
            Gizmos.color = c1;
            Gizmos.DrawLine(temp.center+transform.position, temp.alpha.center+transform.position);
            Gizmos.color = c2;
            Gizmos.DrawLine(transform.position+temp.center, transform.position+temp.beta.center);
            drawTree(temp.alpha);
            drawTree(temp.beta);
        }
    }
    // Use this for initialization

    void Start()
    {
        StartCoroutine("Grow");
        //Tree();
    }
    void Tree () {

        _start = new Node(Vector3.zero, 5, 90 );
        _start.createTree(n,s,alphaVal,betaVal);
       
    }
	

    IEnumerator Grow()
    {
        while (true)
        {
            for (int theta = 0; theta < 46; theta++)
            {
                Tree();
                alphaVal++;
                betaVal++;
                alphaVal = alphaVal % 46;
                betaVal = betaVal % 46;
                yield return new WaitForSeconds(timeSpeed);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Space))
            Tree();
        
    }
}
