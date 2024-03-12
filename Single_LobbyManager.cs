using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Single_LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    [Header("Panels")]
    public GameObject trackSelectPanel;
    public GameObject dayTrackPanel;
    public GameObject nightTrackPanel;
    public GameObject connectingPanel;
    public GameObject loadingPanel;

    [Header("RecordInfo")]
    public TextMeshProUGUI dayBestRecord;
    public TextMeshProUGUI nightBestRecord;
    public TextMeshProUGUI dayBestDate;
    public TextMeshProUGUI nightBestDate;
    public TextMeshProUGUI dayBestCar;
    public TextMeshProUGUI nightBestCar;
    public TextMeshProUGUI dayCurrentCar;
    public TextMeshProUGUI nightCurrentCar;

    [Header("Click Sound")]
    public AudioClip clickSound;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        savedData.data.isOnline = false;

        Hashtable CP = new();

        //PhotonNetwork.LocalPlayer.SetCustomProperties(null);
        PhotonNetwork.LocalPlayer.CustomProperties = CP;
        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(null);


        CP.Add("selectedCar", savedData.data.currentCar);
        CP.Add("wings", savedData.data.savedDownforce);
        CP.Add("win", savedData.data.userWin);
        CP.Add("lose", savedData.data.userLose);
        if (savedData.data.currentCar == 0)
        {
            CP.Add("colorR", (float)savedData.data.supColor.r * 255);
            CP.Add("colorG", (float)savedData.data.supColor.g * 255);
            CP.Add("colorB", (float)savedData.data.supColor.b * 255);
        }
        else if (savedData.data.currentCar == 1)
        {
            CP.Add("colorR", (float)savedData.data.porColor.r * 255);
            CP.Add("colorG", (float)savedData.data.porColor.g * 255);
            CP.Add("colorB", (float)savedData.data.porColor.b * 255);
        }
        else if (savedData.data.currentCar == 2)
        {
            CP.Add("colorR", (float)savedData.data.chiColor.r * 255);
            CP.Add("colorG", (float)savedData.data.chiColor.g * 255);
            CP.Add("colorB", (float)savedData.data.chiColor.b * 255);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(CP);
        /*
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "selectedCar", -1 } });
        CP = PhotonNetwork.LocalPlayer.CustomProperties;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        //targetPlayer.CustomProperties = changedProps;

        Debug.Log(changedProps["selectedCar"].ToString());
    }

    public override void OnConnectedToMaster()
    {
       // joinButton.interactable = true;
        //connectionInfoText.text = "¿Â¶óÀÎ: Master ¼­¹ö¿Í ¿¬°áµÊ";

        //--------------------------------
        PhotonNetwork.JoinLobby();

        PhotonNetwork.LocalPlayer.NickName = savedData.data.userNickName;
        connectingPanel.SetActive(false);
        trackSelectPanel.SetActive(true);
        //--------------------------------


        //PhotonNetwork.LocalPlayer.NickName = nickname.text;
    }

    public void CreateBtnClicked()
    {
        clickSoundPlay();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = false;
        roomOptions.IsOpen = false;
        // string realRoomName = RandomString();

        //roomOptions.CustomRoomProperties = new Hashtable() { {"Room Name", "111111111" },
           // { "Track Name", "SPA¼­Å¶ - ³·" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 0 } };
        /*
        if (mapSelect.value == 0)
        {
            roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", typedRoomName.text },
            { "Track Name", "SPA¼­Å¶ - ³·" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 0 } };
        }
        else if (mapSelect.value == 1)
        {
            roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", typedRoomName.text },
            { "Track Name", "SPA¼­Å¶ - ¹ã" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 1 } };
        }*/
        roomOptions.CleanupCacheOnLeave = true;
        /*
        roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", $"{PhotonNetwork.LocalPlayer.NickName}ÀÇ °ÔÀÓ" },
            { "Track Name", "NightRain Track" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 1 } };*/
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { "Room Code", "Room Name", "Track Name", "Host", "Track Num" };
        PhotonNetwork.CreateRoom("zzzzzzzzzz", roomOptions, null);
        //createCanvas.SetActive(false);
        //roomCanvas.SetActive(true);
    }

    public void RaceBtnClicked()
    {
        loadingPanel.SetActive(true);
        clickSoundPlay();
        savedData.data.isRaining = false;
        //loadingPanel.SetActive(true);
        PhotonNetwork.LoadLevel("Single_LongTrack");
    }
    public void RainRaceBtnClicked()
    {
        loadingPanel.SetActive(true);
        clickSoundPlay();
        savedData.data.isRaining = true;
        //loadingPanel.SetActive(true);
        PhotonNetwork.LoadLevel("Single_RainLongTrack");
    }

    public void DayTrackClicked()
    {
        clickSoundPlay();
        if (savedData.data.timeAttackDayTrackClearedTimeFloat == -1f)
        {
            dayBestRecord.text = "No Record";
            dayBestCar.text = "No Record";
            dayBestDate.text = "No Record";
        }
        else
        {
            dayBestRecord.text = TimeFloatChanger(savedData.data.timeAttackDayTrackClearedTimeFloat);
            if(savedData.data.timeAttackDayTrackClearedCar == 0)
            {
                dayBestCar.text = "GR Supra";
            }
            else if (savedData.data.timeAttackDayTrackClearedCar == 1)
            {
                dayBestCar.text = "718 Cayman GTS";
            }
            else if (savedData.data.timeAttackDayTrackClearedCar == 2)
            {
                dayBestCar.text = "Chiron";
            }
            dayBestDate.text = savedData.data.timeAttackDayTrackClearedDate;
        }

        if (savedData.data.currentCar == 0)
        {
            dayCurrentCar.text = "GR Supra";
        }
        else if (savedData.data.currentCar == 1)
        {
            dayCurrentCar.text = "718 Cayman GTS";
        }
        else if (savedData.data.currentCar == 2)
        {
            dayCurrentCar.text = "Chiron";
        }

        trackSelectPanel.SetActive(false);
        dayTrackPanel.SetActive(true);
        CreateBtnClicked();
    }

    public void NightTrackClicked()
    {
        clickSoundPlay();
        if (savedData.data.timeAttackNightTrackClearedTimeFloat == -1f)
        {
            nightBestRecord.text = "No Record";
            nightBestCar.text = "No Record";
            nightBestDate.text = "No Record";
        }
        else
        {
            nightBestRecord.text = TimeFloatChanger(savedData.data.timeAttackNightTrackClearedTimeFloat);
            if (savedData.data.timeAttackNightTrackClearedCar == 0)
            {
                nightBestCar.text = "GR Supra";
            }
            else if (savedData.data.timeAttackNightTrackClearedCar == 1)
            {
                nightBestCar.text = "718 Cayman GTS";
            }
            else if (savedData.data.timeAttackNightTrackClearedCar == 2)
            {
                nightBestCar.text = "Chiron";
            }
            nightBestDate.text = savedData.data.timeAttackNightTrackClearedDate;
        }

        if (savedData.data.currentCar == 0)
        {
            nightCurrentCar.text = "GR Supra";
        }
        else if (savedData.data.currentCar == 1)
        {
            nightCurrentCar.text = "718 Cayman GTS";
        }
        else if (savedData.data.currentCar == 2)
        {
            nightCurrentCar.text = "Chiron";
        }

        trackSelectPanel.SetActive(false);
        nightTrackPanel.SetActive(true);
        CreateBtnClicked();
    }

    public void BackBtnInTrackSelectClicked()
    {
        clickSoundPlay();
        loadingSceneController.LoadScene("GarageScene");
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public void StartBtnInDayPanelClicked()
    {
        clickSoundPlay();
        loadingPanel.SetActive(true);
        savedData.data.isRaining = false;
        PhotonNetwork.LoadLevel("Single_LongTrack");
    }

    public void StartBtnInNightPanelClicked()
    {
        clickSoundPlay();
        loadingPanel.SetActive(true);
        savedData.data.isRaining = true;
        PhotonNetwork.LoadLevel("Single_RainLongTrack");
    }

    public void BackBtnInDayPanelClicked()
    {
        clickSoundPlay();
        dayTrackPanel.SetActive(false);
        trackSelectPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void BackBtnInNightPanelClicked()
    {
        clickSoundPlay();
        nightTrackPanel.SetActive(false);
        trackSelectPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public string TimeFloatChanger(float clearedTime)
    {
        TimeSpan timespan2 = TimeSpan.FromSeconds(clearedTime);
        string timer2 = string.Format("{1:00}:{2:00}.{3:00}",
            timespan2.Hours, timespan2.Minutes, timespan2.Seconds, timespan2.Milliseconds);

        return timer2;
    }

    void clickSoundPlay()
    {

        audioSource.clip = clickSound;
        audioSource.Play();
    }
}
