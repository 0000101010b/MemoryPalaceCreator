using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shed : Residential {

    public GameObject shedGameobject;
    public float height;
 
    public void ShedConstructor(Vector3 _startPos, Vector2 _buildSpace, Vector3 xDir, Vector3 yDir, float _height)
    {
        height = _height;
        BuildingsConstructor(_buildSpace, _startPos);//base constructor

        Vector3 northWallpos = startPos + new Vector3(0, height / 2, 0) + (buildSpace.x / 2) * xDir;
        Vector3 eastWallpos = startPos + new Vector3(0, height / 2, 0) + (buildSpace.y / 2) * yDir;
        Vector3 westWallpos = startPos + new Vector3(0, height / 2, 0) + (buildSpace.y / 2) * yDir + (buildSpace.x * xDir);
        Vector3 southWallpos = startPos + new Vector3(0, height / 2, 0) + (buildSpace.x / 2) * xDir + (buildSpace.y * yDir);


        List<GameObject> outsideWalls= new List<GameObject>();
        List<GameObject> insideWalls = new List<GameObject>();

        Vector2[] rect = new Vector2[4];
        float[] rectH = new float[4];
        float[] rectW = new float[4];

        for(int i=0;i<4;i++)
        {
            rect[i] = new Vector2(0, 0);
            rectH[i] = 0;
            rectW[i] = 0;
        }

        int doorSide = Random.Range(0, 4);
        if (doorSide > 1)
        {
            rect[doorSide].x = Random.Range(1, (int)(buildSpace.y - 4));
            rectW[doorSide] = 1f;
            rectH[doorSide] = 2f;
        }
        else
        {
            rect[doorSide].x = Random.Range(1, (int)(buildSpace.x - 4));
            rectW[doorSide] = 1f;
            rectH[doorSide] = 2f;
        }


        GameObject doorway= new GameObject("D");

        //north wall
        outsideWalls.Add(MakePlane(false,northWallpos, yDir,buildSpace.x, height, 1.0f, 1.0f, rect[0], rectH[0], rectW[0]));
        insideWalls.Add(MakePlane(true,northWallpos + (yDir * 1.0f), -yDir, buildSpace.x-2f, height, 1.0f, 1.0f,rect[0],rectH[0],rectW[0]));
        if (rectH[0] == 2)
        { 
            Doorway dw=doorway.AddComponent<Doorway>();
            dw.Build();
            doorway.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
            doorway.transform.position = northWallpos + new Vector3(0.5f,-.5f,-(rect[0].x - (buildSpace.x / 2)+.5f) );
        }

        //south wall
        outsideWalls.Add(MakePlane(false,southWallpos,- yDir, buildSpace.x, height, 1.0f, 1.0f, rect[1], rectH[1], rectW[1]));
        insideWalls.Add(MakePlane(true,southWallpos - (yDir * 1.0f), yDir, buildSpace.x-2f, height, 1.0f, 1.0f, rect[1], rectH[1], rectW[1]));
        if (rectH[1] == 2)
        {
            Doorway dw = doorway.AddComponent<Doorway>();
            dw.Build();
            doorway.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
            doorway.transform.position = southWallpos+ new Vector3(-0.5f, -.5f, (rect[1].x - (buildSpace.x / 2) + .5f));
        }

        //eastWall
        outsideWalls.Add(MakePlane(false,eastWallpos, xDir, buildSpace.y, height, 1.0f, 1.0f, rect[2], rectH[2], rectW[2]));
        insideWalls.Add(MakePlane(true,eastWallpos + (xDir * 1.0f), -xDir, buildSpace.y-2f, height, 1.0f, 1.0f, rect[2], rectH[2], rectW[2]));
        
        if (rectH[2] == 2)
        {
            Doorway dw = doorway.AddComponent<Doorway>();
            dw.Build();
            doorway.transform.position = eastWallpos + new Vector3(rect[2].x - (buildSpace.y / 2)+.5f, -.5f,.5f);
        }

        //westWall
        outsideWalls.Add(MakePlane(false,westWallpos, -xDir, buildSpace.y, height, 1.0f, 1.0f, rect[3], rectH[3], rectW[3]));
        insideWalls.Add(MakePlane(true,westWallpos - (xDir * 1.0f), xDir, buildSpace.y-2f, height, 1.0f, 1.0f, rect[3], rectH[3], rectW[3]));

        if (rectH[3] == 2)
        {
            Doorway dw = doorway.AddComponent<Doorway>();
            dw.Build();
            doorway.transform.position = westWallpos + new Vector3(-rect[3].x + (buildSpace.y / 2) - .5f, -.5f, -.5f);
        }


        GameObject ceiling = MakePlane(false,startPos + new Vector3(0, height, 0) + ((buildSpace.y / 2) * yDir )+ ((buildSpace.x/2) * xDir),
            xDir, buildSpace.y, buildSpace.x, 1f, 1f, Vector2.zero, 0, 0);
        ceiling.name = "ceiling";
        ceiling.transform.Rotate(yDir, -90.0f,Space.World);
        ceiling.transform.SetParent(shedGameobject.transform);
        //for polygrid yDir plus transform position

        GameObject roof = MakePlane(false,startPos + new Vector3(0, height, 0) + (buildSpace.y / 2) * yDir + (buildSpace.x / 2) * xDir,
        xDir,
        buildSpace.y,
        buildSpace.x,
        1f, 1f, Vector2.zero, 0, 0);
        roof.name = "roof";
        roof.transform.Rotate(yDir,90.0f, Space.World);
        roof.transform.SetParent(shedGameobject.transform);

        Color c1 = new Color(Random.value, Random.value, Random.value);
        Color c2 = new Color(Random.value, Random.value, Random.value);
        //new Color(0.6f, 0.6f, 0.6f); 
        //new Color(Random.value, Random.value, Random.value);
        Color c3 = new Color(Random.value, Random.value, Random.value);

        //Rendering
        foreach (GameObject g in insideWalls)
        {
            WallMesh wallmesh=g.GetComponent<WallMesh>();
            wallmesh.SetWallTexture(c1);
            g.name = "Inside Wall";
            g.AddComponent<MeshCollider>();
            g.transform.SetParent(shedGameobject.transform);
        }
        foreach (GameObject g in outsideWalls)
        {
            WallMesh wallmesh = g.GetComponent<WallMesh>();
            wallmesh.SetWallTexture(c2);
            g.AddComponent<MeshCollider>();
            g.name = "Outside Wall";
            g.transform.SetParent(shedGameobject.transform);
        }

        
        WallMesh r = roof.GetComponent<WallMesh>();
        r.SetWallTexture(c2);
        /*
        WallMesh f = ceiling.GetComponent<WallMesh>();
        f.SetWallTexture(c3);
        */

        /*
        Mesh mesh=roof.GetComponent<MeshFilter>().mesh;

        Vector3[] baseHeight =mesh.vertices;
        Vector3[] vertices = new Vector3[baseHeight.Length];
        float noiseStrength = 1f;
        float noiseWalk = 1f;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * 10.0f + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * 5f;
            vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        */
        /*
        float scale = 0.1f;
        float speed = 1.0f;
        float noiseStrength = 1f;
        float noiseWalk = 1f;

     private Vector3[] baseHeight;

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
    */


    }
    // Use this for initialization


    GameObject MakePlane(bool invert,Vector3 pos,Vector3 facing,float lenght,float height,float divideByX,float divideByY,Vector2 rect,float rectH,float rectW)
    {
        //Quaternion q=Quaternion.Euler(facing.x,facing.y,facing.z);
        GameObject wall = Instantiate(new GameObject(), pos, Quaternion.identity) as GameObject;
        WallMesh wallMesh = wall.AddComponent<WallMesh>();
        wallMesh.invert = invert;
        wallMesh.rect = rect;
        wallMesh.rectH = rectH;
        wallMesh.rectW = rectW;
        wallMesh.WallMeshContructor(lenght, height, divideByX,divideByY);
        wall.transform.forward = facing;
        return wall;
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
