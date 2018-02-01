using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Leaf
{
    public int minLeafSize;
    public int x, y, width, height;

    public Leaf leftChild;
    public Leaf rightChild;
    public Rectangle room;
    public List<Rectangle> halls;


    public Leaf(int _minLeafSize, int _x, int _y, int _width, int _height)
    {
        minLeafSize = _minLeafSize;
        x = _x;
        y = _y;
        width = _width;
        height = _height;
    }

    public bool Split()
    {
        if (leftChild != null || rightChild != null)
            return false;

        bool splitH = 1 > Random.Range(0, 2);

        if (width > height && width / height >= 1.25f)
        {
            splitH = false;
        }
        else if (height > width && height / width >= 1.25f)
        {
            splitH = true;
        }

        int max = (splitH ? height : width) - minLeafSize;

        if (max < minLeafSize)
            return false;

        int split = Random.Range(minLeafSize, max);

        if (splitH)
        {
            leftChild = new Leaf(minLeafSize, x, y, width, split);
            rightChild = new Leaf(minLeafSize, x, y + split, width, height - split);
        }
        else
        {
            leftChild = new Leaf(minLeafSize, x, y, split, height);
            rightChild = new Leaf(minLeafSize, x + split, y, width - split, height);
        }
        return true;
    }

    public Rectangle getRoom()
    {
        if(room!=null)
        {
            return room;
        }
        else
        {
            Rectangle lRoom=null;
            Rectangle rRoom=null;

            if(leftChild!=null)
            {
                lRoom = leftChild.getRoom();
            }
            if(rightChild!=null)
            {
                rRoom = rightChild.getRoom();
            }

            if (lRoom == null && rRoom == null)
                return null;
            else if (rRoom == null)
                return lRoom;
            else if (lRoom == null)
                return rRoom;
            else if (Random.Range(0.0f, 5.0f) > .5f)
                return lRoom;
            else
                return rRoom;
        }
    }

    public void CreateHall(Rectangle l,Rectangle r)
    {
        halls = new List<Rectangle>();

        Vector2 point1 =new Vector2(
            
            Random.Range(l.x + 1
            , l.x+l.width - 2),
             
            Random.Range(
                l.y  + 1,
                l.y + l.height - 2)
                
                );
        Vector2 point2 =new Vector2(
            Random.Range(
                r.x + 1, 
                r.width+r.x - 2)
                ,
            Random.Range(
                r.y +1,
                r.y + r.height -2)
                );
    

        int w = (int)(point2.x -point1.x);
        int h = (int)(point2.y -point1.y);

        if (w < 0)
        {
            if (h < 0)
            {
                if (Random.Range(.0f,1.0f) < 0.5)
                {
                    halls.Add(new Rectangle((int)point2.x, (int)point1.y,Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point2.x,(int)point2.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle((int)point2.x,(int)point2.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point1.x, (int)point2.y, 1, Mathf.Abs(h)));
                }
            }
            else if (h > 0)
            {
                if (Random.Range(0.0f,1.0f) < 0.5)
                {
                    halls.Add(new Rectangle((int)point2.x, (int)point1.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point2.x, (int)point1.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle((int)point2.x, (int)point2.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point1.x, (int)point1.y, 1, Mathf.Abs(h)));
                }
            }
            else // if (h == 0)
            {
                halls.Add(new Rectangle((int)point2.x, (int)point2.y, Mathf.Abs(w), 1));
            }
        }
        else if (w > 0)
        {
            if (h < 0)
            {
                if (Random.Range(0.0f,1.0f) < 0.5)
                {
                    halls.Add(new Rectangle((int)point1.x, (int)point2.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point1.x, (int)point2.y, 1,Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle((int)point1.x,(int)point1.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point2.x,(int)point2.y, 1, Mathf.Abs(h)));
                }
            }
            else if (h > 0)
            {
                if (Random.Range(0.0f,1.0f) < 0.5)
                {
                    halls.Add(new Rectangle((int)point1.x,(int)point1.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point2.x,(int)point1.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle((int)point1.x, (int)point2.y, Mathf.Abs(w), 1));
                    halls.Add(new Rectangle((int)point1.x, (int)point1.y, 1, Mathf.Abs(h)));
                }
            }
            else // if (h == 0)
            {
                halls.Add(new Rectangle((int)point1.x,(int)(int)point1.y, Mathf.Abs(w), 1));
            }
        }
        else // if (w == 0)
        {
            if (h < 0)
            {
                halls.Add(new Rectangle((int)point2.x, (int)point2.y, 1, Mathf.Abs(h)));
            }
            else if (h > 0)
            {
                halls.Add(new Rectangle((int)point1.x, (int)point1.y, 1, Mathf.Abs(h)));
            }
        }

    }

    public void createRooms()
    {
        // this function generates all the rooms and hallways for this Leaf and all of its children.
        if (leftChild != null || rightChild != null)
        {
            // this leaf has been split, so go into the children leafs
            if (leftChild != null)
            {
                leftChild.createRooms();
            }
            if (rightChild != null)
            {
                rightChild.createRooms();
            }

            // if there are both left and right children in this Leaf, create a hallway between them
            if (leftChild != null && rightChild != null)
            {
                CreateHall(leftChild.getRoom(), rightChild.getRoom());
            }
        }
        else
        {
            // this Leaf is the ready to make a room
            Vector2 roomSize;
            Vector2 roomPos;
            // the room can be between 3 x 3 tiles to the size of the leaf - 2.
            roomSize = new Vector2(Random.Range(3, width - 2), Random.Range(3, height - 2));
            // place the room within the Leaf, but don't put it right 
            // against the side of the Leaf (that would merge rooms together)
            roomPos = new Vector2(Random.Range(1, width - roomSize.x - 1), Random.Range(1, height - roomSize.y - 1));
            room = new Rectangle(x + (int)roomPos.x, y + (int)roomPos.y, (int)roomSize.x, (int)roomSize.y);
        }
    }
}
public class Rectangle
{
    public int x, y, width, height;

    public Rectangle(int _x,int _y,int _width,int _height)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
    }
}

public class BSP : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public int minLeafSize;
    public int maxLeafSize;

    public List<Leaf>_leafs;

    public Leaf root;

	// Use this for initialization
	void Start () {
   
        root=new Leaf(minLeafSize,0,0,mapWidth,mapHeight);

        _leafs = new List<Leaf>();
        _leafs.Add(root);

        bool didSplit = true;

        while (didSplit)
        {
            didSplit = false;
            foreach(Leaf l in _leafs.ToList())
            {
                if (l.leftChild == null && l.rightChild == null) // if this Leaf is not already split...
                {
                    // if this Leaf is too big, or 75% chance...
                    if (l.width > maxLeafSize || l.height > maxLeafSize || Random.Range(0.0f,1.0f) > 0.25)
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

 
    void OnDrawGizmos()
    {
        Draw(root);

    }
    void Draw(Leaf l)
    {
        
        Gizmos.color = Color.black;
        if (l != null)
        {
            Gizmos.DrawLine(new Vector2(l.x, l.y), new Vector2(l.x + l.width, l.y));
            Gizmos.DrawLine(new Vector2(l.x, l.y), new Vector2(l.x, l.y + l.height));

            Gizmos.DrawLine(new Vector2(l.x + l.width, l.y + l.height), new Vector2(l.x + l.width, l.y));
            Gizmos.DrawLine(new Vector2(l.x + l.width, l.y + l.height), new Vector2(l.x, l.y + l.height));
        }
        Gizmos.color = Color.white;
        if (l.room != null)
        {
           Gizmos.DrawLine(new Vector2(l.room.x, l.room.y), new Vector2(l.room.x + l.room.width, l.room.y));
           Gizmos.DrawLine(new Vector2(l.room.x, l.room.y), new Vector2(l.room.x, l.room.y + l.room.height));

           Gizmos.DrawLine(new Vector2(l.room.x + l.room.width, l.room.y + l.room.height), new Vector2(l.room.x + l.room.width, l.room.y));
           Gizmos.DrawLine(new Vector2(l.room.x + l.room.width, l.room.y + l.room.height), new Vector2(l.room.x, l.room.y + l.room.height));
        }
        if (l.halls != null)
        {
            foreach (Rectangle h in l.halls)
            {
                Gizmos.DrawLine(new Vector2(h.x, h.y), new Vector2(h.x + h.width, h.y));
                Gizmos.DrawLine(new Vector2(h.x, h.y), new Vector2(h.x, h.y + h.height));

                Gizmos.DrawLine(new Vector2(h.x + h.width, h.y + h.height), new Vector2(h.x + h.width, h.y));
                Gizmos.DrawLine(new Vector2(h.x + h.width, h.y + h.height), new Vector2(h.x, h.y + h.height));
            }
        }

        if (l.leftChild != null||l.rightChild!=null)
        {
            Draw(l.leftChild);
            Draw(l.rightChild);
        }
        
    }

	// Update is called once per frame
	void Update () {
	
	}

    void CreateBSP()
    {

    }
}
