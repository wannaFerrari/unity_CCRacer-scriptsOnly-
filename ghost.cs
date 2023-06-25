using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ghost : ScriptableObject
{
    public bool isPlay;
    public bool isGhost;
    public float recordFrequancy;

    [Header("current ghostdatas")]
    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Vector3> rotation;
    public float currentClearedTime;
    public int currentCar;

    [Header("saved ghostdatas")]
    public List<float> savedGhostTimeStamp;
    public List<Vector3> savedGhostPosition;
    public List<Vector3> savedGhostRotation;
    public float savedGhostRecordTime = -1f;
    public int savedGhostCar;


    public void ResetData()
    {
        timeStamp.Clear();
        position.Clear();
        rotation.Clear();
        currentClearedTime = -1;
    }

    public void UpdateSavedGhostDatas()
    {
        savedGhostTimeStamp.Clear();
        savedGhostPosition.Clear();
        savedGhostRotation.Clear();
       

        savedGhostTimeStamp = timeStamp.ToList();
        savedGhostPosition = position.ToList();
        savedGhostRotation = rotation.ToList();
        savedGhostRecordTime = currentClearedTime;
        savedGhostCar= currentCar;
    }
    

    
}
