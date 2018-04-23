using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class EditMP_Obj : MonoBehaviour {

    public GameObject ObjDescription;
    public FirstPersonController fpsScript;
    public eEditMode editMode;

    public InputField i1;
    public InputField i2;

    public MPobj mpobj;

    public bool editWall;
    public GameObject currentWall;
    public GameObject textureSelect;

    public enum eEditMode
    {
        Looking,
        EditText,
        NotLooking,
        OtherInterface

    }

	// Use this for initialization
	void Start () {
        editMode = eEditMode.NotLooking;
	}


    void Update()
    {
       
      
    }

    // Update is called once per frame
    void FixedUpdate () {

        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit;

        switch (editMode)
        {
            case eEditMode.NotLooking:
            {
                    i1.enabled = false;
                    i2.enabled = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "MPobj")
                    {

                        mpobj=hit.collider.gameObject.GetComponent<MPobj>();
                        i1.text = mpobj.mpObj.name;
                        i2.text = mpobj.mpObj.description;

                        ObjDescription.SetActive(true);
                       
                        editMode = eEditMode.Looking;
                    }

                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "Inside Wall")
                    {

                        if (Input.GetMouseButton(1))
                        {
                            editWall = true;
                            currentWall=hit.collider.gameObject;
             
                            textureSelect.SetActive(true);

                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            fpsScript.enabled = false;
                            editMode = eEditMode.OtherInterface;

                        }
                    }


                    break;
             }
            case eEditMode.Looking:
                {


                    i1.enabled = false; 
                    i2.enabled = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    if (Input.GetMouseButton(0))
                    {
                        editMode = eEditMode.EditText;
                        fpsScript.enabled = false;
                    }

                    if (!Physics.Raycast(ray, out hit) || hit.collider.gameObject.tag != "MPobj")
                    {
                        ObjDescription.SetActive(false);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        editMode = eEditMode.NotLooking;
                    }
                    break;
                }
                
            case eEditMode.EditText:
                {
                    i1.enabled = true;
                    i2.enabled = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    
                    if (Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.Return))
                    {
                        editMode = eEditMode.NotLooking;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;

                        fpsScript.enabled = true;


                        mpobj.mpObj.name=i1.text;
                        mpobj.mpObj.description = i2.text;

                        //save data
                    }
                    break;
                }
            case eEditMode.OtherInterface:
            {

                    break;
            }

        }

    }
}
