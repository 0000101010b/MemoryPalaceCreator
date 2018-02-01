using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyVector3Lib : MonoBehaviour {

    public static List<Vector3> MakeRandomPolygon(Vector3 center, float pdMin, float pdMax, float r, float pv)
    {

        List<Vector3> points = new List<Vector3>();

        Vector3 pointB = Random.insideUnitSphere;
        Vector3 start = pointB;
        pointB = pointB.normalized;
        Vector3 pointA = Vector3.forward;


        float dot = Vector3.Dot(pointA, pointB);
        float f = dot / (pointA.magnitude * pointB.magnitude);
        f = Mathf.Acos(f);
        float theta = Mathf.Rad2Deg * f;
        float lrDir = AngleDir(pointA, pointB, Vector3.up);

        if (lrDir == -1.0f)
        {
            f = (-1 * theta) * Mathf.Deg2Rad;
        }

        theta = Mathf.Rad2Deg * f; ;
        float i;
        for (i = theta; i < 180; i += Random.Range(pdMin, pdMax))
        {
            float angle = i * Mathf.Deg2Rad;
            Vector3 temp = Vector3.zero;
            temp.x = (float)Mathf.Sin(angle);
            temp.z = (float)Mathf.Cos(angle);
            temp *= Random.Range(r - pv, r);
            points.Add(temp + center);
        }

        i = ((i % 180) - 180);

        for (; i < theta; i += Random.Range(pdMin, pdMax))
        {
            float angle = i * Mathf.Deg2Rad;
            Vector3 temp = Vector3.zero;
            temp.x = (float)Mathf.Sin(angle);
            temp.z = (float)Mathf.Cos(angle);
            temp *= Random.Range(r - pv, r);
            points.Add(temp + center);
        }

        return points;

    }

    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f)
        {
            return 1.0f;
        }
        else if (dir < 0.0f)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }
    }

    public static Vector3 CalculateCentroidSimplePolygon(List<Vector3> polygon)
    {
        Vector3 centroid = Vector3.zero;
        foreach (Vector3 v in polygon)
        {
            centroid += v;
        }
        centroid = centroid / polygon.Count;
        return centroid;
    }

    public static bool ContainsPoint(List<Vector3> poly, Vector2 p)
    {
        var j = poly.Count - 1;
        var inside = false;
        for (int i = 0; i < poly.Count; j = i++)
        {
            if (((poly[i].z <= p.y && p.y < poly[j].z) || (poly[j].z <= p.y && p.y < poly[i].z)) &&
                 (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].z) / (poly[j].z - poly[i].z) + poly[i].x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

}
