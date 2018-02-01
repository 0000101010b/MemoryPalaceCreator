using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class BSPGrid : Grid {


    [Header("Grid Variables")]

    public int minLeafSize;
    public int maxLeafSize;
    public float magnitude;

    public List<Leaf> _leafs;
    public Leaf root;

    public List<Vector3> lines;
    List<Vector2> posBuildings;


    public void Init(int minLeafSize,int maxLeafSize,float magnitude)
    {
        this.maxLeafSize = maxLeafSize;
        this.minLeafSize = minLeafSize;
        this.magnitude = magnitude;
        lines = new List<Vector3>();
        posBuildings = new List<Vector2>();
    }

    void OnDrawGizmos()
    {
       Draw(root);
    }

    
    void Draw(Leaf l)
    {

        Gizmos.color = Color.black;
        if (l != null)
        {
            Gizmos.DrawLine(new Vector3(l.x,0, l.y), new Vector3(l.x + l.width,0, l.y));
            Gizmos.DrawLine(new Vector3(l.x,0, l.y), new Vector3(l.x,0, l.y + l.height));

            Gizmos.DrawLine(new Vector3(l.x + l.width,0, l.y + l.height), new Vector3(l.x + l.width,0, l.y));
            Gizmos.DrawLine(new Vector3(l.x + l.width,0, l.y + l.height), new Vector3(l.x,0, l.y + l.height));
        }
        Gizmos.color = Color.white;
        if (l.room != null)
        {
            Gizmos.DrawLine(new Vector3(l.room.x,0, l.room.y), new Vector3(l.room.x + l.room.width,0, l.room.y));
            Gizmos.DrawLine(new Vector3(l.room.x,0, l.room.y), new Vector3(l.room.x,0, l.room.y + l.room.height));

            Gizmos.DrawLine(new Vector3(l.room.x + l.room.width,0, l.room.y + l.room.height), new Vector3(l.room.x + l.room.width,0, l.room.y));
            Gizmos.DrawLine(new Vector3(l.room.x + l.room.width,0, l.room.y + l.room.height), new Vector3(l.room.x,0, l.room.y + l.room.height));
        }
        if (l.halls != null)
        {
            foreach (Rectangle h in l.halls)
            {
                Gizmos.DrawLine(new Vector3(h.x,0, h.y), new Vector3(h.x + h.width,0, h.y));
                Gizmos.DrawLine(new Vector3(h.x,0, h.y), new Vector3(h.x,0, h.y + h.height));

                Gizmos.DrawLine(new Vector3(h.x + h.width,0, h.y + h.height), new Vector3(h.x + h.width,0, h.y));
                Gizmos.DrawLine(new Vector3(h.x + h.width,0, h.y + h.height), new Vector3(h.x,0, h.y + h.height));
            }
        }

        if (l.leftChild != null || l.rightChild != null)
        {
            Draw(l.leftChild);
            Draw(l.rightChild);
        }

    }

    public void CreateBuildings(Leaf l)
    {
       // Gizmos.color = Color.black;
        if (l != null)
        {

            lines.Add(new Vector3(l.x, 0, l.y));
            lines.Add(new Vector3(l.x + l.width, 0, l.y));

            Rectangle rect = l.getRoom();
            if (rect != null)
            {

                bool createdBefore = false;
                for (int i = 0; i < posBuildings.Count; i++)
                {
                    if (Vector2.Distance(posBuildings[i],
                        new Vector2(rect.x, rect.y))< 0.5f)
                    {
                        createdBefore = true;
                        break;
                    }
                
                }

                if (!createdBefore)
                {
                    GameObject shedGameobject = new GameObject("Shed");
                    Shed shed=shedGameobject.AddComponent<Shed>();
                    shed.shedGameobject = shedGameobject;
                    posBuildings.Add(new Vector2(rect.x, rect.y));
                    int floors = UnityEngine.Random.Range(1, 1);
                    shed.ShedConstructor(new Vector3(rect.x, 0, rect.y), new Vector2(rect.height, rect.width), Vector3.forward, Vector3.right, 3.0f * floors);
                    shed.shedGameobject.transform.SetParent(transform);
                }
            }


            /*
            GameObject road = new GameObject("Road");
            WallMesh r = road.AddComponent<WallMesh>();
            Vector3 to = new Vector3(l.x, 0, l.y) - new Vector3(l.x + l.width, 0, l.y);
            Vector3 mid=(new Vector3(l.x, 0, l.y) + new Vector3(l.x + l.width, 0, l.y)) / 2;
            r.transform.position=mid;
            r.floorMeshConstructor(to.magnitude, 1, 1, 1);
 */           
            lines.Add(new Vector3(l.x + l.width, 0, l.y + l.height));
            lines.Add( new Vector3(l.x, 0, l.y + l.height));
 
        /*
            
            Gizmos.DrawLine(new Vector3(l.x, 0, l.y), new Vector3(l.x + l.width, 0, l.y));
            Gizmos.DrawLine(new Vector3(l.x, 0, l.y), new Vector3(l.x, 0, l.y + l.height));

            Gizmos.DrawLine(new Vector3(l.x + l.width, 0, l.y + l.height), new Vector3(l.x + l.width, 0, l.y));
            Gizmos.DrawLine(new Vector3(l.x + l.width, 0, l.y + l.height), new Vector3(l.x, 0, l.y + l.height));
            */
        }
        if (l.leftChild != null || l.rightChild != null)
        {
            CreateBuildings(l.leftChild);
            CreateBuildings(l.rightChild);
        }
      
    }

    public void  CreateRoads()
    {
        for (int i = 0; i < lines.Count; i += 2)
        {
            GameObject road = new GameObject("Road");
            GameObject start = new GameObject("Start");
            start.transform.position = lines[i];
            
            GameObject end = new GameObject("end");
            end.transform.position = lines[i + 1];
            WallMesh r = road.AddComponent<WallMesh>();
            Vector3 to = lines[i] - lines[i+1];
            Vector3 mid = (lines[i] + lines[i+1]) / 2;
            r.transform.position = mid;
            r.floorMeshConstructor(to.magnitude, UnityEngine.Random.Range(1,4), 1, 1);
        }
    }
    public void CreateBuildings()
    {
        
    }

    public void CombineRoads()
    {
        if (lines != null)
        { 
            
            for (int i = 0; i < lines.ToList().Count-1; i += 2)
            {
                float mag = Mathf.Abs((lines[i] - lines[i + 1]).magnitude);
                
                for (int j = 0; j < lines.ToList().Count-1; j += 2)
                {
                    Debug.Log("I: " + i + "count:" + lines.ToList().Count);
                    lines = lines.ToList();
                    if (Mathf.Abs(lines[i].z - lines[j + 1].z) <1f && i != j)
                    {

                        if (mag < Mathf.Abs((lines[i] - lines[j + 1]).magnitude))
                        {
                            Vector3 temp = lines[j + 1];

                            if (i < j)
                            {
                                lines.ToList().RemoveAt(j + 1);
                                lines.ToList().RemoveAt(j);
                                lines.ToList().RemoveAt(i + 1);
                                lines.ToList().Insert(i + 1, temp);

                            }
                            else
                            {
                                lines.ToList().RemoveAt(i + 1);
                                lines.ToList().Insert(i + 1, temp);
                                lines.ToList().RemoveAt(j + 1);
                                lines.ToList().RemoveAt(j);
                            }
                            mag = Mathf.Abs((lines[i] - lines[i + 1]).magnitude);
                        }
                        else if(i!=j)
                        {
                            lines.ToList().RemoveAt(j + 1);
                            lines.ToList().RemoveAt(j);
                        }

                    }
                    lines = lines.ToList();

                }
                lines = lines.ToList();

            }

        }
    }




    public override void Build(int i, int j, Vector2 buildspace)
    {
        throw new NotImplementedException();
    }

    public override void CreateGrid()
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


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
