using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bungalo : MonoBehaviour {


    public GameObject doorPrefab;

    public List<Vector2> points;

    public List<Line> l2;
    public Vector3 pos;
    public bool[,] isbuildArea;//is build area or not
    public int[,] gridValues; //area category
    //public int gridWidthBreath;
    public int gridWidth;
    public int gridBreath;

    [Header("Grid Variables")]
    public int minLeafSize;
    public int maxLeafSize;
    public float magnitude;

    public List<Leaf> _leafs;
    public Leaf root;

    public List<Vector3> lines;
    List<Vector2> posBuildings;


    public List<Rectangle> halls;
    public List<Rectangle> rooms;


    private void Start()
    {
        MakeBungalo();

        rooms = new List<Rectangle>();
        halls = new List<Rectangle>();

        GetHallsAndLines(root);

        l2 = new List<Line>();
        FindDoors();

    }

    public List<GameObject> doors;
    // Use this for initialization
    void MakeBungalo () {
        points = new List<Vector2>();
        lines = new List<Vector3>();
        posBuildings = new List<Vector2>();
        CreateGrid();
   
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
            l2.Add(l);


            Vector2 dir = l.e - l.s;
            //if intersect with room wall
            foreach (Rectangle r in rooms)
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
                        if (wall.s.x > l.s.x && wall.e.x < l.e.x &&  wall.e.y > l.s.y && wall.s.y < l.s.y)
                        {
                            points.Add(new Vector2(wall.s.x, l.s.y));
                        }

                    }
                    else if (wall.s.y > l.s.y && wall.e.y < l.e.y && wall.e.x > l.s.x && wall.s.x < l.s.x) 
                        points.Add(new Vector2(l.s.x, wall.s.y));
                }
            }
        }
    }

    void GetHallsAndLines(Leaf l)
    {
        if (l!=null)
        {
            rooms.Add(new Rectangle(l.x,l.y,l.width,l.height));

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
        foreach (Rectangle r in rooms)
        {
            Gizmos.DrawLine(new Vector3(r.x, 0, r.y), new Vector3(r.x + r.width, 0, r.y));
            Gizmos.DrawLine(new Vector3(r.x, 0, r.y), new Vector3(r.x, 0, r.y + r.height));

            Gizmos.DrawLine(new Vector3(r.x + r.width, 0, r.y + r.height), new Vector3(r.x + r.width, 0, r.y));
            Gizmos.DrawLine(new Vector3(r.x + r.width, 0, r.y + r.height), new Vector3(r.x, 0, r.y + r.height));
        }

        Gizmos.color = Color.white;
        foreach (Rectangle h in halls)
        {
            Gizmos.DrawLine(new Vector3(h.x, 0, h.y), new Vector3(h.x + h.width, 0, h.y));
            Gizmos.DrawLine(new Vector3(h.x, 0, h.y), new Vector3(h.x, 0, h.y + h.height));

            Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height), new Vector3(h.x + h.width, 0, h.y));
            Gizmos.DrawLine(new Vector3(h.x + h.width, 0, h.y + h.height), new Vector3(h.x, 0, h.y + h.height));
        }
    }



    
    void CreateDoor(Leaf l)
    {
        if (l != null)
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
        }


        
        if (l.leftChild != null || l.rightChild != null)
        {
            CreateDoor(l.leftChild);
            CreateDoor(l.rightChild);
        }

    }




    void OnDrawGizmos()
    {
        Draw2();

        Gizmos.color = Color.red;

        foreach (Line l in l2)
            Gizmos.DrawLine(new Vector3(l.s.x,0,l.s.y), new Vector3(l.e.x, 0, l.e.y));


        foreach (Vector2 v in points)
            Gizmos.DrawWireSphere(new Vector3(v.x,0,v.y), 0.5f);

        //CreateDoor(root);
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
