using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;

    public TextMeshProUGUI nickname;
    public TextMeshProUGUI currentUserInRoomNumber;
    public GameObject[] joinedUsers;
    public Sprite[] carImgs;

    public Button raceBtn;
    public Button readyBtn;

    public GameObject loadingPanel;
    public TextMeshProUGUI leftName;
    public TextMeshProUGUI rightName;
    private Hashtable CP;

    public Sprite[] trackImgs;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;

        connectionInfoText.text = "Master 서버에 접속중...";

        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        CP.Add("selectedCar", savedData.data.currentCar);
        CP.Add("wings", savedData.data.savedDownforce);
        CP.Add("win", savedData.data.userWin);
        CP.Add("lose", savedData.data.userLose);
        if(savedData.data.currentCar == 0)
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

    /*public void SetSelectedCarProperty(int num)
    {
        CP["selectedCar"] = num;
        //PhotonNetwork.LocalPlayer.SetCustomProperties
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties[ "selectedCar"]);
    }*/
    private void UpdatePlayerCounts()
    {
        for(int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            joinedUsers[i].SetActive(false);
        }
        currentUserInRoomNumber.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        for(int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            joinedUsers[i].SetActive(true);
            string nick = PhotonNetwork.CurrentRoom.Players[i + 1].ToString();
            string[] nickSplit = nick.Split(new string[] {"'"}, StringSplitOptions.None); 
            joinedUsers[i].transform.Find("NickName").gameObject.GetComponent<TextMeshProUGUI>().text = nickSplit[1];
            if(i == 0)
            {
                leftName.text = nickSplit[1];
            }
            else if (i==1)
            {
                rightName.text = nickSplit[1];
            }

            joinedUsers[i].transform.Find("carImg").gameObject.GetComponent<Image>().sprite = carImgs[(int)PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"]];
            Debug.Log(PhotonNetwork.PlayerList[0].CustomProperties["selectedCar"]); 
            //Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["selectedCar"]);
            //Debug.Log(CP["selectedCar"]);
            
            /*
            if (savedData.data.currentCar == 0)
            {
                joinedUsers[i].transform.Find("carImg").gameObject.GetComponent<Image>().sprite = carImgs[0];
            }
            else if(savedData.data.currentCar == 1)
            {
                joinedUsers[i].transform.Find("carImg").gameObject.GetComponent<Image>().sprite = carImgs[1];
            }
            else if (savedData.data.currentCar == 2)
            {
                joinedUsers[i].transform.Find("carImg").gameObject.GetComponent<Image>().sprite = carImgs[2];
            }
            //joinedUsers[i].transform.Find("carImg").gameObject.GetComponent<Image>().sprite = */
            if (i == 0)
            {
                joinedUsers[i].transform.Find("isHost").gameObject.SetActive(true);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        //targetPlayer.CustomProperties = changedProps;

        Debug.Log(changedProps["selectedCar"].ToString());
    }
    public void JoinRandomOrCreateRoom()
    {

    }
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인: Master 서버와 연결됨";
        //PhotonNetwork.LocalPlayer.NickName = nickname.text;
    }

    public void OnStartClicked()
    {
        loadingSceneController.LoadScene("LongTrack");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {

        joinButton.interactable = false;
        connectionInfoText.text = "오프라인: Master 서버와 연결되지 않음 \n접속 재시도 중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = nickname.text;
            connectionInfoText.text = "방에 참가하는 중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "오프라인: Master 서버와 연결되지 않음 \n접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName} 방 참가 ");
        UpdatePlayerCounts();
        //PhotonNetwork.LoadLevel("Multi_LongTrack");
        
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            
            if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel("Multi_LongTrack");
            }
            //PhotonNetwork.LoadLevel("Multi_LongTrack");
        }*/
    }

    public override void OnJoinedRoom()
    {
        
        //PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "selectedCar", savedData.data.currentCar } });
        /*
        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        CP.Add("selectedCar", savedData.data.currentCar);
        PhotonNetwork.LocalPlayer.SetCustomProperties(CP);*/
        UpdatePlayerCounts();
        //SetSelectedCarProperty(savedData.data.currentCar);
        //OnPlayerPropertiesUpdate(PhotonNetwork.CurrentRoom.Players, CP);
        connectionInfoText.text = "방 참가 성공";
        //PhotonNetwork.LoadLevel("Multi_LongTrack");
    }

    public void RaceBtnClicked()
    {
        loadingPanel.SetActive(true);
        PhotonNetwork.LoadLevel("Multi_LongTrack");
    }



}
