using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InterfaceSelect : MonoBehaviour {


   
    //PlayerMovement
    public FirstPersonController fpsScript;
    //ObjectSelect
    public GameObject objectSelect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ObjectSelect();

	}

    public void ObjectSelect()
    {
        fpsScript.enabled = !fpsScript.enabled;
        //Cursor.visible = !Cursor.visible;
        objectSelect.SetActive(!objectSelect.activeSelf);
        Cursor.visible = true;
    }
}
