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

	}

    public void EnterObjectSelect()
    {
        editMPobj.enabled = false;
        fpsScript.enabled = !fpsScript.enabled;
        objectSelect.SetActive(!objectSelect.activeSelf);
        Cursor.visible = true;
    }
    public void ExitObjectSelect()
    {
        editMPobj.enabled = true;
        fpsScript.enabled = !fpsScript.enabled;
        objectSelect.SetActive(!objectSelect.activeSelf);
        Cursor.visible = false;
        editMPobj.editMode = EditMP_Obj.eEditMode.NotLooking;
    }




}
