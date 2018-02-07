using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallMesh : MonoBehaviour
{
    //Mesh components
    MeshFilter mf;
    MeshRenderer mr;

    [Header("Mesh Variables")]
    public float yMagnitude, xMagnitude, yDividedBy, xDividedBy;
    public int[] triangles;
    public List<Vector3> verties;

    [Header("Cut Out Variables")]//cuting shapes out of the mesh
    public Vector2 rect;
    public float rectH, rectW;

    [Header("Debug Options")]
    public bool isDebugMode = false;

    public bool invert = true;

    public void WallMeshContructor(float _xMagnitude,float _yMagnitude,float _xDivideBy,float _yDivideBy)
    {
        yMagnitude = _yMagnitude;
        xMagnitude = _xMagnitude;

        xDividedBy = _xDivideBy;
        yDividedBy = _yDivideBy;

        CreateMesh();
       
    }

    public void floorMeshConstructor(float _xMagnitude, float _yMagnitude, float _xDivideBy, float _yDivideBy)
    {

        yMagnitude = _yMagnitude;
        xMagnitude = _xMagnitude;

        xDividedBy = _xDivideBy;
        yDividedBy = _yDivideBy;

        CreateMeshFloor();

    }


    // Update is called once per frame
    void Update()
    {
        if (isDebugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                CreateMesh();
        }
    }

    public void CreateMeshFloor()
    {
        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.AddComponent<MeshRenderer>();

        /*
        mf = this.gameObject.AddComponent<MeshFilter>();
        mr = this.gameObject.AddComponent<MeshRenderer>();
        */
        mr.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.grey);
        tex.Apply();
        mr.material.mainTexture = tex;
        mr.material.color = Color.grey;


        Mesh m = new Mesh();
        //m.name = name;

        List<Vector3> vertices = new List<Vector3>();
        for (float i = -xMagnitude / 2; i < xMagnitude / 2 + xDividedBy; i += xDividedBy)
        {
            for (float j = -yMagnitude / 2; j < yMagnitude / 2 + yDividedBy; j += yDividedBy)
            {
                vertices.Add(new Vector3(i, 0, j));
            }
        }

        int columnLenght = (int)(yMagnitude / yDividedBy);

        List<int> triangles = new List<int>();
        //remove triangles if they're inside cut out shapes


        for (int i = 0; i < vertices.Count - columnLenght - 2; i++)
        {

            if ((i + 1) % (columnLenght + 1) != 0)
            {

                triangles.Add(i);//current
                triangles.Add(i + 1);//
                triangles.Add(i + columnLenght + 1);//(1,1) away

                //triangle 2 right
                triangles.Add(i + 1);//current
                triangles.Add(i + columnLenght + 2);//
                triangles.Add(i + columnLenght + 1);
            }
        }

        verties = vertices;
        this.triangles = triangles.ToArray();
        m.vertices = vertices.ToArray();
        Vector2[] uvs = new Vector2[vertices.Count];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        m.uv = uvs;
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();

        //set mesh Filter
        mf.mesh = m;
    }


    public void CreateMesh()
    {
        
            mf = gameObject.AddComponent<MeshFilter>();
            mr = gameObject.AddComponent<MeshRenderer>();

            /*
            mf = this.gameObject.AddComponent<MeshFilter>();
            mr = this.gameObject.AddComponent<MeshRenderer>();
            */
            //mr.material.shader = Shader.Find("Particles/Additive");
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.grey);
            tex.Apply();
            mr.material.mainTexture = tex;
            mr.material.color = Color.grey;


            Mesh m = new Mesh();
        //m.name = name;

        List<Vector3> vertices = new List<Vector3>();
        for (float i = -xMagnitude/2; i < xMagnitude/2 + xDividedBy; i += xDividedBy)
        {
            for (float j = -yMagnitude/2; j < yMagnitude/2 + yDividedBy; j += yDividedBy)
            {
                vertices.Add(new Vector3(i, j, 0));
            }
        }

        int columnLenght = (int)(yMagnitude / yDividedBy);

        List<int> triangles = new List<int>();
        //remove triangles if they're inside cut out shapes

        Vector2 lRect;
        lRect.y = rect.y - (yMagnitude / 2);

        lRect.x = rect.x - (xMagnitude / 2);

        Debug.Log(xMagnitude-rect.x-rectW);
        if (invert)
            lRect.x = (xMagnitude - rect.x - rectW) - (xMagnitude / 2);



        for (int i = 0; i < vertices.Count - columnLenght-2; i++)
         {
            Vector3 v = vertices[i];

            if (!(v.x >  lRect.x - xDividedBy &&
                  v.x <  lRect.x + rectW &&
                  v.y >  lRect.y - yDividedBy &&
                  v.y <  lRect.y + rectH)
                  && (i + 1) % (columnLenght + 1) != 0)
            {

                triangles.Add(i);//current
                triangles.Add(i + 1);//
                triangles.Add(i + columnLenght + 1);//(1,1) away

                //triangle 2 right
                triangles.Add(i + 1);//current
                triangles.Add(i + columnLenght + 2);//
                triangles.Add(i + columnLenght + 1);


            }
        }

        verties = vertices;
        this.triangles = triangles.ToArray();
        m.vertices = vertices.ToArray();
        Vector2[] uvs = new Vector2[vertices.Count];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        m.uv = uvs;
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();

        //set mesh Filter
        mf.mesh = m;
    }

    public void SetWallTexture(Color c)
    {
        mr = gameObject.GetComponent<MeshRenderer>();
        //mr.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, c);
        tex.Apply();
        mr.material.mainTexture = tex;
        mr.material.color = c;

    }

}
