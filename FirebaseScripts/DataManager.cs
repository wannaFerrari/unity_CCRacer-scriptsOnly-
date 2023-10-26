using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class PlayerData
{
    public string name;
    public int id;
    public int level;

    public List<item> includedPowerList = new List<item>();
    public List<item> includedDriveList = new List<item>();
    public List<item> includedSpecialList = new List<item>();

    public List<item> PowerInventoryList = new List<item>();
    public List<item> DriveInventoryList = new List<item>();
    public List<item> SpecialInventoryList = new List<item>();

    public float savedTorque = 0f;
    public float savedBrake = 0f;
    public float savedWeight = 0f;
    public float savedDamper = 0f;
    public float savedSpring = 0f;
    public float savedGrip = 0f;
    public float savedAngle = 0f;
    public float savedDownforce = 0f;

    public Color supColor;
    public Color porColor;
    public Color chiColor;

}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public ItemManager itemManager;

    PlayerData currentPlayer = new PlayerData();

    string path;
    string filename = "save";

    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        path = Application.persistentDataPath + "/";
    }

    public void SaveData()
    {
        UpdateDatabase();
        string data = JsonUtility.ToJson(currentPlayer);

        File.WriteAllText(path + filename, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + filename);
        currentPlayer = JsonUtility.FromJson<PlayerData>(data);

        UpdateSavedData();

        inventory.instance.DownLoadInvenToSavedData();
        for(int i = 0; i < currentPlayer.includedPowerList.Count; i++)
        {
            itemManager.CheckToInclude(currentPlayer.includedPowerList[i]);
        }
        for(int i = 0; i < currentPlayer.includedDriveList.Count; i++)
        {
            itemManager.CheckToInclude(currentPlayer.includedDriveList[i]);
        }
        for (int i = 0; i < currentPlayer.includedSpecialList.Count; i++)
        {
            itemManager.CheckToInclude(currentPlayer.includedSpecialList[i]);
        }

    }

    public void UpdateDatabase()
    {
        currentPlayer.includedPowerList = savedData.data.includedPowerList;
        currentPlayer.includedDriveList = savedData.data.includedDriveList;
        currentPlayer.includedSpecialList = savedData.data.includedSpecialList;
        inventory.instance.UpLoadInvenToSavedData();
        
        currentPlayer.PowerInventoryList = savedData.data.PowerInventoryList;
        currentPlayer.DriveInventoryList = savedData.data.DriveInventoryList;
        currentPlayer.SpecialInventoryList = savedData.data.SpecialInventoryList;
        Debug.Log(currentPlayer.PowerInventoryList.Count);

        currentPlayer.savedTorque = savedData.data.savedTorque;
        currentPlayer.savedBrake = savedData.data.savedBrake;
        currentPlayer.savedWeight = savedData.data.savedWeight;
        currentPlayer.savedDamper = savedData.data.savedDamper;
        currentPlayer.savedSpring = savedData.data.savedSpring;
        currentPlayer.savedGrip = savedData.data.savedGrip;
        currentPlayer.savedAngle = savedData.data.savedAngle;
        currentPlayer.savedDownforce = savedData.data.savedDownforce;

        currentPlayer.supColor = savedData.data.supColor;
        currentPlayer.porColor = savedData.data.porColor;
        currentPlayer.chiColor = savedData.data.chiColor;
    }

    public void UpdateSavedData()
    {
        savedData.data.includedPowerList = currentPlayer.includedPowerList;
        savedData.data.includedDriveList = currentPlayer.includedDriveList;
        savedData.data.includedSpecialList = currentPlayer.includedSpecialList;


        

        savedData.data.PowerInventoryList = currentPlayer.PowerInventoryList;
        savedData.data.DriveInventoryList = currentPlayer.DriveInventoryList;
        savedData.data.SpecialInventoryList = currentPlayer.SpecialInventoryList;

        savedData.data.savedTorque = currentPlayer.savedTorque;
        savedData.data.savedBrake = currentPlayer.savedBrake;
        savedData.data.savedWeight = currentPlayer.savedWeight;
        savedData.data.savedDamper = currentPlayer.savedDamper;
        savedData.data.savedSpring = currentPlayer.savedSpring;
        savedData.data.savedGrip = currentPlayer.savedGrip;
        savedData.data.savedAngle = currentPlayer.savedAngle;
        savedData.data.savedDownforce = currentPlayer.savedDownforce;

        savedData.data.supColor = currentPlayer.supColor;
        savedData.data.porColor = currentPlayer.porColor;
        savedData.data.chiColor = currentPlayer.chiColor;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
