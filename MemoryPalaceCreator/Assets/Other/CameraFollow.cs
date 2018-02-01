using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject center;
    public GameObject player;
    public float lerpSpeed;
    public float z;
    public float x;
    public float y;
    public float dist;
    public float switchUpDown=-1;

    bool lerp=true;
    Vector3 to;
    Vector3 from;
    float t;

    bool up = false;
    

	// Use this for initialization
	void Start () {
        Vector3 toPlayer=player.transform.position-center.transform.position;
        transform.position = z * toPlayer.normalized + toPlayer + center.transform.position;
       transform.position += y * Vector3.up;
        transform.LookAt(center.transform);

	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(player.transform.position, transform.position) > dist && !lerp)
        {
            /*if (Vector3.Dot(player.transform.up, center.transform.up) > 0 && !up)
            {
                player.transform.Rotate(transform.forward, 180);
                up = true;
            }
            else if (Vector3.Dot(player.transform.up, center.transform.up) < 0 && up)
            {
                player.transform.Rotate(transform.forward, 180);
                up = false;
            }*/
            
            Vector3 toPlayer = player.transform.position - center.transform.position;
            to = z* toPlayer.normalized + toPlayer + center.transform.position;
           to += y*Vector3.up;
            from = transform.position;

            t = 0;
            lerp = true;
            
            
        }
        

        if (lerp)
        {
            transform.position=Vector3.Slerp(from, to, t);
            t += Time.deltaTime * lerpSpeed;
            transform.LookAt(center.transform);
            if (t >= 1)
                lerp = false;
        }


	}

   
}
