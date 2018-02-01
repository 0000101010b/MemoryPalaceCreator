using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


    public float speed;
    public float SideMoveDist;
    public float ForwardDist;

    //movement directions
    bool right = false;
    bool left = false;
    bool back = false;
    bool forward = false;
    bool up = false;
    bool down = false;

    //transition
    bool wait=false;
    public float waitTime = 1.0f;
    float t = 1;
    public Vector3 oldPos;
    public Vector3 newPos;

    public GameObject center;
    public GameObject rotate;
    float upIsUp = 1;

    Quaternion oldRot;
    Quaternion newRot;

    bool isRotate = false;
  

    // Update is called once per frame
    void Update() {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (!wait&&t>=1)
        {
            if (isRotate)
                isRotate = false;

            if (h > 0)
                left = true;
            else if (h < 0)
                right = true;

            if (v > 0 && Vector3.Distance(transform.position, center.transform.position) > 7f)
                forward = true;
            else if (v < 0 && Vector3.Distance(transform.position, center.transform.position) < 50f)
                back = true;

            if (Input.GetKeyDown(KeyCode.Space))
                up = true;
            else if (Input.GetKeyDown(KeyCode.LeftShift))
                down = true;



            if (right)
                SideMove(SideMoveDist);
            if (left)
                SideMove(-SideMoveDist);
            if (forward)
                Straight(ForwardDist);
            if (back)
                Straight(-ForwardDist);
            if (down)
                Up(-10);
            if (up)
            {
                Up(10);
                Debug.Log("up");
            }

           

          
      /*  }else if(!isRotate)
        {*/
            /*
            transform.position = Vector3.Lerp(oldPos, newPos, t);
            t += (float)(Time.deltaTime * 0.5*speed);
            transform.LookAt(center.transform.position);*/
       }else if(isRotate){
            Debug.Log("rot");
            rotate.transform.rotation = Quaternion.Slerp(oldRot,newRot, t);
            t += (float)(Time.deltaTime * 0.5 * speed);

        }else
        {
            transform.position = Vector3.Lerp(oldPos, newPos, t);
            t += (float)(Time.deltaTime * 0.5 * speed);
            transform.LookAt(center.transform.position);
        }

    }

    IEnumerator DuckExit()
    {
        yield return new WaitForSeconds(.5f);
        Duck(1f);
    }

    void Duck(float dist)
    {
        oldPos = transform.position;
        newPos = transform.position+(Vector3.up*dist);
    }
    void Straight(float dist)
    {
        StartCoroutine("Wait");
        oldPos = transform.position;
        newPos = transform.position + transform.forward * dist;
        t = 0;
        isRotate = false;
        back = forward = false;
    }

    void Up(float dist)
    {
        StartCoroutine("Wait");
        t = 0;
        oldRot = rotate.transform.rotation;
        newRot = rotate.transform.rotation*Quaternion.AngleAxis(dist,rotate.transform.right);
        isRotate = true;
        Debug.Log("hello");




        up = down = false;
      
    }

    void SideMove(float dist)
    {
        StartCoroutine("Wait");
        oldRot = rotate.transform.rotation;
        newRot = rotate.transform.rotation * Quaternion.AngleAxis(dist, center.transform.up);
        /*
        Vector3 dir = transform.position - center.transform.position;

        Quaternion q = Quaternion.AngleAxis(-dist, transform.parent.transform.up);
        newPos = q * dir.normalized;
        newPos = (newPos * dir.magnitude) + center.transform.position;
        oldPos = transform.position;*/
        isRotate = true;
        t = 0;
        left = right = false;
    }

    IEnumerator Wait()
    {
        wait = true;
        yield return new WaitForSeconds(waitTime);
        wait = false;
    }
}
