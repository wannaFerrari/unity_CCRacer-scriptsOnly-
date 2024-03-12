using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savedData : MonoBehaviour
{
    public static savedData data;

    public int currentCar = 0;
    public bool activateGhost;
    public bool playerIsPlaying = true;
    public ghost ghostDatas;
    public ghost rainGhostDatas;
    public bool isOnline = false;
    public bool isRaining = false;

    public item turbo;
    public item air;
    public item intakePipe;
    public item throttleBody;
    public item hicam;
    public item throttle;
    public item interCooler;
    public item oilCooler;
    public item Radiator;

    public item disk;
    public item pad;
    public item shoba;
    public item spring;
    public item angle;
    public item tire;

    public item wing;
    public item bodykit;

    public List<item> includedPowerList = new List<item>();
    public List<item> includedDriveList = new List<item>();
    public List<item> includedSpecialList= new List<item>();
    /*public GameObject turbo;
   public GameObject air;
   public GameObject intakePipe;
   public GameObject throttleBody;
   public GameObject hicam;
   public GameObject throttle;
   public GameObject interCooler;
   public GameObject oilCooler;
   public GameObject Radiator;*/



    public float savedTorque = 0f;
    public float savedBrake = 0f;
    public float savedWeight = 0f;
    public float savedDamper = 0f;
    public float savedSpring = 0f;
    public float savedGrip = 0f;
    public float savedAngle = 0f;
    public float savedDownforce = 0f;

    public List<item> PowerInventoryList = new List<item>();
    public List<item> DriveInventoryList = new List<item>();
    public List<item> SpecialInventoryList = new List<item>();

    [Header("save Color to database")]
    public Color supColor;
    public Color porColor;
    public Color chiColor;

    [Header("save Login data")]
    public string userUid = "";
    public string userNickName = "";
    public int userWin = 0;
    public int userLose = 0;
    public double UserWinRate = 0;

    [Header("Need Update Datas After Game")]
    public bool needUpdateData = false;

    [Header("Datas for TimeAttack Records")]
    public float timeAttackDayTrackClearedTimeFloat = -1f;
    public float timeAttackNightTrackClearedTimeFloat = -1f;
    public int timeAttackDayTrackClearedCar;
    public int timeAttackNightTrackClearedCar;
    public string timeAttackDayTrackClearedDate;
    public string timeAttackNightTrackClearedDate;



    public bool isInitial = true;
    //public bool firstSave = true;
    // Start is called before the first frame update
    private void Awake()
    {
        includedPowerList.Add(turbo);
        includedPowerList.Add(air);
        includedPowerList.Add(intakePipe);
        includedPowerList.Add(throttleBody);
        includedPowerList.Add(hicam);
        includedPowerList.Add(throttle);
        includedPowerList.Add(interCooler);
        includedPowerList.Add(oilCooler);
        includedPowerList.Add(Radiator);

        includedDriveList.Add(disk);
        includedDriveList.Add(pad);
        includedDriveList.Add(shoba);
        includedDriveList.Add(spring);
        includedDriveList.Add(angle);
        includedDriveList.Add(tire);

        includedSpecialList.Add(wing);
        includedSpecialList.Add(bodykit);

        //Debug.Log("----------------------------------------");
        DontDestroyOnLoad(gameObject);

        if(data == null)
        {
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }
    /*
    public void AfterAddPowerInven(item _item , int i)
    {
        PowerInventoryList[i] = _item;
    }
    public void AfterAddDriveInven(item _item , int i)
    {
        DriveInventoryList[i] = _item;
    }

    */
    public void ClearInvenList()
    {
        PowerInventoryList.Clear();
        DriveInventoryList.Clear();
        SpecialInventoryList.Clear();
    }
    public void AddPowerInvenItems(item _item)
    {
        PowerInventoryList.Add(_item);
        /*if (firstSave)
        {
            PowerInventoryList.Add(_item);
            firstSave = !firstSave;
        }*/
        //PowerInventoryList.Clear();
        //PowerInventoryList.Add(_item);
       /* else
        {
            PowerInventoryList[i] = _item;
        }*/
    }
    public void AddDriveInvenItems(item _item)
    {
        DriveInventoryList.Add(_item);
       /* if (firstSave)
        {
            DriveInventoryList.Add(_item);
            firstSave = !firstSave;
        }*/
        //DriveInventoryList.Clear();
        //DriveInventoryList.Add(_item);
        /*else
        {
            DriveInventoryList[i] = _item;
        }*/
    }

    public void AddSpeicalInvenItems(item _item)
    {
        //SpecialInventoryList.Clear();
        SpecialInventoryList.Add(_item);
        //SpecialInventoryList[i] = _item;
    }

    public item[] ReturnPowerInvenItems()
    {
        
        return PowerInventoryList.ToArray();
    }
    public item[] ReturnDriveInvenItems()
    {
        return DriveInventoryList.ToArray();
    }
    public item[] ReturnSpecialInvenItems()
    {
        return SpecialInventoryList.ToArray();
    }

    public void ChangeInitialInventory()
    {
        isInitial = false;
    }
    public bool ReturnInitialInventory()
    {
        return isInitial;
    }

    public void LoadUserDataToSavedData(string Uid, string Nick, int Win, int Lose, double WinRate)
    {
        userUid = Uid;
        userNickName = Nick;
        userWin = Win;
        userLose = Lose;
        UserWinRate = WinRate;
    }

    public void NeedUpdateDatas()
    {
        needUpdateData = true;
    }

    public void DataUpdateCompleted()
    {
        needUpdateData = false;
    }

    public bool CheckNeedUpdate()
    {
        return needUpdateData;
    }
}
