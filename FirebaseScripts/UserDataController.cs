using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;



public class UserData
{
    public string userNickName;
    public int userWin;
    public int userLose;
    public float userWinRate;

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

    /*
    public UserData(string userNickName, int userWin, int userLose, float userWinRate)
    {
        this.userNickName = userNickName;
        this.userWin = userWin;
        this.userLose = userLose;
        this.userWinRate = userWinRate;
    }*/

}

public class UserDataController : MonoBehaviour
{
    /*
    public static UserDataController Instance
    {
        get
        {
            if (ud_instance == null)
            {
                ud_instance = FindObjectOfType<UserDataController>();
            }
            return ud_instance;
        }
    }*/


    private static UserDataController ud_instance;


    public TextMeshProUGUI text;
    public string email;
    public string userNickName;
    public int userWin;
    public int userLose;
    public float userWinRate;

    public LoginSystem logSystem;
    public ItemManager itemManager;

    

    UserData userData = new UserData();

    /*
    public class UserData
    {
        public string userNickName;
        public int userWin;
        public int userLose;
        public float userWinRate;

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

        /*
        public UserData(string userNickName, int userWin, int userLose, float userWinRate)
        {
            this.userNickName = userNickName;
            this.userWin = userWin;
            this.userLose = userLose;
            this.userWinRate = userWinRate;
        }*/

    /* }*/

    private DatabaseReference databaseReference;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (ud_instance == null)
        {
            ud_instance = this;
        }
        else if (ud_instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnClickedCreateButton()
    {
        userData.userNickName = logSystem.ReturnNickName();
        userData.userWin = 0;
        userData.userLose = 0;
        userData.userWinRate = 0;
        string uid = logSystem.ReturnUserID();
        string jsonData = JsonUtility.ToJson(userData);

        databaseReference.Child(uid).SetRawJsonValueAsync(jsonData);
    }
    
    public void OnClickSaveButton()
    {
        
        itemManager = GameObject.FindGameObjectWithTag("MenuPageManager").GetComponent<ItemManager>();
        email = logSystem.ReturnEmail();
        userNickName = logSystem.ReturnNickName();
        //userWin = 100;
        //userLose = 0;
        //userWinRate = 100f;
        

        string uid = logSystem.ReturnUserID();
       

        //UserData userData = new UserData(userNickName, userWin, userLose, userWinRate);

        userData.userNickName = userNickName;
        userData.userWin = userWin;
        userData.userLose = userLose;
        userData.userWinRate = userWinRate;
        UpdateDatabase();
        string jsonData = JsonUtility.ToJson(userData);

        databaseReference.Child(uid).SetRawJsonValueAsync(jsonData);
       

       // text.text = email;
    }
    
    public void OnClickLoadButton()
    {
        itemManager = GameObject.FindGameObjectWithTag("MenuPageManager").GetComponent<ItemManager>();
        // text.text = "zzzzzzzzzzzzzzzzzzzzz";
        email = logSystem.ReturnEmail();
        string uid = logSystem.ReturnUserID();
        Debug.Log(uid);
        //databaseReference.Child(uid).GetValueAsync().ContinueWith(task =>
        FirebaseDatabase.DefaultInstance.GetReference(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                text.text = "로드 취소";
            }
            else if (task.IsFaulted)
            {
                text.text = "로드 실패";
            }
            else
            {
                DataSnapshot dataSnapshot = task.Result;

                string dataString = "";
                foreach (DataSnapshot data in dataSnapshot.Children)
                {
                    /*
                   dataString += data.Key + " " + data.Value + "\n";
                    var dd = new Dictionary<object, object>();

                    IDictionary rr = (IDictionary)data.Value;
                    Debug.Log(rr["userNickName"] + " ---" + rr["userWin"]);*/
                    /*
                    if (data.Key.Equals("userWin"))
                    {
                        Debug.Log(data.Value);
                        userWin = (int)data.Value;
                    }
                    else if (data.Key.Equals("userNickName"))
                    {
                        Debug.Log(data.Value);
                        userNickName = (string)data.Value;
                    }
                    else if (data.Key.Equals("userLose"))
                    {
                        Debug.Log(data.Value);
                        userLose = (int)data.Value;
                    }
                    else if (data.Key.Equals("userWinRate"))
                    {
                        Debug.Log(data.Value);
                        userWinRate = (float)data.Value;
                    }*/

                    //Debug.Log(data.Key + "-----------" +data.Child("userWin").Value);
                }
                
                userData = JsonUtility.FromJson<UserData>(dataSnapshot.GetRawJsonValue());
                Debug.Log(userData.userLose);
                Debug.Log(userData.userWin);
                Debug.Log(userData.userWinRate);
                userNickName = userData.userNickName;
                userWin = userData.userWin;
                userLose = userData.userLose;
                userWinRate = userData.userWinRate;
               // text.text = dataString;
               // text.gameObject.SetActive(false);
                //text.gameObject.SetActive(true);

                UpdateSavedData();

                inventory.instance.DownLoadInvenToSavedData();
                for (int i = 0; i < userData.includedPowerList.Count; i++)
                {
                    itemManager.CheckToInclude(userData.includedPowerList[i]);
                }
                for (int i = 0; i < userData.includedDriveList.Count; i++)
                {
                    itemManager.CheckToInclude(userData.includedDriveList[i]);
                }
                for (int i = 0; i < userData.includedSpecialList.Count; i++)
                {
                    itemManager.CheckToInclude(userData.includedSpecialList[i]);
                }
                //Debug.Log(dataSnapshot.email)
                savedData.data.LoadUserDataToSavedData(uid, userNickName, userWin, userLose, userWinRate);
                //SceneManager.LoadScene("GarageScene");
            }
        });
        
    }

    public void UpdateDatabase()
    {
        itemManager = GameObject.FindGameObjectWithTag("MenuPageManager").GetComponent<ItemManager>();
        userData.includedPowerList = savedData.data.includedPowerList;
        userData.includedDriveList = savedData.data.includedDriveList;
        userData.includedSpecialList = savedData.data.includedSpecialList;
        inventory.instance.UpLoadInvenToSavedData();

        userData.PowerInventoryList = savedData.data.PowerInventoryList;
        userData.DriveInventoryList = savedData.data.DriveInventoryList;
        userData.SpecialInventoryList = savedData.data.SpecialInventoryList;
        Debug.Log(userData.PowerInventoryList.Count);

        userData.savedTorque = savedData.data.savedTorque;
        userData.savedBrake = savedData.data.savedBrake;
        userData.savedWeight = savedData.data.savedWeight;
        userData.savedDamper = savedData.data.savedDamper;
        userData.savedSpring = savedData.data.savedSpring;
        userData.savedGrip = savedData.data.savedGrip;
        userData.savedAngle = savedData.data.savedAngle;
        userData.savedDownforce = savedData.data.savedDownforce;

        userData.supColor = savedData.data.supColor;
        userData.porColor = savedData.data.porColor;
        userData.chiColor = savedData.data.chiColor;

        userData.userNickName= savedData.data.userNickName;
        userData.userWin = savedData.data.userWin;
        userData.userLose = savedData.data.userLose;
        userData.userWinRate = savedData.data.UserWinRate;

    }

    public void UpdateSavedData()
    {
        itemManager = GameObject.FindGameObjectWithTag("MenuPageManager").GetComponent<ItemManager>();
        savedData.data.includedPowerList = userData.includedPowerList;
        savedData.data.includedDriveList = userData.includedDriveList;
        savedData.data.includedSpecialList = userData.includedSpecialList;




        savedData.data.PowerInventoryList = userData.PowerInventoryList;
        savedData.data.DriveInventoryList = userData.DriveInventoryList;
        savedData.data.SpecialInventoryList = userData.SpecialInventoryList;

        /*
        savedData.data.savedTorque = userData.savedTorque;
        savedData.data.savedBrake = userData.savedBrake;
        savedData.data.savedWeight = userData.savedWeight;
        savedData.data.savedDamper = userData.savedDamper;
        savedData.data.savedSpring = userData.savedSpring;
        savedData.data.savedGrip = userData.savedGrip;
        savedData.data.savedAngle = userData.savedAngle;
        savedData.data.savedDownforce = userData.savedDownforce;*/
            
        savedData.data.supColor = userData.supColor;
        savedData.data.porColor = userData.porColor;
        savedData.data.chiColor = userData.chiColor;

        savedData.data.userNickName= userData.userNickName;
        savedData.data.userWin = userData.userWin;  
        savedData.data.userLose = userData.userLose;
        savedData.data.UserWinRate = userData.userWinRate;
    }
    // Start is called before the first frame update

}
