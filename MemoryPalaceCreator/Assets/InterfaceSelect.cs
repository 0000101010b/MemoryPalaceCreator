using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InterfaceSelect : MonoBehaviour {


   
    //PlayerMovement
    public FirstPersonController fpsScript;
    //ObjectSelect
    public GameObject objectSelect;
    //EditObject
    public EditMP_Obj editMPobj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.I)&&!objectSelect.activeSelf && editMPobj.editMode == EditMP_Obj.eEditMode.NotLooking)
            EnterObjectSelect();

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitObjectSelect();
	}

    public void EnterObjectSelect()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fpsScript.enabled = false;
        editMPobj.editMode = EditMP_Obj.eEditMode.OtherInterface;
        objectSelect.SetActive(!objectSelect.activeSelf);
    }

    public void ExitObjectSelect()
    {
        editMPobj.enabled = true;
        fpsScript.enabled = !fpsScript.enabled;

        objectSelect.SetActive(!objectSelect.activeSelf);


        editMPobj.editMode = EditMP_Obj.eEditMode.NotLooking;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        editMPobj.editWall=false;
    }





}
