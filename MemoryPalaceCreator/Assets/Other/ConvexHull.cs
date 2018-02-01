using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ConvexHull : MonoBehaviour
{
    List<Vector2> leftSide;
    List<Vector2> rightSide;


    [Header ("Convex Hull")]
    public int n;
    public int height;
    public int width;

    List<Vector2> points;
    List<Vector2> hull;

    // Use this for initialization
    void Start()
    {

        Time.timeScale = 0;
        points = new List<Vector2>();
        for (int i = 0; i < n; i++)
        {
            points.Add(new Vector2(Random.Range(0.0f, width), Random.Range(0.0f, height)));
        }
        hull = convexHull(points);
        //hull = SlowConvexHull(points);
        /*
        leftSide = new List<Vector2>();
        rightSide = new List<Vector2>();
        RandomLine(points);
        */
    }

    void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
             points.Clear();
             for (int i = 0; i < n; i++)
             {
                 points.Add(new Vector2(Random.Range(0.0f, width), Random.Range(0.0f, height)));
             }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
           /* points.Clear();
            for (int i = 0; i < n; i++)
            {
                points.Add(new Vector2(Random.Range(0.0f, width), Random.Range(0.0f, height)));
            }*/

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            hull = convexHull(points);
            stopWatch.Stop();
            Debug.Log("Convex Hull "+stopWatch.Elapsed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
           /* points.Clear();
            for (int i = 0; i < n; i++)
            {
                points.Add(new Vector2(Random.Range(0.0f, width), Random.Range(0.0f, height)));
            }*/
            System.Diagnostics.Stopwatch stopWatch=new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            hull = SlowConvexHull(points);
            stopWatch.Stop();
            Debug.Log("Slow Convex Hull " + stopWatch.Elapsed);
        }
    }



    void OnDrawGizmos()
    {
        if (hull != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < hull.Count; i++)
            {
                Gizmos.DrawLine((Vector3)hull[i] + new Vector3(0, 0, 10f), (Vector3)hull[(i + 1) % hull.Count] + new Vector3(0, 0, 10f));
            }
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hull[0], 14f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hull[1], 20f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hull[hull.Count - 1], 15f);
        }
        if (points != null)
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawWireCube(points[i], new Vector3(15f, 15f, 15f));
            }
        }
        if(leftSide!=null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < leftSide.Count; i++)
            {
                Gizmos.DrawWireCube(leftSide[i], new Vector3(15f, 15f, 15f));
            }
        }
        if (rightSide != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < rightSide.Count; i++)
            {
                Gizmos.DrawWireCube(rightSide[i], new Vector3(15f, 15f, 15f));
            }
        }
    }


    void RandomLine(List<Vector2> p)
    {
        Vector2 p1 = p[0];
        Vector2 p2 = p[1];

        Debug.DrawLine(p1, p2, Color.red, 1000f, true);
        Vector2 dir = (p1 - p2).normalized;
        dir = dir.Rotate(90f);

        Vector2 pointCheck;


        for (int i = 0; i < p.Count; i++)
        {
            pointCheck = (p1 + p2) / 2;
            pointCheck = (p[i] - pointCheck).normalized;

            float dot = Vector2.Dot(dir, pointCheck);
            if (dot > 0.2f)
            {
                rightSide.Add(p[i]);

            }
            else if (dot < -0.2f)
            {
                leftSide.Add(p[i]);
            }
        }
    }

    List<Vector2> convexHull(List<Vector2> p)
    {
        p = p.OrderBy(v => v.x).ThenBy(v => v.y).ToList();
        List<Vector2> l_upper = new List<Vector2>();

        l_upper.Add(p[0]);
        l_upper.Add(p[1]);



        for (int i = 2; i < p.Count; i++)
        {
            l_upper.Add(p[i]);

            if (l_upper.Count > 2)
            {
                Vector2 dir = (l_upper[l_upper.Count - 3] - l_upper[l_upper.Count - 2]).normalized;
                dir = dir.Rotate(90f);
                Vector2 pointCheck = (l_upper[l_upper.Count - 3] + l_upper[l_upper.Count - 2]) / 2;
                pointCheck = (l_upper[l_upper.Count - 1] - pointCheck).normalized;
                float dot = Vector2.Dot(dir, pointCheck);


                while (dot < 0 && l_upper.Count > 2)//while left turn upperhull
                {

                    l_upper.RemoveAt(l_upper.Count - 2);

                    if (l_upper.Count > 2)
                    {
                        dir = (l_upper[l_upper.Count - 3] - l_upper[l_upper.Count - 2]).normalized;
                        dir = dir.Rotate(90f);
                        pointCheck = (l_upper[l_upper.Count - 3] + l_upper[l_upper.Count - 2]) / 2;
                        pointCheck = (l_upper[l_upper.Count - 1] - pointCheck).normalized;
                        dot = Vector2.Dot(dir, pointCheck);
                    }

                }
            }
        }
        List<Vector2> l_lower = new List<Vector2>();
        l_lower.Add(p[p.Count - 1]);
        l_lower.Add(p[p.Count - 2]);

        for (int i = p.Count - 3; i > -1; i--)
        {
            l_lower.Add(p[i]);


            if (l_lower.Count > 2)
            {
                Vector2 dir = (l_lower[l_lower.Count - 3] - l_lower[l_lower.Count - 2]).normalized;
                dir = dir.Rotate(90f);
                Vector2 pointCheck = (l_lower[l_lower.Count - 3] + l_lower[l_lower.Count - 2]) / 2;
                pointCheck = (l_lower[l_lower.Count - 1] - pointCheck).normalized;
                float dot = Vector2.Dot(dir, pointCheck);

                while (dot < 0 && l_lower.Count > 2)//while left turn upperhull
                {
                    l_lower.RemoveAt(l_lower.Count - 2);

                    if (l_lower.Count > 2)
                    {
                        dir = (l_lower[l_lower.Count - 3] - l_lower[l_lower.Count - 2]).normalized;
                        dir = dir.Rotate(90f);
                        pointCheck = (l_lower[l_lower.Count - 3] + l_lower[l_lower.Count - 2]) / 2;
                        pointCheck = (l_lower[l_lower.Count - 1] - pointCheck).normalized;
                        dot = Vector2.Dot(dir, pointCheck);
                    }
                }
            }
        }

        l_lower.RemoveAt(l_lower.Count - 1);
        l_upper.RemoveAt(l_upper.Count - 1);


        foreach (Vector2 v in l_lower)
        {
            l_upper.Add(v);
        }

        return l_upper;
    }


    List<Vector2> SlowConvexHull(List<Vector2> p)
    {
        List<Vector2> output = new List<Vector2>();
        int p1 = -1;

        bool right = true;

        for (int i = 0; i < p.Count; i++)
        {
            for (int j = 0; j < p.Count; j++)
            {
                right = false;
                if (i != j)
                {
                    Vector2 dir = (p[i] - p[j]).normalized;
                    dir = dir.Rotate(90f);


                    for (int x = 0; x < p.Count; x++)
                    {
                        if (x != i && j != x)
                        {
                            Vector2 pointCheck = (p[i] + p[j]) / 2;

                            pointCheck = (p[x] - pointCheck).normalized;

                            float dot = Vector2.Dot(dir, pointCheck);

                            if (dot > 0)
                            {
                                right = true;
                                break;
                            }

                        }

                    }

                    if (!right)
                    {
                        p1 = j;
                        output.Add(p[i]);
                        output.Add(p[j]);
                        break;
                    }
                }
            }

            if (!right)
                break;
        }


        for (int j = 0; j < p.Count; j++)
        {
            if (p1 != j)
            {
                Vector2 dir = (p[p1] - p[j]).normalized;
                dir = dir.Rotate(90f);

                right = false;
                for (int x = 0; x < p.Count; x++)
                {
                    if (x != j && x != p1)
                    {
                        Vector2 pointCheck = (p[p1] + p[j]) / 2;
                        pointCheck = (p[x] - pointCheck).normalized;

                        float dot = Vector2.Dot(dir, pointCheck);

                        if (dot > 0)
                        {
                            right = true;
                            break;
                        }
                    }
                }
                if (!right)
                {
                    if (Vector2.Distance(p[j], output[0]) < .01f)
                    {
                        break;
                    }
                    else
                    {
                        p1 = j;
                        output.Add(p[j]);
                        j = 0;
                    }

                }

            }
        }



        return output;
    }

}


public static class Vector2Extension
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}