using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bungalo : MonoBehaviour {


    //GameObjects
    private List<GameObject> bungaloObjs;

    //Debug Lines
    public List<Line> DL1;
    public Dictionary<Vector2, Vector3> doorLocations;

    [Header("Grid Variables")]
    public int gridWidth;
    public int gridBreath;
    public int minLeafSize;
    public int maxLeafSize;
    public float magnitude;


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
    void MakeBungalo () {

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

    }



    void Create()
    {
        foreach (Room r in rooms)
            r.Create();
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
        Gizmos.color = Color.black;
        if (bspRects != null)
        {
            foreach (Rectangle r in bspRects)
            {
                Gizmos.DrawLine(new Vector3(r.x, 0, r.y), new Vector3(r.x + r.width, 0, r.y));
                Gizmos.DrawLine(new Vector3(r.x, 0, r.y), new Vector3(r.x, 0, r.y + r.height));

                Gizmos.DrawLine(new Vector3(r.x + r.width, 0, r.y + r.height), new Vector3(r.x + r.width, 0, r.y));
                Gizmos.DrawLine(new Vector3(r.x + r.width, 0, r.y + r.height), new Vector3(r.x, 0, r.y + r.height));
            }
        }

        Gizmos.color = Color.white;
        if (halls != null)
        {
            foreach (Rectangle h in halls)
            {
                Gizmos.DrawLine(new Vector3(h.x, 0, h.y), new Vector3(h.x + h.width, 0, h.y));
                Gizmos.DrawLine(new Vector3(h.x, 0, h.y), new Vector3(h.x, 0, h.y + h.height));

                Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height), new Vector3(h.x + h.width, 0, h.y));
                Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height), new Vector3(h.x, 0, h.y + h.height));
            }
        }
    }

    void OnDrawGizmos()
    {
        Draw2();

        Gizmos.color = Color.red;

        foreach (Line l in DL1)
            Gizmos.DrawLine(new Vector3(l.s.x,0,l.s.y), new Vector3(l.e.x, 0, l.e.y));


        foreach (KeyValuePair<Vector2,Vector3> v in doorLocations)
            Gizmos.DrawWireSphere(new Vector3(v.Key.x,0,v.Key.y), 0.1f);


        Gizmos.color = Color.green;
        foreach(Room r in rooms)
            Gizmos.DrawWireSphere(new Vector3(r.Center.x, 0, r.Center.y), 0.1f);

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

    }
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
    public eWall directionFacing;
    public Rectangle rect;
    public List<Rectangle> doorways;


    public Wall(eWall _dirFacing,Rectangle _rect,List<Rectangle>  _doorways)
    {
        this.directionFacing = _dirFacing;
        this.rect = _rect;
        this.doorways = _doorways;
    }

    public void Create()
    {
        switch(directionFacing)
        {
            case eWall.Floor:
                Floor();
                break;
            case eWall.East:
                East();
                break;
            default:
                break;

               
        }
    }
    public void East()
    {
        GameObject wall = new GameObject("East Inside Wall");
        WallMesh wallMesh = wall.AddComponent<WallMesh>();
        wallMesh.invert = true;

        if (doorways != null)
        {
            for (int i = 0; i < doorways.Count; i++)
            {


                wallMesh.rect = new Vector2(doorways[i].x,doorways[i].y);
                wallMesh.rectH = doorways[i].height;
                wallMesh.rectW = doorways[i].width;
            }
        }
        wallMesh.WallMeshContructor(rect.height , 3, 1, 1);
        wall.transform.rotation *= Quaternion.AngleAxis(90.0f, Vector3.up);



        wall.transform.position = wall.transform.position + rect.East() + Vector3.up *1.5f;
    }

    public void Floor()
    {
        GameObject wall = new GameObject("Floor");
        WallMesh wallMesh = wall.AddComponent<WallMesh>();
        wallMesh.invert = false;
        wallMesh.WallMeshContructor(rect.width-1, rect.height-1, 1, 1);
        wall.transform.rotation *= Quaternion.AngleAxis(90, Vector3.right);
        wall.transform.position = wall.transform.position + rect.GetCenter3d();
    }
}

public class Room
{
    public List<Wall> walls;

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

        walls = new List<Wall>();
        //walls.Add(new Wall(eWall.Floor, r, r));

        List<Rectangle> eastDoorways = new List<Rectangle>();
        List<Rectangle> northDoorways = new List<Rectangle>();

        foreach (Vector2 v in doorways)
        {

            switch (rect.PointOnWall(v))
            {

                case eWall.East:
                    eastDoorways.Add(new Rectangle((int)(v.y + 0.5f) - rect.y, 0, 1, 2));
                    break;
                case eWall.North:
                    northDoorways.Add(new Rectangle((int)(v.y + 0.5f) - rect.y, 0, 1, 2));
                    break;
                default:
                    break;

            {

            }
            /*if (rect.PointOnWall(v) == eWall.East)
            {
                eastDoorways.Add(new Rectangle((int)(v.y + 0.5f) - rect.y, 0, 1, 2));
            }*/

            }

        }


        walls.Add(new Wall(eWall.East, r, eastDoorways));

      
    }

    public void Create()
    {
        foreach (Wall wall in walls)
            wall.Create();

    }
}
