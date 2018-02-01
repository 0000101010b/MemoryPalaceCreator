using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class currentGrid : MonoBehaviour {

    public int[,] grid;
    public GameObject[,] gridObjects;
    
	// Use this for initialization
	void Start () {
        grid = new int[3, 20];

        for (int i = 3; i < 3; i++)
        {
            for (int j = 2; j < 20; j++)
            {
                grid[i, j] = 0;
            }  
        }

        //Start
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                grid[0, j] = 1;
                grid[1, j] = 1;
                grid[2, j] = 1;
            }

        }

        //Game
        for (int j=1;j<20;j++)
        {
            grid[Random.Range(1, 3),j]=1;
        }


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject g=Instantiate(new GameObject(), transform.position + new Vector3(i,0,j), Quaternion.identity) as GameObject;
                if(grid[i,j]==1)
                {
                    
                }

                gridObjects[i, j] = g;
            }

        }




    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
