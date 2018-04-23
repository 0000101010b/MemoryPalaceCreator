using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPobj : MonoBehaviour {

    public MP_Obj mpObj;

	// Use this for initialization
	void Start () {
        mpObj = new MP_Obj();
        mpObj.name = "";
        mpObj.description = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class MP_Obj
{
    public string name;
    public string description;

}
