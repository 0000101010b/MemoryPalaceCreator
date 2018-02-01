using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PolyGrid :Grid {

    [Header("Grid Variables")]
    public int gridWidthBreath;
    public float gridZmagnitude = 1f;
    public float gridXYmagnitude = 1f;
    public Vector3 xDir, yDir;//noramlised direction of x and y of the grid


    [Header("Debug perimeter polygon")]
    public bool isDebugMode;
    public float pdMin, pdMax, r, pV;


    [Header("Perimeter polygon Variables")]
    public List<Vector3> polygon;
    public Vector3 centroid;
    GameObject prefab;

    public void PolyGridConstructor(Vector3 _pos,int mapWidthBreath,GameObject _prefab)
    {
        GridConstructor(_pos, mapWidthBreath,mapWidthBreath);
        prefab = _prefab;
        gridWidthBreath = mapWidthBreath;
    }

    public Vector3 gridPos(int i, int j)
    {
        /*
        Debug.Log(xDir);
        Debug.Log("pos"+pos);
        Debug.Log("gridWidth Breath" + gridWidthBreath);
        Debug.Log("XY" + gridXYmagnitude);
        */
        return (xDir * (i - (gridWidthBreath / 2)) * gridXYmagnitude) + (yDir *(j - (gridWidthBreath / 2)) * gridXYmagnitude) + pos;
    }

    public override void CreateGrid()
    {
        Debug.Log("run");
        if (isDebugMode)
        {
            polygon = new List<Vector3>();
            polygon = MyVector3Lib.MakeRandomPolygon(pos, pdMin, pdMax, r, pV);
            centroid = MyVector3Lib.CalculateCentroidSimplePolygon(polygon);
        }

        //zero the gridValues array
        Array.Clear(gridValues, 0, gridValues.Length);

        #region set X and Y directions in the grid

        xDir = UnityEngine.Random.insideUnitSphere;
        xDir.y = 0;
        xDir.Normalize();
        yDir = Quaternion.AngleAxis(90.0f, Vector3.up) * xDir;
        #endregion

        #region Set Grid areas in polygon to build areas 
        Vector3 current;
        for (int i = -gridWidthBreath / 2; i < gridWidthBreath / 2; i++)
        {
            for (int j = -gridWidthBreath / 2; j < gridWidthBreath / 2; j++)
            {
                current = (xDir * i * gridXYmagnitude) + (yDir * j * gridXYmagnitude) + pos;
                if (MyVector3Lib.ContainsPoint(polygon, new Vector2(current.x, current.z)))
                {
                    isbuildArea[i + gridWidthBreath / 2, j + gridWidthBreath / 2] = true;
                }
                else
                {
                    isbuildArea[i + gridWidthBreath / 2, j + gridWidthBreath / 2] = false;
                }
            }
        }
        #endregion

        Display();
    }
    public override void Build(int i, int j,Vector2 _buildSpace)
    {
        GameObject g=new GameObject("wall Mesh");
        Shed s =g.AddComponent<Shed>();
        //float height=(float)UnityEngine.Random.Range(3, 25);
        float height = 3.0f;
        s.ShedConstructor(gridPos(i, j), _buildSpace,xDir,yDir,height);
        
    }

    void Display()
    {
        for (int i = 0; i < polygon.Count; i++)
        { 
           // Debug.DrawLine(polygon[i], polygon[(i + 1) % polygon.Count],Color.yellow,200000f,true);
           // Gizmos.DrawLine(polygon[i], polygon[(i + 1) % polygon.Count]);
        }
        #region Find and Draw Grid Cyan
        if (isbuildArea != null)
        {
            Gizmos.color = Color.cyan;
            bool startVectorFound = false, stopVectorFound = false;
            Vector3 startVector = Vector3.zero;
            Vector3 endVector = Vector3.zero;

            for (int i = 0; i < gridWidthBreath; i++)
            {
                for (int j = 0; j < gridWidthBreath; j++)
                {
                    if (!startVectorFound && !stopVectorFound && isbuildArea[i, j])
                    {
                        startVectorFound = true;
                        startVector = (xDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (yDir * (j - gridWidthBreath / 2) * gridXYmagnitude) + pos;
                    }

                    if (startVectorFound && !stopVectorFound && !isbuildArea[i, j])
                    {
                        stopVectorFound = true;
                        endVector = (xDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (yDir * (j - gridWidthBreath / 2 - 1) * gridXYmagnitude) + pos;
                    }

                    if (stopVectorFound && startVectorFound)
                    {
                        //  Debug.DrawLine(startVector, endVector, Color.cyan, 200000f, true);
                        for(int z=0;z<20;z++)
                        {
                            GameObject r = Instantiate(prefab, startVector, Quaternion.identity) as GameObject;
                            r.transform.forward = xDir;
                            r.transform.position += new Vector3(0, .5f+z, 0f);
                            r.transform.Rotate(Vector3.up, -90f);
                            r.transform.position -= yDir / 2;
                            r.GetComponent<Renderer>().material.color = Color.blue;
                        }
                        for (int z = 0; z < 20; z++)
                        {
                            GameObject r = Instantiate(prefab, endVector, Quaternion.identity) as GameObject;
                            r.transform.forward = xDir;
                            r.transform.position += yDir / 2;
                            r.transform.position += new Vector3(0, .5f + z, 0f);
                            r.transform.Rotate(Vector3.up, 90f);
                            r.GetComponent<Renderer>().material.color = Color.blue;
                        }


                        startVectorFound = stopVectorFound = false;
                    }


                }
            }
            Gizmos.color = Color.red;
            Vector3 startV2 = Vector3.zero;
            Vector3 endV2 = Vector3.zero;
            for (int i = 0; i < gridWidthBreath; i++)
            {
                for (int j = 0; j < gridWidthBreath; j++)
                {
                    if (!startVectorFound && !stopVectorFound && isbuildArea[j, i])
                    {
                        startVectorFound = true;
                        startV2 = (yDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (xDir * (j - gridWidthBreath / 2) * gridXYmagnitude) + pos;
                    }

                    if (startVectorFound && !stopVectorFound && !isbuildArea[j, i])
                    {
                        stopVectorFound = true;
                        endV2 = (yDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (xDir * (j - gridWidthBreath / 2 - 1) * gridXYmagnitude) + pos;
                      
                    }

                    if (stopVectorFound && startVectorFound)
                    {

                        // Debug.DrawLine(startV2, endV2, Color.blue, 200000f, true);
                        for (int z = 0; z < 20; z++)
                        {
                            GameObject r = Instantiate(prefab, startV2, Quaternion.identity) as GameObject;
                            r.transform.forward = xDir;
                            r.transform.position += new Vector3(0, .5f + z, 0f);
                            r.transform.position -= xDir/2;
                            r.transform.Rotate(Vector3.up, 180f);

                            r.GetComponent<Renderer>().material.color = Color.blue;
                        }
                        for (int z = 0; z < 20; z++)
                        {
                            GameObject r = Instantiate(prefab, endV2, Quaternion.identity) as GameObject;
                            r.transform.forward = xDir;
                            r.transform.position += new Vector3(0, .5f + z, 0f);
                            r.transform.Rotate(Vector3.up, 0f);
                            r.transform.position += xDir/2;
                            r.GetComponent<Renderer>().material.color = Color.blue;
                        }
                        startVectorFound = stopVectorFound = false;
                    }


                }
            }
        }
        #endregion

        if (isbuildArea != null)
        {
            Gizmos.color = Color.green;
            for (int i = -gridWidthBreath / 2; i < gridWidthBreath / 2; i++)
            {
                for (int j = -gridWidthBreath / 2; j < gridWidthBreath / 2; j++)
                {
                    if (isbuildArea[i + gridWidthBreath / 2, j + gridWidthBreath / 2])
                    {
                        Vector3 current = (xDir * i * gridXYmagnitude) + (yDir * j * gridXYmagnitude) + pos;
                       
                        GameObject g=Instantiate(prefab, current, Quaternion.identity)as GameObject;
                        g.transform.forward = xDir;
                        g.transform.Rotate(Vector3.right, 90f);
                       
                        g.GetComponent<Renderer>().material.color=Color.green;

                        GameObject r = Instantiate(prefab, current, Quaternion.identity) as GameObject;
                        r.transform.forward = xDir;
                   
                        r.transform.Rotate(Vector3.right, -90f);
                        r.transform.position += new Vector3(0,20f,0f);
                        r.GetComponent<Renderer>().material.color = Color.blue;


                    }

                }
            }
        }

    }
    void OnDrawGizmos()
    {
        #region Display grid X direction yellow Y direction green
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(centroid, centroid + xDir);
        Debug.Log(xDir);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(centroid, centroid + yDir);
        #endregion

        #region Draw perimeter
        Gizmos.color = Color.yellow;
        for (int i = 0; i < polygon.Count; i++)
        {
            Gizmos.DrawLine(polygon[i], polygon[(i + 1) % polygon.Count]);
        }
        #endregion

        #region Find and Draw Grid Cyan
        if (isbuildArea != null)
        {
            Gizmos.color = Color.cyan;
            bool startVectorFound = false, stopVectorFound = false;
            Vector3 startVector = Vector3.zero;
            Vector3 endVector = Vector3.zero;

            for (int i = 0; i < gridWidthBreath; i++)
            {
                for (int j = 0; j < gridWidthBreath; j++)
                {
                    if (!startVectorFound && !stopVectorFound && isbuildArea[i, j])
                    {
                        startVectorFound = true;
                        startVector = (xDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (yDir * (j - gridWidthBreath / 2) * gridXYmagnitude) + pos;
                    }

                    if (startVectorFound && !stopVectorFound && !isbuildArea[i, j])
                    {
                        stopVectorFound = true;
                        endVector = (xDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (yDir * (j - gridWidthBreath / 2 - 1) * gridXYmagnitude) + pos;
                    }

                    if (stopVectorFound && startVectorFound)
                    {
                        Gizmos.DrawLine(startVector, endVector);
                        startVectorFound = stopVectorFound = false;
                    }


                }
            }
            Gizmos.color = Color.red;
            Vector3 startV2 = Vector3.zero;
            Vector3 endV2 = Vector3.zero;
            for (int i = 0; i < gridWidthBreath; i++)
            {
                for (int j = 0; j < gridWidthBreath; j++)
                {
                    if (!startVectorFound && !stopVectorFound && isbuildArea[j, i])
                    {
                        startVectorFound = true;
                        startV2 = (yDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (xDir * (j - gridWidthBreath / 2) * gridXYmagnitude) + pos;
                    }

                    if (startVectorFound && !stopVectorFound && !isbuildArea[j, i])
                    {
                        stopVectorFound = true;
                        endV2 = (yDir * (i - gridWidthBreath / 2) * gridXYmagnitude) + (xDir * (j - gridWidthBreath / 2 - 1) * gridXYmagnitude) + pos;
                        
                    }

                    if (stopVectorFound && startVectorFound)
                    {
                        Gizmos.DrawLine(startV2, endV2);
                        startVectorFound = stopVectorFound = false;
                    }


                }
            }
        }
        #endregion

        #region Show build areas Green
        if (isbuildArea != null)
        {
            Gizmos.color = Color.green;
            for (int i = -gridWidthBreath / 2; i < gridWidthBreath / 2; i++)
            {
                for (int j = -gridWidthBreath / 2; j < gridWidthBreath / 2; j++)
                {
                    if (isbuildArea[i + gridWidthBreath / 2, j + gridWidthBreath / 2])
                    {
                        Vector3 current = (xDir * i * gridXYmagnitude) + (yDir * j * gridXYmagnitude) + pos;
                        Gizmos.DrawWireSphere(current, .1f);

                    }

                }
            }
        }
        #endregion
    }
}
