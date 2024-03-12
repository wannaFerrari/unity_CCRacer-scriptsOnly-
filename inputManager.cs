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
    public bool isOnline;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        isOnline = GM.isOnline;
    }

    private void FixedUpdate () {

        keyboard();
    }

    private void Update()
    {
        EscKey();
        ResetKey();
        CameraKey();
        FinishKey();
    }

    private void keyboard () {
        vertical = Input.GetAxis ("Vertical");
        horizontal = Input.GetAxis ("Horizontal");
        handbrake = (Input.GetAxis ("Jump") != 0) ? true : false;
    }
    
    private void EscKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isOnline)
        {
           
            if (MenuStatus)
            {
                MenuStatus = false;
                //Menu.SetActive(false);
                GM.MenuCanvasClose();
            }
            else
            {
                MenuStatus = true;
                //Menu.SetActive(true);
                GM.MenuCanvasOpen();
            }
        }
    }
    
    private void ResetKey()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.GetComponent<controller>().resetCar();
            
        }
    }
    
    private void CameraKey()
    {
        
        if (Input.GetKey(KeyCode.Z))
        {
            gameObject.GetComponent<controller>().cameraChange(1);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            gameObject.GetComponent<controller>().cameraChange(2);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            gameObject.GetComponent<controller>().cameraChange(3);
        }
        else
        {
            gameObject.GetComponent<controller>().cameraChange(0);
        }
        //gameObject.GetComponent<controller>().mainCamera.enabled= true;

    }

    private void FinishKey()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            GM = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
            GM.testFinish();
        }
    }

}