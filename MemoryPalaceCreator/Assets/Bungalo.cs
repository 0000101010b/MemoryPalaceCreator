using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bungalo : MonoBehaviour {

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


    private void Start()
    {
        MakeBungalo();
    }

    // Use this for initialization
    void MakeBungalo () {
        lines = new List<Vector3>();
        posBuildings = new List<Vector2>();
        CreateGrid();
    }
    void OnDrawGizmos()
    {
        Draw(root);
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
