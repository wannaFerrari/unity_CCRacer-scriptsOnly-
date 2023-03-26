using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour {

    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool handbrake;
    public GameObject Menu;
    public GameManager GM;
    public bool KeyboardStatus = true;
    public bool MenuStatus = false;

    private void FixedUpdate () {

        keyboard();
    }

    private void Update()
    {
        EscKey();
        ResetKey();
        CameraKey();
    }

    private void keyboard () {
        vertical = Input.GetAxis ("Vertical");
        horizontal = Input.GetAxis ("Horizontal");
        handbrake = (Input.GetAxis ("Jump") != 0) ? true : false;
    }

    private void EscKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            if (MenuStatus)
            {
                MenuStatus = false;
                Menu.SetActive(false);
            }
            else
            {
                MenuStatus = true;
                Menu.SetActive(true);
            }
        }
    }
    private void ResetKey()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GM.resetCar();
            
        }
    }

    private void CameraKey()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            GM.cameraChange(1);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            GM.cameraChange(2);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            GM.cameraChange(3);
        }
        else
        {
            GM.cameraChange(0);
        }
        
    }

}