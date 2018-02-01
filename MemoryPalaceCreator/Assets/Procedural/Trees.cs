using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trees : MonoBehaviour {

    public float radius;
    public int numberOfPoints;
    public float dist;

    List<Vector3> points;
    List<List<int>> attachedTo;
    bool[] isAttached;

    // Use this for initialization
    void Start()
    {
        Vector3 center = transform.position + new Vector3(0, radius, 0);
        points = new List<Vector3>();
        isAttached = new bool[numberOfPoints];
        points.Add(transform.position);

        isAttached[0] = false;

        for (int i = 0; i < numberOfPoints-1; i++)
        {
            isAttached[i] = false;
            points.Add(Random.insideUnitSphere * Random.Range(-radius, radius));
        }

        //Add stem
        attachedTo = new List<List<int>>();
        attachedTo.Add(new List<int>() { });

        for (int i = 0; i < points.Count; i++)
        {
            attachedTo = new List<List<int>>();
            attachedTo.Add(new List<int>() { });
        }


        int current = 0;
        bool pointFound = true;

        while (pointFound)
        {
            pointFound = false;
            for (int i = 0; i < points.Count; i++)
            {
                if (Vector3.Distance(points[current], points[i]) < dist && !isAttached[i])
                {
                    Debug.Log(i);
                    isAttached[i] = true;
                    pointFound = true;
                    attachedTo[current].Add(i);
                    current = i;
                }
               
            }

        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawWireSphere(points[i], .2f);
                if (attachedTo[i] != null)
                {
                    for (int j = 0; j < attachedTo[i].Count; j++)
                    {
                        Gizmos.DrawLine(points[i], points[attachedTo[i][j]]);
                    }
                }
            }
        }
    }

    
    }
