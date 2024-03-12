using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ghostPlayer : MonoBehaviour
{
    public ghost ghost;
    public ghost rainGhost;
    public ghost thisMapGhost;
    public GameObject supra;
    public GameObject porsche;
    public GameObject chiron;

    public GameObject ghostCar;

    private float timeValue;
    private int index1;
    private int index2;

    //private bool isRaining = false;

    private bool noGhosts;
    private bool startGhost = false;
    public GameManager gManager;

    private void Awake()
    {
        gManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        if (savedData.data.isRaining)
        {
            thisMapGhost = savedData.data.rainGhostDatas;

        }
        else
        {
            thisMapGhost = savedData.data.ghostDatas;
        }
        // Debug.Log("awake");
        // ghost = savedData.data.ghostDatas;
        if (!gManager.isOnline)
        {

        }
        if (thisMapGhost.savedGhostRecordTime != -1f) 
        {   
            //Debug.Log("instat front"); 
            InstantiateGhostCars(); 
            noGhosts = false; 
        }
        else noGhosts = true;
        timeValue = 0;
        index1= 0;
        index2= 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("-----------------");

        if (thisMapGhost.isGhost && !noGhosts && startGhost)
        {
            timeValue += Time.unscaledDeltaTime;
            GetIndex();
            SetTransform();

        }
    }

    private void GetIndex()
    {
        /*
        for (int i = 0; i < ghost.timeStamp.Count -2; i++)
        {
            if (ghost.timeStamp[i] == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }
            else if (ghost.timeStamp[i] < timeValue & timeValue < ghost.timeStamp[i + 1])
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }
        index1 = ghost.timeStamp.Count - 1;
        index2 = ghost.timeStamp.Count - 1;*/

        for (int i = 0; i < thisMapGhost.savedGhostTimeStamp.Count - 2; i++)
        {
            if (thisMapGhost.savedGhostTimeStamp[i] == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }
            else if (thisMapGhost.savedGhostTimeStamp[i] < timeValue & timeValue < thisMapGhost.savedGhostTimeStamp[i + 1])
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }
        index1 = thisMapGhost.savedGhostTimeStamp.Count - 1;
        index2 = thisMapGhost.savedGhostTimeStamp.Count - 1;
    }

    private void SetTransform()
    {
        if (index1 == index2)
        {
            //this.transform.position = ghost.position[index1];
            //this.transform.eulerAngles = ghost.rotation[index1];
            ghostCar.transform.position = thisMapGhost.savedGhostPosition[index1];
            ghostCar.transform.eulerAngles = thisMapGhost.savedGhostRotation[index1];
        }
        else
        {
            float interpolationFactor = (timeValue - thisMapGhost.savedGhostTimeStamp[index1]) / (thisMapGhost.savedGhostTimeStamp[index2] - thisMapGhost.savedGhostTimeStamp[index1]);

            //this.transform.position = Vector3.Lerp(ghost.position[index1], ghost.position[index2], interpolationFactor);
            //this.transform.eulerAngles = Vector3.Lerp(ghost.rotation[index1], ghost.rotation[index2], interpolationFactor);
            ghostCar.transform.position = Vector3.Lerp(thisMapGhost.savedGhostPosition[index1], thisMapGhost.savedGhostPosition[index2], interpolationFactor);
            ghostCar.transform.eulerAngles = Vector3.Lerp(thisMapGhost.savedGhostRotation[index1], thisMapGhost.savedGhostRotation[index2], interpolationFactor);
        }
    }

    private void InstantiateGhostCars()
    {
        if (thisMapGhost.savedGhostCar == 0)
        {
            ghostCar = Instantiate(supra, thisMapGhost.savedGhostPosition[0], Quaternion.identity);

        }
        else if(thisMapGhost.savedGhostCar == 1)
        {
            ghostCar = Instantiate(porsche, thisMapGhost.savedGhostPosition[0], Quaternion.identity);

        }
        else if(thisMapGhost.savedGhostCar == 2)
        {
            ghostCar = Instantiate(chiron, thisMapGhost.savedGhostPosition[0], Quaternion.identity);

        }
        ghostCar.transform.eulerAngles = thisMapGhost.savedGhostRotation[0];
    }
    public void DestroyGhostCar()
    {
        if(!noGhosts)Destroy(ghostCar);
    }
    public void StartGhost()
    {
        startGhost = true;
    }
  
}
