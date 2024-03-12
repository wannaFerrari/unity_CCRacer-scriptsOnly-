using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
//using JetBrains.Annotations;
//using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;

    public TextMeshProUGUI nickname;
    public TextMeshProUGUI currentUserInRoomNumber;
    public TextMeshProUGUI rList;
    public GameObject[] joinedUsers;
    public Sprite[] carImgs;
    public Sprite[] mapImgs;

    public Button raceBtn;
    public Button readyBtn;

    [Header("Canvas")]
    public GameObject roomCanvas;
    public GameObject lobbyMainCanvas;
    public GameObject createCanvas;
    public GameObject connectingCanvas;

    [Header("roomListInfo")]
    public GameObject roomInfoPrefab1;
    public GameObject roomInfoPrefab2;

    [Header("scroll View")]
    public UnityEngine.UIElements.ScrollView scrollVew;
    public GameObject gs;

    [Header("lobbyMain")]
    public GameObject notExist;


    public GameObject loadingPanel;

    [Header("inRoom")]
    public TextMeshProUGUI leftName;
    public TextMeshProUGUI rightName;
    public TextMeshProUGUI leftCar;
    public TextMeshProUGUI rightCar;
    public TextMeshProUGUI leftWinLose;
    public TextMeshProUGUI rightWinLose;
    public TextMeshProUGUI leftWinRate;
    public TextMeshProUGUI rightWinRate;
    public TextMeshProUGUI thisRoomName;
    public GameObject noPlayersJoined;
    public Image selectedTrackImg;
    public TextMeshProUGUI selectedTrackName;
    public GameObject selectedTrackImgObject;
    public GameObject selectedTrackNameObject;
    public GameObject inRoomRaceBtn;
    public GameObject inRoomReadyBtn;
    public GameObject inRoomCancelReadyBtn;
    public GameObject inRoomExitBtn;
    public GameObject isReadyText;
    public GameObject left50;
    public GameObject left70;
    public GameObject right50;
    public GameObject right70;

    [Header("createCanvas")]
    public TMP_Dropdown mapSelect;
    public TMP_InputField typedRoomName;

    [Header("clickSound")]
    public AudioClip clickSound;
    //public AudioClip startSound;
    public AudioSource audioSource;
   
    //private Hashtable CP;

    public Sprite[] trackImgs;

    public GameObject rpcReady;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;

        //connectionInfoText.text = "Master 서버에 접속중...";
        savedData.data.isOnline = true;
        Hashtable CP = new();

        //PhotonNetwork.LocalPlayer.SetCustomProperties(null);
        PhotonNetwork.LocalPlayer.CustomProperties = CP;
        CP = PhotonNetwork.LocalPlayer.CustomProperties;
        //PhotonNetwork.LocalPlayer.SetCustomProperties(null);


        CP.Add("selectedCar", savedData.data.currentCar);
        CP.Add("wings", savedData.data.savedDownforce);
        CP.Add("win", savedData.data.userWin);
        CP.Add("lose", savedData.data.userLose);
        CP.Add("winRate", savedData.data.UserWinRate);
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
       // Debug.Log("zzzzzzzzzzzzz" + PhotonNetwork.CurrentRoom.PlayerCount + "    zzz    " + PhotonNetwork.CurrentRoom.Players[0] +"-----"+ PhotonNetwork.CurrentRoom.Players[1]);
        for(int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            joinedUsers[i].SetActive(false);
        }
        currentUserInRoomNumber.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        for(int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            joinedUsers[i].SetActive(true);
            string nick = PhotonNetwork.PlayerList[i].ToString();
            string[] nickSplit = nick.Split(new string[] {"'"}, StringSplitOptions.None); 
            joinedUsers[i].transform.Find("NickName").gameObject.GetComponent<TextMeshProUGUI>().text = nickSplit[1];
            if(i == 0)
            {
                leftName.text = nickSplit[1];
                if((int)PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"] == 0)
                {
                    leftCar.text = "GR Supra";
                }
                else if ((int)PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"] == 1)
                {
                    leftCar.text = "718 Cayman GTS";
                }
                else if ((int) PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"] == 2)
                {
                    leftCar.text = "Chiron";
                }
                leftWinLose.text = $"전적 : {(int)PhotonNetwork.PlayerList[i].CustomProperties["win"]}승 {(int)PhotonNetwork.PlayerList[i].CustomProperties["lose"]}패";
                //leftWinLose.text = $"zzzzzzzzzzzzzzz";
                //Debug.Log(((int)PhotonNetwork.PlayerList[i].CustomProperties["win"]).ToString());
                //leftLose.text = (string)PhotonNetwork.PlayerList[i].CustomProperties["lose"];
                if (((int)PhotonNetwork.PlayerList[i].CustomProperties["win"] == 0) && ((int)PhotonNetwork.PlayerList[i].CustomProperties["lose"] == 0))
                {
                    leftWinRate.text = "0%";
                }
                else if ((int)PhotonNetwork.PlayerList[i].CustomProperties["lose"] == 0)
                {
                    leftWinRate.text = "100%";
                }
                else
                {
                    //leftWinRate.text = ((int)PhotonNetwork.PlayerList[i].CustomProperties["win"] / (int)PhotonNetwork.PlayerList[i].CustomProperties["lose"]).ToString();
                    leftWinRate.text = $"승률 : {(double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"]}%";
                }
                if((double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"] >= 50.0 && (double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"] < 70.0)
                {
                    left50.SetActive(true);
                    left70.SetActive(false);
                    
                }
                else if((double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"] >= 70.0)
                {
                    left50.SetActive(false);
                    left70.SetActive(true);
                }
                else
                {
                    left50.SetActive(false);
                    left70.SetActive(false);
                }

            }
            else if (i == 1)
            {
                rightName.text = nickSplit[1];
                if ((int)PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"] == 0)
                {
                    rightCar.text = "GR Supra";
                }
                else if ((int)PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"] == 1)
                {
                    rightCar.text = "718 Cayman GTS";
                }
                else if ((int)PhotonNetwork.PlayerList[i].CustomProperties["selectedCar"] == 2)
                {
                    rightCar.text = "Chiron";
                }
                rightWinLose.text = $"전적 : {(int)PhotonNetwork.PlayerList[i].CustomProperties["win"]}승 {(int)PhotonNetwork.PlayerList[i].CustomProperties["lose"]}패";
                //rightLose.text = (string)PhotonNetwork.PlayerList[i].CustomProperties["lose"];
                if (((int)PhotonNetwork.PlayerList[i].CustomProperties["win"] == 0) && ((int)PhotonNetwork.PlayerList[i].CustomProperties["lose"] == 0))
                {
                    rightWinRate.text = "0%";
                }
                else if ((int)PhotonNetwork.PlayerList[i].CustomProperties["lose"] == 0)
                {
                    rightWinRate.text = "100%";
                }
                else
                {
                    //rightWinRate.text = ((int)PhotonNetwork.PlayerList[i].CustomProperties["win"] / (int)PhotonNetwork.PlayerList[i].CustomProperties["lose"]).ToString();
                    rightWinRate.text = $"승률 : {(double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"]}%";
                }
                if ((double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"] >= 50.0 && (double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"] < 70.0)
                {
                    right50.SetActive(true);
                    right70.SetActive(false);

                }
                else if ((double)PhotonNetwork.PlayerList[i].CustomProperties["winRate"] >= 70.0)
                {
                    right50.SetActive(false);
                    right70.SetActive(true);
                }
                else
                {
                    right50.SetActive(false);
                    right70.SetActive(false);
                }
            }
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                noPlayersJoined.SetActive(true);
            }
            else
            {
                noPlayersJoined.SetActive(false);
            }



            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                inRoomRaceBtn.SetActive(true);
            }
            else
            {
                inRoomReadyBtn.SetActive(false) ;
                inRoomReadyBtn.SetActive(true);
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count> 0)
        {
            notExist.SetActive(false);
            foreach (Transform child in GameObject.FindGameObjectWithTag("Content").transform)
            {
                Destroy(child.gameObject);
            }
            for(int i = 0; i < roomList.Count; i++)
            {
               // rList.text = $"{i} + {roomList[i].Name}";
                Debug.Log(roomList[i].CustomProperties["Track Name"] +"   "+ roomList[i].CustomProperties["Host"]+"   "+ roomList[i].CustomProperties["Track Num"]);
                Debug.Log(roomList[i].PlayerCount);
                if (roomList[i].PlayerCount == 2)
                {
                    int index = i;
                    var ri = Instantiate(roomInfoPrefab2);
                    ri.transform.Find("Room NameT").gameObject.GetComponent<TextMeshProUGUI>().text = (string)roomList[index].CustomProperties["Room Name"];
                    ri.transform.Find("Track NameT").gameObject.GetComponent<TextMeshProUGUI>().text = (string)roomList[index].CustomProperties["Track Name"];
                    ri.transform.Find("Host NameT").gameObject.GetComponent<TextMeshProUGUI>().text = (string)roomList[index].CustomProperties["Host"];
                    ri.transform.Find("players in room").gameObject.GetComponent<TextMeshProUGUI>().text = $"{roomList[index].PlayerCount} / {roomList[index].MaxPlayers}";
                    if((int)roomList[index].CustomProperties["Track Num"] == 0)
                    {
                        ri.transform.Find("TrackImg").gameObject.GetComponent<Image>().sprite = mapImgs[0];
                    }
                    else if ((int)roomList[index].CustomProperties["Track Num"] == 1)
                    {
                        ri.transform.Find("TrackImg").gameObject.GetComponent<Image>().sprite = mapImgs[1];
                    }
                    ri.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
                }
                else
                {
                    int index = i;
                    var ri = Instantiate(roomInfoPrefab1);
                    ri.transform.Find("Room NameT").gameObject.GetComponent<TextMeshProUGUI>().text = (string)roomList[index].CustomProperties["Room Name"];
                    ri.transform.Find("Track NameT").gameObject.GetComponent<TextMeshProUGUI>().text = (string)roomList[index].CustomProperties["Track Name"];
                    ri.transform.Find("Host NameT").gameObject.GetComponent<TextMeshProUGUI>().text = (string)roomList[index].CustomProperties["Host"];
                    ri.transform.Find("players in room").gameObject.GetComponent<TextMeshProUGUI>().text = $"{roomList[index].PlayerCount} / {roomList[index].MaxPlayers}";
                    ri.transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

                    //ri.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() => JoinBtnClicked((string)roomList[i].CustomProperties["Room Name"]));
                    if ((int)roomList[index].CustomProperties["Track Num"] == 0)
                    {
                        ri.transform.Find("TrackImg").gameObject.GetComponent<Image>().sprite = mapImgs[0];
                    }
                    else if ((int)roomList[index].CustomProperties["Track Num"] == 1)
                    {
                        ri.transform.Find("TrackImg").gameObject.GetComponent<Image>().sprite = mapImgs[1];
                    }
                    ri.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() => JoinBtnClicked((string)roomList[index].CustomProperties["Room Code"]));
                    ri.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
                }
                

                
            }
           
        }
        else
        {
            notExist.SetActive(true);
            rList.text = roomList.Count.ToString();
        }
        //rList.text = roomList.Count.ToString();
        
        //base.OnRoomListUpdate(roomList);
    }
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인: Master 서버와 연결됨";
        
        //--------------------------------
        PhotonNetwork.JoinLobby();

        
        PhotonNetwork.LocalPlayer.NickName = savedData.data.userNickName;

        connectingCanvas.SetActive(false);
        lobbyMainCanvas.SetActive(true);
        //--------------------------------


        //PhotonNetwork.LocalPlayer.NickName = nickname.text;
    }

    public void OnStartClicked()
    {
        loadingSceneController.LoadScene("LongTrack");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnnnnnnnnnnnnnnnnnnnnnnn");
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인: Master 서버와 연결되지 않음 \n접속 재시도 중...";

        Start();
        //PhotonNetwork.ConnectUsingSettings();

    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.LocalPlayer.NickName = nickname.text;
            PhotonNetwork.LocalPlayer.NickName = savedData.data.userNickName;
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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        string realRoomName = RandomString();
        roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", $"{PhotonNetwork.LocalPlayer.NickName}의 게임" }, 
            { "Track Name", "NightRain Track" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 1 } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] {"Room Code", "Room Name", "Track Name", "Host", "Track Num" };
        PhotonNetwork.CreateRoom($"{ PhotonNetwork.LocalPlayer.NickName}의 게임", roomOptions, null);

        
        
        //PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, new RoomOptions { MaxPlayers = 2 });

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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCounts();
        isReadyText.SetActive(false);
        inRoomRaceBtn.SetActive(true);
        inRoomReadyBtn.SetActive(false);
        inRoomCancelReadyBtn.SetActive(false);
        inRoomRaceBtn.GetComponent<Button>().interactable = false;
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
        thisRoomName.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["Room Name"];
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            inRoomRaceBtn.SetActive(true);
            inRoomReadyBtn.SetActive(false);
            inRoomCancelReadyBtn.SetActive(false);
        }
        else
        {
            inRoomRaceBtn.SetActive(false);
            inRoomReadyBtn.SetActive(true);
            inRoomCancelReadyBtn.SetActive(false);
        }
        
        if((int)PhotonNetwork.CurrentRoom.CustomProperties["Track Num"] == 0)
        {
            selectedTrackImg.sprite = trackImgs[0];
            selectedTrackName.text = "SPA서킷 - 낮";
            selectedTrackImgObject.SetActive(true);
            selectedTrackNameObject.SetActive(true);
        }
        else if((int)PhotonNetwork.CurrentRoom.CustomProperties["Track Num"] == 1)
        {
            selectedTrackImg.sprite = trackImgs[1];
            selectedTrackName.text = "SPA서킷 - 밤";
            selectedTrackImgObject.SetActive(true);
            selectedTrackNameObject.SetActive(true);
        }
        roomCanvas.SetActive(true);
        //PhotonNetwork.LoadLevel("Multi_LongTrack");
    }

    public override void OnLeftRoom()
    {
        roomCanvas.SetActive(false);
        lobbyMainCanvas.SetActive(true);
        inRoomReadyBtn.SetActive(true);
        inRoomCancelReadyBtn.SetActive(false);
        isReadyText.SetActive(false);
        inRoomRaceBtn.GetComponent<Button>().interactable = false;
        //PhotonNetwork.Disconnect();

    }

    public void RaceBtnClicked()
    {
        clickSoundPlay();
        //loadingPanel.SetActive(true);
        PhotonView pv = rpcReady.GetComponent<PhotonView>();
        if((int)PhotonNetwork.CurrentRoom.CustomProperties["Track Num"] == 0)
        {
            savedData.data.isRaining = false;
            RPCSetAllUserIsRainingOrNot(false);
            pv.RPC("PopUpLoadingPanel", RpcTarget.All, leftName.text, rightName.text, 0);
            PhotonNetwork.LoadLevel("Multi_LongTrack");
        }
        else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Track Num"] == 1)
        {
            savedData.data.isRaining = true;
            RPCSetAllUserIsRainingOrNot(true);
            pv.RPC("PopUpLoadingPanel", RpcTarget.All, leftName.text, rightName.text, 1);
            PhotonNetwork.LoadLevel("Multi_RainLongTrack");
        }
        //pv.RPC("PopUpLoadingPanel", RpcTarget.All,leftName.text, rightName.text);
        //PhotonNetwork.LoadLevel("Multi_RainLongTrack");
    }

    public void JoinBtnClicked(string roomName)
    {
        clickSoundPlay();
        PhotonNetwork.LocalPlayer.NickName = savedData.data.userNickName;
        PhotonNetwork.JoinRoom(roomName);
        inRoomCancelReadyBtn.SetActive(false);
        inRoomRaceBtn.SetActive(true);

        lobbyMainCanvas.SetActive(false);
        //roomCanvas.SetActive(true);
    }

    public void BackBtnClicked()
    {
        clickSoundPlay();
        loadingSceneController.LoadScene("GarageScene");
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }
    public void CreateBtnInLobbyClicked()
    {
        clickSoundPlay();
        lobbyMainCanvas.SetActive(false);
        createCanvas.SetActive(true);

    }
    public void CreateBtnClicked()
    {
        clickSoundPlay();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        string realRoomName = RandomString();
        
        if(mapSelect.value == 0)
        {
            roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", typedRoomName.text },
            { "Track Name", "SPA서킷 - 낮" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 0 } };
        }
        else if(mapSelect.value == 1)
        {
            roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", typedRoomName.text },
            { "Track Name", "SPA서킷 - 밤" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 1 } };
        }
        roomOptions.CleanupCacheOnLeave = true;
        /*
        roomOptions.CustomRoomProperties = new Hashtable() { { "Room Code", realRoomName }, {"Room Name", $"{PhotonNetwork.LocalPlayer.NickName}의 게임" },
            { "Track Name", "NightRain Track" }, { "Host", PhotonNetwork.LocalPlayer.NickName }, { "Track Num", 1 } };*/
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "Room Code", "Room Name", "Track Name", "Host", "Track Num" };
        PhotonNetwork.CreateRoom(realRoomName, roomOptions, null);
        createCanvas.SetActive(false);
        roomCanvas.SetActive(true);
        inRoomRaceBtn.SetActive(true);
        inRoomReadyBtn.SetActive(false);
        inRoomCancelReadyBtn.SetActive(false);
    }


    private static System.Random random = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값

    public static string RandomString(int _nLength = 12)
    {
        const string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  //문자 생성 풀
        char[] chRandom = new char[_nLength];

        for (int i = 0; i < _nLength; i++)
        {
            chRandom[i] = strPool[random.Next(strPool.Length)];
        }
        string strRet = new string(chRandom);   // char to string
        return strRet;
    }
    
    public void JoinedUserClickedReady()
    {
        clickSoundPlay();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            inRoomRaceBtn.GetComponent<Button>().interactable = true;
            
        }
        else
        {
            inRoomReadyBtn.SetActive(false);
            inRoomCancelReadyBtn.SetActive(true);
            //inRoomExitBtn.GetComponent<Button>().interactable = false;
            
        }
        isReadyText.SetActive(true);
    }

    public void JoinedUserCanceledReady()
    {
        clickSoundPlay();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            inRoomRaceBtn.GetComponent<Button>().interactable = false;

        }
        else
        {
            inRoomCancelReadyBtn.SetActive(false);
            inRoomReadyBtn.SetActive(true);
            inRoomExitBtn.GetComponent<Button>().interactable = true;
            //inRoomCancelReadyBtn.GetComponent<Button>().enabled = false;


        }
        isReadyText.SetActive(false);
    }

    public void CreateBackBtnClicked()
    {
        clickSoundPlay();
        createCanvas.SetActive(false);
        lobbyMainCanvas.SetActive(true);
    }

    public void InRoomExitBtnClicked()
    {
        clickSoundPlay();
        PhotonNetwork.LeaveRoom();
        
    }

    public void RPCJoinedUserCanceledReady()
    {
        PhotonView pv = rpcReady.GetComponent<PhotonView>();
        pv.RPC("JoinedUserCanceledReady", RpcTarget.All);
    }

    public void RPCJoinedUserClickedReady() 
    {
        PhotonView pv = rpcReady.GetComponent<PhotonView>();
        pv.RPC("JoinedUserClickedReady", RpcTarget.All);
    }

    public void RPCSetAllUserIsRainingOrNot(bool isRaining)
    {
        PhotonView pv = rpcReady.GetComponent<PhotonView>();
        pv.RPC("SetIsRainingOrNot", RpcTarget.All, isRaining);
    }

    void clickSoundPlay()
    {

        audioSource.clip = clickSound;
        audioSource.Play();
    }

}
