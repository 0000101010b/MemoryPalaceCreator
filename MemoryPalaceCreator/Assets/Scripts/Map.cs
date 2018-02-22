using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Map : MonoBehaviour {

    Grid grid;

    [Header("Grid Variables")]
    public int gridWidthBreath=150;
    public int gridtype=0;

    [Header("Poly Grid Variables")]
    public float pdMin;
    public float pdMax;
    public float r;
    public float pV;

    [Header("BSP Grid Variables")]
    public int x;
    public int y;
    public int minLeafSize;
    public int maxLeafSize;
    public float mag;

    [Header("Prefabs")]
    public GameObject prefabGround;

    void Awake()
    {
        //grid = gameObject.AddComponent<PolyGrid>();
        grid = gameObject.AddComponent<BSPGrid>();

        if(grid is BSPGrid)
        {
            BSPGrid bspGrid = (BSPGrid)grid;
            bspGrid.gridWidth = x;
            bspGrid.gridBreath = y;
            bspGrid.Init(minLeafSize, maxLeafSize, mag);
            bspGrid.CreateGrid();


            List<Vector3> doneAlready= new List<Vector3>();
            bspGrid.CreateBuildings(bspGrid.root,ref doneAlready);
            
            //bspGrid.CreateRoads();
        }

        if (grid is PolyGrid)
        {
            PolyGrid polyGrid = (PolyGrid)grid;
            polyGrid.PolyGridConstructor(transform.position,gridWidthBreath,prefabGround);
            polyGrid.isDebugMode = true;
            polyGrid.pdMin = pdMin;
            polyGrid.pdMax = pdMax;
            polyGrid.r = r;
            polyGrid.pV = pV;
            polyGrid.CreateGrid();
            polyGrid.FindBuildSpaceUniform();
        }

        string nameToAdd = "New Game Object";
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == nameToAdd)
                    Destroy(go);
        }
           
    }
   

}
