using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bungalo : MonoBehaviour {


    //GameObjects
    public List<GameObject> bungaloObjs;

    //Debug Lines
    public List<Line> DL1;
    public Dictionary<Vector2, Vector3> doorLocations;

    [Header("Grid Variables")]
    public int gridWidth;
    public int gridBreath;
    public int minLeafSize;
    public int maxLeafSize;
    public float magnitude;
    public Vector2 offset;


    //contruction objects
    List<Leaf> _leafs;
    Leaf root;
    List<Rectangle> halls;
    List<Rectangle> bspRects;


    //Rooms
    public List<Room> rooms;
    
    private void Start()
    {
        MakeBungalo();
    }

    // Use this for initialization
    public void MakeBungalo () {

        doorLocations = new Dictionary<Vector2,Vector3>();
       
        CreateGrid();

        DL1 = new List<Line>();
        bspRects = new List<Rectangle>();
        halls = new List<Rectangle>();

        if (bungaloObjs != null)
        {
            foreach (GameObject g in bungaloObjs)
                Destroy(g);
        }

        bungaloObjs = new List<GameObject>();

        GetHallsAndLines(root);
        FindDoors();
        SetDoorPositions();

        FindRoomCenters();
        Create();

        foreach (GameObject g in bungaloObjs)
            g.transform.position = g.transform.position + new Vector3(offset.x,0,offset.y);
    }



    void Create()
    {
        foreach (Room r in rooms)
        {
            
            r.offset = offset;
            r.Create(root);
            foreach (Wall w in r.insideWalls)
            {
                bungaloObjs.Add(w.obj);
                if(w.outsideObj!=null)
                    bungaloObjs.Add(w.outsideObj);
                if (w.outsideDoor != null)
                    bungaloObjs.Add(w.outsideDoor);
            }

            bungaloObjs.Add(r.floor.obj);
            bungaloObjs.Add(r.ceiling.obj);
        }

        Rectangle box = new Rectangle(root.x, root.y, root.width, root.height);

        List<Rectangle> doorways = new List<Rectangle>();
        Wall roof = new Wall(eWall.Roof, box, doorways);
        roof.Create(box);
        bungaloObjs.Add(roof.obj);

        //create corners
        GameObject c1= new GameObject("C1");
        c1.transform.position = new Vector3(root.x-.25f, 0, root.y-0.25f);
        c1.AddComponent<Corner_Gen>().Build();
        bungaloObjs.Add(c1);

        GameObject c2 = new GameObject("C2");
        c2.transform.position = new Vector3(root.x + root.width+0.25f, 0, root.y-0.25f);
        c2.AddComponent<Corner_Gen>().Build();
        c2.transform.rotation *= Quaternion.AngleAxis(270.0f, transform.up);
        bungaloObjs.Add(c2);


        GameObject c3 = new GameObject("C3");
        c3.transform.position = new Vector3(root.x-0.25f, 0, root.y + root.height+0.25f);
        c3.AddComponent<Corner_Gen>().Build();
        c3.transform.rotation *= Quaternion.AngleAxis(90.0f, transform.up);
        bungaloObjs.Add(c3); 

        GameObject c4 = new GameObject("C4");
        c4.transform.position = new Vector3(root.x + root.width+0.25f, 0, root.y + root.height+0.25f);
        c4.AddComponent<Corner_Gen>().Build();
        c4.transform.rotation *= Quaternion.AngleAxis(180.0f, transform.up);
        bungaloObjs.Add(c4);





    }

    void CreateWall2(Vector3 pos)
    {
        GameObject g = new GameObject("corner1");
        WallMesh wallMesh = g.AddComponent<WallMesh>();
        //wallMesh.invert = invert;
        wallMesh.WallMeshContructor(.5f, 3.0f, 0.5f, 0.5f);
        g.transform.rotation *= Quaternion.identity;
        g.transform.position = pos;
    }


    void FindRoomCenters()
    {
        rooms = new List<Room>();

        for (int i = 0; i < bspRects.Count; i++)
        {
            int count = 0;

            for (int j = i; j < bspRects.Count; j++)
            {
                if (bspRects[i].PointInRect(bspRects[j].GetCenter()))
                    count++;
            }

          
            if (count == 1)
                rooms.Add(new Room(bspRects[i],doorLocations.Keys.ToList()));
        }
    }

    void SetDoorPositions()
    {
        foreach (KeyValuePair<Vector2,Vector3> p in doorLocations)
        {
            GameObject door = new GameObject("Door");
            door.transform.position = new Vector3(p.Key.x ,0, p.Key.y) +new Vector3(0,1f);
     
            
            Doorway_Gen script=door.AddComponent<Doorway_Gen>();
            script.Build();
            if (p.Value == Vector3.right)
                door.transform.rotation *= Quaternion.AngleAxis(90, transform.up);

            bungaloObjs.Add(door);
        }
    }

    void FindDoors()
    {
        foreach(Rectangle h in halls)
        {
            bool isWidth=false;

            if (h.width > 1)
                isWidth = true;

            Line l;
            if (isWidth)
                l = new Line(new Vector2(h.x, h.y), new Vector2(h.x + h.width, h.y));
            else
                l = new Line(new Vector2(h.x, h.y), new Vector2(h.x , h.y + h.height));
            DL1.Add(l);


            Vector2 dir = l.e - l.s;
            //if intersect with room wall
            foreach (Rectangle r in bspRects)
            {
                Line wall;
                if (isWidth)
                    wall =new Line(new Vector2(r.x, r.y), new Vector2(r.x, r.y + r.height));
                else
                    wall= new Line(new Vector2(r.x, r.y), new Vector2(r.x + r.width, r.y));

                Vector2 dir2 = wall.e - wall.s;

                if (Vector2.Dot(dir, dir2) == 0)
                {
                    if (isWidth)
                    {
                        if (wall.s.x > l.s.x && wall.e.x < l.e.x && wall.e.y > l.s.y && wall.s.y < l.s.y)
                        {
                            Vector2 key = new Vector2(wall.s.x, l.s.y) + new Vector2(0, 0.5f);
                            if (!doorLocations.ContainsKey(key))
                                doorLocations.Add( key,Vector3.right);
                        }

                    }
                    else if (wall.s.y > l.s.y && wall.e.y < l.e.y && wall.e.x > l.s.x && wall.s.x < l.s.x)
                    {
                        Vector2 key = new Vector2(l.s.x, wall.s.y) + new Vector2(0.5f, 0);

                        if(!doorLocations.ContainsKey(key))
                            doorLocations.Add( key, Vector3.forward);
                    }
                }
            }
        }
    }

    void GetHallsAndLines(Leaf l)
    {
        if (l!=null)
        {
            bspRects.Add(new Rectangle(l.x,l.y,l.width,l.height));

            if (l.halls != null)
            {
                foreach (Rectangle h in l.halls)
                    halls.Add(h);
            }

            if (l.leftChild != null || l.rightChild != null)
            {
                GetHallsAndLines(l.leftChild);
                GetHallsAndLines(l.rightChild);
            }
        }
    }
    public class Line
    {
        public Vector2 s;
        public Vector2 e;
     

        public Line(Vector2 _s,Vector2 _e)
        {
            s = _s;
            e = _e;
        }

    }

  

    void Draw2()
    {

        Vector3 o = new Vector3(offset.x, 0, offset.y);

        Gizmos.color = Color.black;
        if (bspRects != null)
        {
            foreach (Rectangle r in bspRects)
            {
                Gizmos.DrawLine(new Vector3(r.x, 0, r.y)+o, new Vector3(r.x + r.width, 0, r.y)+o);
                Gizmos.DrawLine(new Vector3(r.x, 0, r.y)+o, new Vector3(r.x, 0, r.y + r.height)+o);

                Gizmos.DrawLine(new Vector3(r.x + r.width, 0, r.y + r.height)+o, new Vector3(r.x + r.width, 0, r.y)+o);
                Gizmos.DrawLine(new Vector3(r.x + r.width, 0, r.y + r.height)+o, new Vector3(r.x, 0, r.y + r.height)+o);
            }
        }

        Gizmos.color = Color.white;
        if (halls != null)
        {
            foreach (Rectangle h in halls)
            {
                Gizmos.DrawLine(new Vector3(h.x, 0, h.y)+o, new Vector3(h.x + h.width, 0, h.y)+o);
                Gizmos.DrawLine(new Vector3(h.x, 0, h.y)+o, new Vector3(h.x, 0, h.y + h.height)+o);

                Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height)+o, new Vector3(h.x + h.width, 0, h.y)+o);
                Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height)+o, new Vector3(h.x, 0, h.y + h.height)+o);
            }
        }
    }

    void OnDrawGizmos()
    {
        Draw2();

        Gizmos.color = Color.red;
        Vector3 o = new Vector3(offset.x,0, offset.y);

        foreach (Line l in DL1)
            Gizmos.DrawLine(new Vector3(l.s.x,0,l.s.y)+o , new Vector3(l.e.x, 0, l.e.y)+o);


        foreach (KeyValuePair<Vector2,Vector3> v in doorLocations)
            Gizmos.DrawWireSphere(new Vector3(v.Key.x,0,v.Key.y)+o, 0.1f);


        Gizmos.color = Color.green;
        foreach(Room r in rooms)
            Gizmos.DrawWireSphere(new Vector3(r.Center.x, 0, r.Center.y)+o, 0.1f);

        //Draw(root);
    }



    public void CreateGrid()
    {
        root = new Leaf(minLeafSize, 0, 0, gridWidth, gridBreath);

        _leafs = new List<Leaf>();
        _leafs.Add(root);

        bool didSplit = true;

        while (didSplit)
        {
            didSplit = false;
            foreach (Leaf l in _leafs.ToList())
            {
                if (l.leftChild == null && l.rightChild == null) // if this Leaf is not already split...
                {
                    // if this Leaf is too big, or 75% chance...
                    if (l.width > maxLeafSize || l.height > maxLeafSize || UnityEngine.Random.Range(0.0f, 1.0f) > 0.25)
                    {
                        if (l.Split()) // split the Leaf!
                        {
                            // if we did split, Add the child leafs to the Vector so we can loop into them next
                            _leafs.Add(l.leftChild);
                            _leafs.Add(l.rightChild);
                            didSplit = true;
                        }
                    }
                }
            }

        }

        root.createRooms();
    }
    /*
    void Draw(Leaf l)
    {

        Gizmos.color = Color.black;
        if (l != null)
        {
            Gizmos.DrawLine(new Vector3(l.x, 0, l.y), new Vector3(l.x + l.width, 0, l.y));
            Gizmos.DrawLine(new Vector3(l.x, 0, l.y), new Vector3(l.x, 0, l.y + l.height));

            Gizmos.DrawLine(new Vector3(l.x + l.width, 0, l.y + l.height), new Vector3(l.x + l.width, 0, l.y));
            Gizmos.DrawLine(new Vector3(l.x + l.width, 0, l.y + l.height), new Vector3(l.x, 0, l.y + l.height));
        }
        Gizmos.color = Color.white;
        if (l.room != null)
        {
            Gizmos.DrawLine(new Vector3(l.room.x, 0, l.room.y), new Vector3(l.room.x + l.room.width, 0, l.room.y));
            Gizmos.DrawLine(new Vector3(l.room.x, 0, l.room.y), new Vector3(l.room.x, 0, l.room.y + l.room.height));

            Gizmos.DrawLine(new Vector3(l.room.x + l.room.width, 0, l.room.y + l.room.height), new Vector3(l.room.x + l.room.width, 0, l.room.y));
            Gizmos.DrawLine(new Vector3(l.room.x + l.room.width, 0, l.room.y + l.room.height), new Vector3(l.room.x, 0, l.room.y + l.room.height));
        }
        if (l.halls != null)
        {
            foreach (Rectangle h in l.halls)
            {
                Gizmos.DrawLine(new Vector3(h.x, 0, h.y), new Vector3(h.x + h.width, 0, h.y));
                Gizmos.DrawLine(new Vector3(h.x, 0, h.y), new Vector3(h.x, 0, h.y + h.height));

                Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height), new Vector3(h.x + h.width, 0, h.y));
                Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height), new Vector3(h.x, 0, h.y + h.height));
            }
        }

        if (l.leftChild != null || l.rightChild != null)
        {
            Draw(l.leftChild);
            Draw(l.rightChild);
        }

    }*/
    // Update is called once per frame

    void Update () {
        if (Input.GetKey(KeyCode.Space)) MakeBungalo();	
	}

}

public enum eWall
{
    East,
    West,
    North,
    South,
    Floor,
    Ceiling,
    Roof
}
/*
public class Doorway
{
    public Rectangle rect;
}*/

public class Wall
{
    public GameObject outsideDoor;

    public GameObject obj;
    public GameObject outsideObj;

    public eWall directionFacing;
    public Rectangle rect;
    public List<Rectangle> doorways;


    public Wall(eWall _dirFacing,Rectangle _rect,List<Rectangle>  _doorways)
    {
        this.directionFacing = _dirFacing;
        this.rect = _rect;
        this.doorways = _doorways;
    }

    public void Create(Rectangle b)
    {
        switch(directionFacing)
        {
            case eWall.Floor:
                Floor(b);
                break;
            case eWall.West:
                West(b,3);
                break;
            case eWall.East:
                East(b,3);
                break;
            case eWall.North:
                North(b,3);
                break;
            case eWall.South:
                South(b,3);
                break;
            case eWall.Ceiling:
                Ceiling(b);
                break;
            case eWall.Roof:
                Roof(b);
                break;
            default:
                break;
        }
    }

    public void Roof(Rectangle b)
    {
        string name;
        Vector3 pos;
        Quaternion rot;
        int width;
        int height;

        name = "Roof";
        pos = rect.GetCenter3d() + Vector3.up * 3.01f ;

        rot = Quaternion.AngleAxis(90.0f, Vector3.right);
        width = rect.width+1;
        height = rect.height + 1;


        CreateWall(b, name, pos, rot, width, height, true, ref obj);
    }

    public void East(Rectangle b, int height)
    {
        string name;
        Vector3 pos;
        Quaternion rot;
        int width;

        if (b.PointOnWall(new Vector2(this.rect.East().x, this.rect.East().z)) == eWall.East)
        {
            doorways = new List<Rectangle>();
            doorways.Add(new Rectangle((int)2, 0, 1, 2));

            name = "East Inside Wall";
            pos = rect.East() + Vector3.up * 1.5f + Vector3.right * 0.5f;
         
            rot = Quaternion.AngleAxis(270.0f, Vector3.up);
            width = rect.height;

            CreateWall(b,name, pos, rot, width, height, false, ref outsideObj);


            outsideDoor = new GameObject("Exit");
            outsideDoor.transform.position = rect.East() + Vector3.up * 1f - Vector3.forward * (float)width /2.0f + Vector3.forward *  ((float)doorways[0].x + 0.5f);
            Doorway_Gen e= outsideDoor.AddComponent<Doorway_Gen>();
            e.Build();
            outsideDoor.transform.rotation *= Quaternion.AngleAxis(270.0f, Vector3.up); 



            Rectangle dw = doorways[0];
            dw.x += 1;
            doorways.Clear();
            doorways.Add(dw);
            

        }

        name   = "East Inside Wall";
        pos   =  rect.East() + Vector3.up * 1.5f + Vector3.right * -0.5f;
      
        rot = Quaternion.AngleAxis(90.0f, Vector3.up);
        width=rect.height;
      

        CreateWall(b,name, pos,rot,width,height,true, ref obj);

      
    }

    public void CreateWall(Rectangle b,string name,Vector3 pos,Quaternion rot, int width,int height, bool invert,ref GameObject g)
    {
        g = new GameObject(name);
        WallMesh wallMesh = g.AddComponent<WallMesh>();
        wallMesh.invert = invert;

        if (doorways != null)
        {
            for (int i = 0; i < doorways.Count; i++)
            {
                wallMesh.rect = new Vector2(doorways[i].x, doorways[i].y);
                wallMesh.rectH = doorways[i].height;
                wallMesh.rectW = doorways[i].width;
            }
        }

        wallMesh.WallMeshContructor(width, height, 1, 1);
        g.transform.rotation *= rot;
        //pos += new Vector3(b.x, 0, b.y);
        g.transform.position  = pos;
    }

    public void West(Rectangle b, int height)
    {
        string name = "West Inside Wall";
        Vector3 pos = rect.West() + Vector3.up * 1.5f + Vector3.right * 0.5f;
        Quaternion rot = Quaternion.AngleAxis(270.0f, Vector3.up);
        int width = rect.height;
       // int height = 3;
        bool invert = false;

        CreateWall(b,name, pos, rot, width, height, invert, ref obj);

        if (b.PointOnWall(new Vector2(this.rect.West().x, this.rect.West().z)) == eWall.West)
        {
            name = "West outside Wall";
            pos = rect.West() + Vector3.up * 1.5f + Vector3.right * -0.5f;
            rot = Quaternion.AngleAxis(90.0f, Vector3.up);
            width = rect.height;
            //height = 3;
            invert = true;

            CreateWall(b,name, pos, rot, width, height, invert, ref outsideObj);
        }
    }

    public void North(Rectangle b, int height)
    {
        string name = "North Inside Wall";
        Vector3 pos = rect.North() + Vector3.up * 1.5f + Vector3.forward * -0.5f;
        Quaternion rot = Quaternion.identity;
        int width = rect.width;
        //int height = 3;
        bool invert = false;

        CreateWall(b,name, pos, rot, width, height, invert, ref obj);

        if (b.PointOnWall(new Vector2(this.rect.North().x, this.rect.North().z)) == eWall.North)
        {
            name = "North outside Wall";
            pos = rect.North() + Vector3.up * 1.5f + Vector3.forward * 0.5f;
            rot = Quaternion.AngleAxis(180.0f, Vector3.up);
            width = rect.width;
            //height = 3;
            invert = true;

            CreateWall(b,name, pos, rot, width, height, invert, ref outsideObj);
        }
    }
    public void South(Rectangle b,int height)
    {
        string name = "South Inside Wall";
        Vector3 pos = rect.South() + Vector3.up * 1.5f + Vector3.forward * 0.5f;
        Quaternion rot = Quaternion.AngleAxis(180.0f, Vector3.up);
        int width = rect.width;
        
        bool invert = true;

        CreateWall(b,name, pos, rot, width, height, invert, ref obj);

        if (b.PointOnWall(new Vector2(this.rect.South().x, this.rect.South().z)) == eWall.South)
        {
            name = "North outside Wall";
            pos = rect.South() + Vector3.up * 1.5f + Vector3.forward * -0.5f;
            rot = Quaternion.identity;
            width = rect.width;
            //height = 3;
            invert = false;

            CreateWall(b,name, pos, rot, width, height, invert, ref outsideObj);
        }


    }

    public void Floor(Rectangle b)
    {
        obj = new GameObject("Floor");
        WallMesh wallMesh = obj.AddComponent<WallMesh>();
        wallMesh.invert = false;
        wallMesh.WallMeshContructor(rect.width-1, rect.height-1, 1, 1);
        obj.transform.rotation *= Quaternion.AngleAxis(90, Vector3.right);
        obj.transform.position =  rect.GetCenter3d() +new Vector3(0,0.01f);
    }
    public void Ceiling(Rectangle b)
    {
        obj = new GameObject("Ceiling");
        WallMesh wallMesh = obj.AddComponent<WallMesh>();
        wallMesh.invert = true;
        wallMesh.WallMeshContructor(rect.width-1 , rect.height-1 , 1, 1);
        obj.transform.rotation *= Quaternion.AngleAxis(270, Vector3.right);
        obj.transform.position = rect.Ceiling();
    }
}

public class Room
{

    public Wall ceiling;
    public Wall floor;

    public List<Wall> insideWalls;

    public Vector2 offset;
    private Vector2 center;
    private Rectangle rect;

    public Vector2 Center
    {
        get
        {
            return center;
        }
        set
        {
            center = value;
        }
    }

    public Rectangle Rect
    {
        get
        {
            return rect;
        }
        set
        {
            rect = value;
        }
    }


    public Room(Rectangle r, List<Vector2> doorways)
    {
       
        this.center = r.GetCenter();
        this.rect = r;  

        List<Rectangle> eastDoorways = new List<Rectangle>();
        List<Rectangle> westDoorways = new List<Rectangle>();
        List<Rectangle> northDoorways = new List<Rectangle>();
        List<Rectangle> southDoorways = new List<Rectangle>();
        List<Rectangle> holeInFloor = new List<Rectangle>();
        List<Rectangle> holeInCeiling = new List<Rectangle>();

        foreach (Vector2 v in doorways)
        {

            switch (rect.PointOnWall(v))
            {

                case eWall.East:
                    eastDoorways.Add(new Rectangle((int)(v.y + 0.5f) - rect.y, 0, 1, 2));
                    break;
                case eWall.West:
                    westDoorways.Add(new Rectangle((int)(v.y - 0.5f) - rect.y, 0, 1, 2));
                    break;
                case eWall.North:
                    northDoorways.Add(new Rectangle((int)(v.x - 0.5f) - rect.x, 0, 1, 2));
                    break;
                case eWall.South:
                    southDoorways.Add(new Rectangle((int)(v.x + 0.5f) - rect.x, 0, 1, 2));
                    break;
                default:
                    break;

            }
        }


        floor   = new Wall(eWall.Floor, r, holeInFloor);
        ceiling = new Wall(eWall.Ceiling, r, holeInCeiling);


        insideWalls = new List<Wall>();
        insideWalls.Add(new Wall(eWall.East, r, eastDoorways));
        insideWalls.Add(new Wall(eWall.West, r, westDoorways));
        insideWalls.Add(new Wall(eWall.North, r, northDoorways));
        insideWalls.Add(new Wall(eWall.South, r, southDoorways));

    }

    public void Create(Leaf l)
    {
        Rectangle b = new Rectangle(l.x, l.y, l.width, l.height);

        floor.Create(b);
        floor.obj.AddComponent<MeshCollider>();
        //floor.obj.transform.position = floor.obj.transform.position + new Vector3(offset.x,0,offset.y);

        ceiling.Create(b);
        //ceiling.obj.transform.position = ceiling.obj.transform.position + new Vector3(offset.x,0, offset.y);
        ceiling.obj.AddComponent<MeshCollider>();

        foreach (Wall wall in insideWalls)
        {
            wall.Create(b);
            wall.obj.AddComponent<MeshCollider>();
            //wall.obj.transform.position = wall.obj.transform.position + new Vector3(offset.x,0, offset.y);
            if (wall.outsideObj != null)
            {
                wall.outsideObj.AddComponent<MeshCollider>();
            //    wall.outsideObj.transform.position = wall.outsideObj.transform.position + new Vector3(offset.x,0, offset.y);
            }
        }

       
    }
}
