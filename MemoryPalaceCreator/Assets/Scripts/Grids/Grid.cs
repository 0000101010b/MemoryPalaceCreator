using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Grid:MonoBehaviour{

    public Vector3 pos;
    public bool[,] isbuildArea;//is build area or not
    public int[,] gridValues; //area category
    //public int gridWidthBreath;
    public int gridWidth;
    public int gridBreath;

    abstract public void CreateGrid();


    public void GridConstructor(Vector3 pos,int gridWidth,int gridBreath)
    {
        this.pos = pos;
        //this.gridWidthBreath = gridWidthBreath;

        this.gridWidth = gridWidth;
        this.gridBreath = gridBreath;
        isbuildArea = new bool[gridWidth, gridBreath];
        gridValues = new int[gridBreath, gridBreath];

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridBreath; j++)
            {
                gridValues[i, j] = 0;
            }
        }
    }

    public void FindBuildSpaceUniform()
    {
        #region Find and build in building areas

        //list of lists for chosing random postions for buildings
        List<List<int>> ij = new List<List<int>>();//stores the j position indexes
        List<int> il = new List<int>();//stores list currenty around index as it should be on the grid

        for (int i = 0; i <gridWidth ; i++)
        {
            ij.Add(new List<int>());
            for (int j = 0; j < gridBreath; j++)
            {
                ij[i].Add(j);
            }

            il.Add(i);
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridBreath; y++)
            {
                //pick a random  space
                int i = UnityEngine.Random.Range(0, ij.Count - 1);
                int j = UnityEngine.Random.Range(0, ij[i].Count - 1);

                checkForBuildingSpace(il[i], ij[i][j]);//if space exists it creates a building

                ij[i].RemoveAt(j);//remove the chosen element

                if (ij[i].Count == 0)//if list empty then 
                {
                    ij.RemoveAt(i);
                    il.RemoveAt(i);
                }

            }

        }
        #endregion
    }

    void checkForBuildingSpace(int i, int j)
    {
        //get random from random chosen building type
        int buildingXmax = Random.Range(10, 26);
        int buildingZmax = Random.Range(5, 18);

        if (gridValues[i, j] != -1 && isbuildArea[i, j])
        {
            bool build = true;
            for (int w = -1; w < buildingXmax + 1; w++)
            {
                for (int y = -1; y < buildingZmax + 1; y++)
                {
                    if (!isbuildArea[i + w, j + y] || gridValues[i + w, j + y] == -1)
                    {
                        build = false;
                        break;
                    }
                }
            }

            if (build)
            {
                Build(i,j,new Vector2(buildingXmax,buildingZmax));
                /*
                   int floors = UnityEngine.Random.Range(floorMin, floorMax);
                   int width = UnityEngine.Random.Range(buildingXmin, buildingXmax);
                   int breath = UnityEngine.Random.Range(buildingZmin, buildingZmax);
                */
                //CreateBuilding(i, j, floors, width, breath);
                for (int w = 0; w < buildingXmax; w++)
                {
                    for (int _y = 0; _y < buildingZmax; _y++)
                    {
                        gridValues[i + w, j + _y] = -1;
                    }
                }

            }
        }
    }    
    public abstract void Build(int i, int j,Vector2 buildspace);
}

