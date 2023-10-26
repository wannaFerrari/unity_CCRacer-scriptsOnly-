using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class cameraController : MonoBehaviourPun 
{

    private GameObject Player;
    private controller RR;
    private GameObject cameralookAt,cameraPos;
    private float speed = 0;
    //private float defaltFOV = 0;
    [Range (0, 50)] public float smothTime = 8;

    private void Start () 
    {
        if (photonView.IsMine)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            //Debug.Log(Player.GetPhotonView().Owner);
            RR = Player.GetComponent<controller>();
            cameralookAt = Player.transform.Find("camera lookAt").gameObject;
            cameraPos = Player.transform.Find("camera constraint").gameObject;

        }
        else
        {
            /*
            Player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(Player.GetPhotonView().Owner);
            RR = Player.GetComponent<controller>();
            Player.transform.Find("camera lookAt").gameObject.SetActive(false) ;
            Player.transform.Find("camera constraint").gameObject.SetActive(false);
            return;*/
        }
        //RR = transform.parent.GetComponent<controller> ();
        // RR = Player.GetComponent<controller>();

        //GameObject a = this.GetComponentInParent<GameObject>().transform.Find("camera lookAt").gameObject;
        /*cameralookAt = this.transform.parent.Find("camera lookAt").gameObject;
            cameraPos = this.transform.parent.Find("camera constraint").gameObject;*/
        /*  cameralookAt = Player.transform.Find("camera lookAt").gameObject;
          cameraPos = Player.transform.Find("camera constraint").gameObject;
      */

        /*
        cameralookAt = Player.transform.Find ("camera lookAt").gameObject;
        cameraPos = Player.transform.Find ("camera constraint").gameObject;*/

        Player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(Player.GetPhotonView().Owner);
        RR = Player.GetComponent<controller>();
        cameralookAt = Player.transform.Find("camera lookAt").gameObject;
        cameraPos = Player.transform.Find("camera constraint").gameObject;


    }

    private void FixedUpdate () 
    {
        
        follow ();
      

    }
    private void follow () 
    {
        /*
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>().IsMine)
        {
            speed = RR.KPH / smothTime;
            this.gameObject.transform.position = Vector3.Lerp(transform.position, cameraPos.transform.position, Time.deltaTime * speed);
            this.gameObject.transform.LookAt(cameralookAt.gameObject.transform.position);
        }*/
        
        speed = RR.KPH / smothTime;
        this.gameObject.transform.position = Vector3.Lerp (transform.position, cameraPos.transform.position ,  Time.deltaTime * speed);
        this.gameObject.transform.LookAt (cameralookAt.gameObject.transform.position);
    }
   

}