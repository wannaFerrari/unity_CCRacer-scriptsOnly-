using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private static GameManager m_instance;
    public GameObject car;
    //public vehicleList list;
    public controller RR;
    public GameObject needle;
    public GameObject startPosition;
    public Text kph;
    public Text currentPosition;
    public Text gearNum;
    private float startPosiziton = 174f, endPosition = -90f;   //32f ,  -211f
    private float desiredPosition;

    private int count4 = 0;
    private int count3 = 0;
    private int count2 = 0;
    private int count1 = 0;
    private int finishSound = 0;
    private int recordSound = 0;

    private int currentLap = -1;

    private bool Finished = false;

    public startScript startScript;
    private float finishCounter = 0;

    [Header("countdown Timer")]
    public float timeLeft = 9;
    public Text timeLeftText;
    private float timeLeftFloat;

    [Header("UI")]
    public GameObject RaceUI;
    public GameObject SpeedUI;
    //public GameObject timeAttack;
    public GameObject BestRecord;
    public Text BestRecordTime;

    public lapUIManager lpM;

    public GameObject firstPosition;
    public GameObject secondPosition;

    [Header("Timer")]
    public string m_Timer = @"00:00.000";
    public KeyCode m_KcdPlay = KeyCode.Space;
    private bool m_IsPlaying= false;
    public float m_TotalSeconds; 
    public Text m_Text;

    [Header("Images")]
    public GameObject RL1;
    public GameObject RL2;
    public GameObject RL3;
    public GameObject GL1;
    public GameObject GL2;
    public GameObject GL3;
    public GameObject GL4;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera leftCam, frontCam, rightCam;
    private Camera[] Cameras;

    [Header("ReSpawn")]
    public GameObject SpawnPoint;

    [Header("Start Banner")]
    public GameObject TimeAttackBanner;
    public GameObject SupraBanner;
    public GameObject PorscheBanner;
    public GameObject ChironBanner;

    [Header("Selected Car")]
    public GameObject Supra;
    public GameObject Porsche;
    public GameObject Chiron;

    [Header("Ghost Cars")]
    public GameObject SupraGhost;
    public GameObject ChironGhost;
    public GameObject PorscheGhost;
    public ghostPlayer ghostPlayer;

    public ghostRecorder gRecorder;

    public int selectedCarNum = 0;
    public bool activateGhostCars;

    [Header("Wings")]
    public GameObject supraWing;
    public GameObject porscheWing;
    public GameObject chironWingOpen;
    public GameObject chironWingClosed;

    [Header("mainCanvas")]
    public GameObject mainCanvas;

    [Header("test")]
    public GameObject[] spawnPosition;
    public GameObject[] PlayerPrefab;

    [Header("NickName")]
    public GameObject MyNickname;
    private GameObject SelectedBanner;

    [Header("MusicPlayer")]
    public GameObject musicPlayerUI;
    public BGM bgmScript;
    public GameObject reduceVolumeBtn;
    public GameObject increaseVolumeBtn;
    public GameObject previousMusicBtn;
    public GameObject nextMusicBtn;
    public GameObject pauseMusicBtn;
    public GameObject playMusicBtn;
    public TextMeshProUGUI musicName;

    [Header("resultPanel")]
    public GameObject resultPanel;
    public GameObject winnerNickName;
    public GameObject winnerRecord;
    public GameObject winnerCarImg;
    public GameObject loserNickName;
    public GameObject loserRecord;
    public GameObject loserCarImg;
    public Sprite[] carImgArr;
    public TextMeshProUGUI winInWin;
    public TextMeshProUGUI loseInWin;
    public TextMeshProUGUI rateInWin;
    public TextMeshProUGUI winInLose;
    public TextMeshProUGUI loseInLose;
    public TextMeshProUGUI rateInLose;

    [Header("RPC Object")]
    public GameObject rpcOb;
    public bool readyFlag = false;

    private bool isStarted = false;
    private float carInfoPosition;

    public int curRoomUserCount = 0;

    public bool isOnline = true;
    public bool isAllPlayerLoaded = false;
    public bool isAllPlayersColorLoaded = false;

    private RectTransform timaAttackBanner, carInfoBanner;

    public string timerToSend = "";

    private bool countdownFlag = false;
    private void Awake () {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
        selectedCarNum = savedData.data.currentCar;
        savedData.data.ghostDatas.currentCar = selectedCarNum;
        activateGhostCars = savedData.data.activateGhost;
        //Debug.Log(activateGhostCars);
        
       // Debug.Log(savedData.data.ghostDatas.savedGhostRecordTime);
        savedData.data.ghostDatas.isGhost = true;
        savedData.data.ghostDatas.isPlay = true;
        //Instantiate(zz, savedData.data.ghostDatas.savedGhostPosition[0], Quaternion.identity);
        if (isOnline)
        {



            Vector3 spawnPos = spawnPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position;
            Quaternion spawnRot = spawnPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation;
            GameObject Player = PhotonNetwork.Instantiate(PlayerPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["selectedCar"]].name, spawnPos, spawnRot);
            if (Player.transform.GetChild(1).gameObject.GetPhotonView().IsMine)
            {
                Player.transform.Find("Main Camera").GetComponent<Camera>().enabled = true;
                Player.transform.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
                Player.transform.Find("Main Camera").GetComponent<BGM>().enabled = true;
                Player.transform.Find("Main Camera").GetComponent<AudioSource>().enabled = true;
                Player.transform.Find("Main Camera").transform.Find("soundBox").GetComponent<AudioSource>().enabled = true;
                Player.transform.Find("Main Camera").transform.Find("soundBox").GetComponent<startScript>().enabled = true;
                bgmScript = Player.transform.Find("Main Camera").GetComponent<BGM>();

                Player.transform.GetChild(1).gameObject.GetComponent<inputManager>().enabled = true;
                Player.transform.GetChild(1).gameObject.GetComponent<carEffects>().enabled = true;
                Player.transform.GetChild(1).gameObject.GetComponent<audio>().enabled = true;
                Player.transform.GetChild(1).gameObject.GetComponent<controller>().enabled = true;
                Player.transform.GetChild(1).transform.Find("CarEffectAudioSource").transform.Find("skid").gameObject.GetComponent<AudioSource>().enabled = true;
                Player.transform.GetChild(1).transform.Find("CarEffectAudioSource").transform.Find("collide").gameObject.GetComponent<AudioSource>().enabled = true;
                Player.transform.GetChild(1).transform.Find("CarEffectAudioSource").transform.Find("friction").gameObject.GetComponent<AudioSource>().enabled = true;
                Player.transform.GetChild(1).transform.Find("trigger").gameObject.SetActive(true);
                Player.transform.GetChild(1).transform.Find("minimapCam").GetComponent<Camera>().enabled = true;
                Player.transform.GetChild(1).transform.Find("PlayerUICanvas").gameObject.SetActive(true);

                GameObject[] gg = GameObject.FindGameObjectsWithTag("supraColor");
                for(int i =0; i < gg.Length; i++)
                {
                    Debug.Log(gg[i].name);
                }

                rpcOb = GameObject.FindGameObjectWithTag("CheckUsers");
                PhotonView pv = rpcOb.GetComponent<PhotonView>();
                pv.RPC("AddOrEjectWings", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber,
                    (int)PhotonNetwork.LocalPlayer.CustomProperties["selectedCar"], (float)PhotonNetwork.LocalPlayer.CustomProperties["wings"]);

                /*
                Color c1 = new Color((float)PhotonNetwork.PlayerList[0].CustomProperties["colorR"], (float)PhotonNetwork.PlayerList[0].CustomProperties["colorG"],
                    (float)PhotonNetwork.PlayerList[0].CustomProperties["colorB"]);

                Color c2 = new Color((float)PhotonNetwork.PlayerList[1].CustomProperties["colorR"], (float)PhotonNetwork.PlayerList[1].CustomProperties["colorG"],
                    (float)PhotonNetwork.PlayerList[1].CustomProperties["colorB"]);*/

                /*
                pv.RPC("SetColorToServer", RpcTarget.All, PhotonNetwork.PlayerList[0].ActorNumber, (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCar"],
                    (Color)PhotonNetwork.PlayerList[0].CustomProperties["color"], PhotonNetwork.PlayerList[1].ActorNumber, (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCar"],
                    (Color)PhotonNetwork.PlayerList[1].CustomProperties["color"]);*/
                /*---
                pv.RPC("SetColorToServer", RpcTarget.All, PhotonNetwork.PlayerList[0].ActorNumber, (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCar"],
                    c1, PhotonNetwork.PlayerList[1].ActorNumber, (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCar"],
                    c2);
                */

                /*
                if ((float)PhotonNetwork.LocalPlayer.CustomProperties["wings"] == 0)
                {
                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["selectedCar"] == 2)
                    {
                        Player.transform.GetChild(1).transform.Find("spoiler").gameObject.SetActive(false);
                        Player.transform.GetChild(1).transform.Find("closedSpoiler").gameObject.SetActive(true);
                    }
                    else
                    {
                        Player.transform.GetChild(1).transform.Find("spoiler").gameObject.SetActive(false);
                    }
                }
                else
                {
                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["selectedCar"] == 2)
                    {
                        Player.transform.GetChild(1).transform.Find("spoiler").gameObject.SetActive(true);
                        Player.transform.GetChild(1).transform.Find("closedSpoiler").gameObject.SetActive(false);
                    }
                    else
                    {
                        Player.transform.GetChild(1).transform.Find("spoiler").gameObject.SetActive(true);
                    }
                }
                */


                //curRoomUserCount++;

                //Player.transform.GetChild(1).gameObject.GetComponents<AudioSource>()[0].enabled = true;
                //Player.transform.GetChild(1).gameObject.GetComponents<AudioSource>()[1].enabled = true;
                //Player.transform.GetChild(1).gameObject.GetComponents<AudioSource>()[2].enabled = true;

                //Player.transform.GetChild(1).gameObject.GetComponent<controller>().enabled = true;
            }
        }




        //loadCar(selectedCarNum); // -----------------
        //SetWings(); //-------------------
        //setGhosts();
        RR = GameObject.FindGameObjectWithTag ("Player").GetComponent<controller> ();
        car = GameObject.FindGameObjectWithTag("Player");
        gRecorder = GameObject.FindGameObjectWithTag("Player").GetComponent<ghostRecorder>();
        
        //selectedCarNum = savedData
        selectedCarNum = savedData.data.currentCar;
        //Debug.Log(selectedCarNum);
        
        
    }

    private void Start()
    {
        startScript = GameObject.FindGameObjectWithTag("soundBox").GetComponent<startScript>();
        mainCanvas = GameObject.Find("PlayerUICanvas");
        needle = GameObject.Find("PlayerUICanvas").transform.Find("speedometer").transform.Find("needle").gameObject;
        startPosition = GameObject.Find("Lap Line");
        kph = GameObject.Find("PlayerUICanvas").transform.Find("speedometer").transform.Find("kph").gameObject.GetComponent<Text>();
        currentPosition = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").transform.Find("Time").transform.Find("Timer").gameObject.GetComponent<Text>();
        gearNum = GameObject.Find("PlayerUICanvas").transform.Find("speedometer").transform.Find("current gear").gameObject.GetComponent<Text>();
        timeLeftText = GameObject.Find("PlayerUICanvas").transform.Find("countdown").transform.Find("countdownText").GetComponent<Text>();
        RaceUI = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").gameObject;
        if (!isOnline)
        {
            RaceUI.transform.Find("Position").gameObject.SetActive(false);
        }
        firstPosition = RaceUI.transform.Find("Position").transform.Find("1st").gameObject;
        secondPosition = RaceUI.transform.Find("Position").transform.Find("2nd").gameObject;
        SpeedUI = GameObject.Find("PlayerUICanvas").transform.Find("speedometer").gameObject;
        //timeAttack = GameObject.Find("PlayerUICanvas").transform.Find("TimeAttack").gameObject;
        BestRecord = GameObject.Find("PlayerUICanvas").transform.Find("Finished").transform.Find("BestRecord").gameObject;
        BestRecordTime = GameObject.Find("PlayerUICanvas").transform.Find("Finished").transform.Find("BestRecordTime").gameObject.GetComponent<Text>();
        lpM = GameObject.Find("FinishLineTrigger").GetComponent<lapUIManager>();
        m_Text = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").transform.Find("Time").transform.Find("Timer").gameObject.GetComponent<Text>();
        RL1 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("RedLight1").gameObject;
        RL2 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("RedLight2").gameObject;
        RL3 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("RedLight3").gameObject;
        GL1 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("GreenLight1").gameObject;
        GL2 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("GreenLight2").gameObject;
        GL3 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("GreenLight3").gameObject;
        GL4 = GameObject.Find("PlayerUICanvas").transform.Find("signContainer").transform.Find("GreenLight4").gameObject;
        MyNickname = GameObject.Find("PlayerUICanvas").transform.Find("MyNickname").gameObject;
        
        // ---------------- music(BGM)
        musicPlayerUI = GameObject.Find("PlayerUICanvas").transform.Find("Music").gameObject;
        pauseMusicBtn = musicPlayerUI.transform.Find("Panel").transform.Find("pause").gameObject;
        pauseMusicBtn.GetComponent<Button>().onClick.AddListener(PauseMusic);
        playMusicBtn = musicPlayerUI.transform.Find("Panel").transform.Find("resume").gameObject;
        playMusicBtn.GetComponent<Button>().onClick.AddListener(ResumeMusic);
        nextMusicBtn = musicPlayerUI.transform.Find("Panel").transform.Find("next").gameObject;
        nextMusicBtn.GetComponent<Button>().onClick.AddListener(NextMusic);
        previousMusicBtn = musicPlayerUI.transform.Find("Panel").transform.Find("previous").gameObject;
        previousMusicBtn.GetComponent<Button>().onClick.AddListener(PreviousMusic);
        reduceVolumeBtn = musicPlayerUI.transform.Find("Panel").transform.Find("reduce").gameObject;
        reduceVolumeBtn.GetComponent<Button>().onClick.AddListener(ReduceBGMVolume);
        increaseVolumeBtn = musicPlayerUI.transform.Find("Panel").transform.Find("increase").gameObject;
        increaseVolumeBtn.GetComponent<Button>().onClick.AddListener(IncreaseBGMVolume);
        musicName = musicPlayerUI.transform.Find("Panel").transform.Find("MusicName").transform.Find("musicName").gameObject.GetComponent<TextMeshProUGUI>();

        // ---------------ResultPanel
        resultPanel = GameObject.Find("PlayerUICanvas").transform.Find("MultiRecordWindow").gameObject;
        winnerNickName = resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").transform.Find("nickName").gameObject;
        winnerRecord = resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").transform.Find("recordTime").gameObject;
        winnerCarImg = resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").transform.Find("carImage").gameObject;
        winInWin = resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").transform.Find("winInWin").gameObject.GetComponent<TextMeshProUGUI>();
        loseInWin = resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").transform.Find("loseInWin").gameObject.GetComponent<TextMeshProUGUI>();
        rateInWin = resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").transform.Find("rateInWin").gameObject.GetComponent<TextMeshProUGUI>();
        loserNickName = resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").transform.Find("nickName").gameObject;
        loserRecord = resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").transform.Find("recordTime").gameObject;
        loserCarImg = resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").transform.Find("carImage").gameObject;
        winInLose = resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").transform.Find("winInLose").gameObject.GetComponent<TextMeshProUGUI>();
        loseInLose = resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").transform.Find("loseInLose").gameObject.GetComponent<TextMeshProUGUI>();
        rateInLose = resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").transform.Find("rateInLose").gameObject.GetComponent<TextMeshProUGUI>();

        MyNickname.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;

        SpawnPoint = GameObject.Find("checkPoint");
        TimeAttackBanner = GameObject.Find("PlayerUICanvas").transform.Find("TimeAttackBanner").gameObject;
        SupraBanner = GameObject.Find("PlayerUICanvas").transform.Find("SupraBanner").gameObject;
        PorscheBanner = GameObject.Find("PlayerUICanvas").transform.Find("PorscheBanner").gameObject;
        ChironBanner = GameObject.Find("PlayerUICanvas").transform.Find("ChironBanner").gameObject;
        //setCameras();
        setBanner();
        moveBanner();
        RaceUI.SetActive(false);
        SpeedUI.SetActive(false);
        musicPlayerUI.SetActive(false);
        //StartCoroutine(timedLoop());
        freezePlayers();
    }

    private void FixedUpdate () {
        if (!isAllPlayerLoaded)
        {
            CheckPlayersLoad();
        }
        if(isOnline)
        {
            CheckPlayersColorLoad();
            if (isAllPlayerLoaded && isAllPlayersColorLoaded)
            {
                /*
                if (readyFlag)
                {
                    //SetCarColors();
                }*/
                kph.text = RR.KPH.ToString("0"); //숫자만
                updateNeedle();
                UpdateMusicName();

                if (!RR.photonView.IsMine)
                {
                    mainCanvas.SetActive(false);
                }

                if (!isStarted)
                {
                    coundDownTimer();
                    moveBanner();
                }

                if (m_IsPlaying)
                {
                    m_Timer = StockwatchTimer();
                }

                if (m_Text && m_IsPlaying)
                    m_Text.text = m_Timer;

                
                if (Finished)
                {
                    finishedTimer();
                }
            }
        }
        

    }

    public void CheckPlayersLoad()
    {
        if(PhotonNetwork.PlayerList.Length == GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<RPC_CheckLoad>().loadedUser)
        {
            SetCarColors();
            isAllPlayerLoaded = true;
            readyFlag = true;
        }
    }

    public void CheckPlayersColorLoad()
    {
        if (PhotonNetwork.PlayerList.Length == GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<RPC_CheckLoad>().colorFinishedUser)
        {
            isAllPlayersColorLoaded = true;
        }

    }

    public void updateNeedle () {
        desiredPosition = startPosiziton - endPosition;
        float temp = RR.engineRPM / 10000;
        needle.transform.eulerAngles = new Vector3 (0, 0, (startPosiziton - temp * desiredPosition));

    }

    public void changeGear () {
        gearNum.text = (!RR.reverse) ? (RR.gearNum + 1).ToString () : "R";

    }
    
    private IEnumerator timedLoop () {
        while (true) {
            yield return new WaitForSeconds (.7f);
        }
    }

    private void coundDownTimer(){
        if(timeLeft <= -5) return;
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)unfreezePlayers();
        else freezePlayers();
        
        if (timeLeft > 1) { timeLeftText.text = timeLeft.ToString("0"); timeLeftFloat = float.Parse(timeLeftText.text); }
        else timeLeftText.text = "";
        
        if (timeLeftFloat == 4.0 && count4 == 0) { 
            startScript.countSoundPlay(); count4 = 1; 
            //timeAttack.SetActive(false); 
            RL1.SetActive(true);
            TimeAttackBanner.SetActive(false);
            SelectedBanner.SetActive(false);
        }
        else if (timeLeftFloat == 3.0 && count3 == 0) { startScript.countSoundPlay(); count3 = 1; RL2.SetActive(true); }
        else if (timeLeftFloat == 2.0 && count2 == 0) { startScript.countSoundPlay(); count2 = 1; RL3.SetActive(true); }
        else if (timeLeftFloat == 1.0 && count1 == 0) { 
            startScript.startSoundPlay(); count1 = 1;
            RL1.SetActive(false);
            RL2.SetActive(false);
            RL3.SetActive(false);
            GL1.SetActive(true);
            GL2.SetActive(true);
            GL3.SetActive(true);
            GL4.SetActive(true);
            
        }

    }
    
    private void freezePlayers(){
        if(countdownFlag) return;
        car.GetComponent<Rigidbody>().isKinematic = true;
        countdownFlag = true;
        
    }

    public void freezeFinishedPlayers()
    {
        car.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void unfreezePlayers(){
        if(!countdownFlag) return;
        car.GetComponent<Rigidbody>().isKinematic = false;
        m_IsPlaying = true;
        countdownFlag = false;
        RaceUI.SetActive(true);
        SpeedUI.SetActive(true);
        musicPlayerUI.SetActive(true);
        GL1.SetActive(false);
        GL2.SetActive(false);
        GL3.SetActive(false);
        GL4.SetActive(false);

        isStarted = true;
        ghostPlayer.StartGhost();
        gRecorder.StartRecording();
        

    }

   // public void loadAwakeScene(){
       // SceneManager.LoadScene("awakeScene");
    //}

    public void finishGame()
    {
        RR.isFinished = true;
        Finished = true;
        savedData.data.ghostDatas.isGhost = false;
        savedData.data.ghostDatas.isPlay = false;
    }

    public void FirstPlayerEntered(int actorNumber, string recordTime)
    {
        finishGame();
        winnerNickName.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[actorNumber - 1].NickName;
        winnerRecord.GetComponent<TextMeshProUGUI>().text = recordTime;
        winnerCarImg.GetComponent<Image>().sprite = carImgArr[(int)PhotonNetwork.PlayerList[actorNumber - 1].CustomProperties["selectedCar"]];
        winInWin.text = $"{(int)PhotonNetwork.PlayerList[actorNumber - 1].CustomProperties["win"] + 1}승";
        loseInWin.text = $"{(int)PhotonNetwork.PlayerList[actorNumber - 1].CustomProperties["lose"]}패";

        int winrateInWin;
        int winrateInLose;

        
       
        
        winrateInWin = ((int)PhotonNetwork.PlayerList[actorNumber - 1].CustomProperties["win"] + 1)
            / ((int)PhotonNetwork.PlayerList[actorNumber - 1].CustomProperties["win"] + (int)PhotonNetwork.PlayerList[actorNumber - 1].CustomProperties["lose"] + 1);
        
        
        rateInWin.text = $"승률{winrateInWin * 100}%";

        int act = 0;
        if (actorNumber == 1) act = 1;

        loserNickName.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[act].NickName;
        loserRecord.GetComponent<TextMeshProUGUI>().text = "DNF";
        loserCarImg.GetComponent<Image>().sprite = carImgArr[(int)PhotonNetwork.PlayerList[act].CustomProperties["selectedCar"]];
        winInLose.text = $"{(int)PhotonNetwork.PlayerList[act].CustomProperties["win"]}승";
        loseInLose.text = $"{(int)PhotonNetwork.PlayerList[act].CustomProperties["lose"] + 1}패";

        
        
        winrateInLose = (int)PhotonNetwork.PlayerList[act].CustomProperties["win"]
            / ((int)PhotonNetwork.PlayerList[act].CustomProperties["win"] + (int)PhotonNetwork.PlayerList[act].CustomProperties["lose"] + 1);
        

        
        rateInLose.text = $"승률{winrateInLose * 100}%";


        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            resultPanel.transform.Find("recordPanel").transform.Find("winnerPanel").gameObject.GetComponent<Outline>().effectColor = Color.green;
            savedData.data.userWin++;
        }
        else
        {
            resultPanel.transform.Find("recordPanel").transform.Find("loserPanel").gameObject.GetComponent<Outline>().effectColor = Color.green;
            savedData.data.userLose++;
        }
    }

    private void finishedTimer()
    {
        if (isOnline)
        {
            RaceUI.SetActive(false);
            SpeedUI.SetActive(false);
            finishCounter += Time.deltaTime;
            

            if (finishCounter >= 1 && finishSound == 0)
            {
                startScript.finishedSoundPlay();
                finishSound = 1;
                //BestRecord.SetActive(true);
                resultPanel.SetActive(true);
            }
            if (finishCounter >= 2 && recordSound == 0)
            {
                /*
                TimeSpan timespan2 = TimeSpan.FromSeconds(lpM.TotalBest);
                string timer2 = string.Format("{1:00}:{2:00}.{3:00}",
                    timespan2.Hours, timespan2.Minutes, timespan2.Seconds, timespan2.Milliseconds);
                */
                startScript.recordSoundPlay();
                recordSound = 1;
                //BestRecordTime.text = timer2;

                //ghostPlayer.DestroyGhostCar();
                if (!isOnline)
                {
                    savedData.data.ghostDatas.currentClearedTime = m_TotalSeconds;
                    CompareGhostRecord();
                }
            }

            if (finishCounter >= 10)
            {
                if (isOnline)
                {
                    PhotonNetwork.LeaveRoom();
                }
                loadingSceneController.LoadScene("GarageScene");
            }
        }
        else
        {


            RaceUI.SetActive(false);
            SpeedUI.SetActive(false);
            finishCounter += Time.deltaTime;

            if (finishCounter >= 1 && finishSound == 0)
            {
                startScript.finishedSoundPlay();
                finishSound = 1;
                BestRecord.SetActive(true);
            }
            if (finishCounter >= 2 && recordSound == 0)
            {
                TimeSpan timespan2 = TimeSpan.FromSeconds(lpM.TotalBest);
                string timer2 = string.Format("{1:00}:{2:00}.{3:00}",
                    timespan2.Hours, timespan2.Minutes, timespan2.Seconds, timespan2.Milliseconds);

                startScript.recordSoundPlay();
                recordSound = 1;
                BestRecordTime.text = timer2;

                ghostPlayer.DestroyGhostCar();
                if (!isOnline)
                {
                    savedData.data.ghostDatas.currentClearedTime = m_TotalSeconds;
                    CompareGhostRecord();
                }
            }

            if (finishCounter >= 6)
            {
                if (isOnline)
                {
                    PhotonNetwork.LeaveRoom();
                }
                loadingSceneController.LoadScene("GarageScene");
            }
        }

    }
    public void testFinish()
    {
        lpM.TestFinish();
    }
    public void resetCar()
    {
        
        RR.resetCar();
        
    }
    /*
    public void setCameras()
    {
        if(RR.photonView.IsMine)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            leftCam = GameObject.FindGameObjectWithTag("leftCam").GetComponent<Camera>();
            frontCam = GameObject.FindGameObjectWithTag("frontCam").GetComponent<Camera>();
            rightCam = GameObject.FindGameObjectWithTag("rightCam").GetComponent<Camera>();

            leftCam.enabled = false;
            frontCam.enabled = false;
            rightCam.enabled = false;
        }
        /*
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        leftCam = GameObject.FindGameObjectWithTag("leftCam").GetComponent<Camera>();
        frontCam = GameObject.FindGameObjectWithTag("frontCam").GetComponent<Camera>();
        rightCam = GameObject.FindGameObjectWithTag("rightCam").GetComponent<Camera>();

        leftCam.enabled = false;
        frontCam.enabled = false;
        rightCam.enabled = false;*/

        //leftCam.GetComponent<Camera>().enabled = true;
        /*Cameras[0] = mainCamera;
        Cameras[1] = leftCam;
        Cameras[2] = frontCam;
        Cameras[3] = rightCam;*/


    //}*/
    /*
    public void cameraChange(int num)
    {
        /*
        for (int i=0; i<Cameras.Length; i++)
        {
            if ( num == i)
            {
                Cameras[num].enabled = true;
            }
            else
            {
                Cameras[i].enabled = false;
            }
        }*//*
        if ( num == 0)
        {
            mainCamera.enabled = true;
            leftCam.enabled = false;    
            frontCam.enabled = false;   
            rightCam.enabled = false;   
        }
        else if (num == 1)
        {
            mainCamera.enabled = false;
            leftCam.enabled = true;
            frontCam.enabled = false;
            rightCam.enabled = false;
        }
        else if (num == 2)
        {
            mainCamera.enabled = false;
            leftCam.enabled = false;
            frontCam.enabled = true;
            rightCam.enabled = false;
        }
        else if (num == 3)
        {
            mainCamera.enabled = false;
            leftCam.enabled = false;
            frontCam.enabled = false;
            rightCam.enabled = true;
        }
    }*/
    public void setBanner()
    {
        TimeAttackBanner.SetActive(true);
        timaAttackBanner = TimeAttackBanner.GetComponent<RectTransform>();
        if( selectedCarNum == 0)
        {
            SupraBanner.gameObject.SetActive(true);
            SelectedBanner = SupraBanner;
            carInfoBanner = SelectedBanner.GetComponent<RectTransform>();
        }
        else if (selectedCarNum == 1)
        {
            PorscheBanner.gameObject.SetActive(true);
            SelectedBanner = PorscheBanner;
            carInfoBanner = SelectedBanner.GetComponent<RectTransform>();
        }
        else if (selectedCarNum == 2)
        {
            ChironBanner.gameObject.SetActive(true);
            SelectedBanner = ChironBanner;
            carInfoBanner = SelectedBanner.GetComponent<RectTransform>();
        }
        //carInfoBanner = SupraBanner.GetComponent<RectTransform>();
    }

    public void moveBanner()
    {
        //SupraBanner.transform.Translate (Vector3.)
       // SupraBanner.transform.position = new Vector3(500, 0, 0);
        //RectTransform su = SupraBanner.GetComponent<RectTransform>();
        //su.anchoredPosition = new Vector2(1300,0);
        //RectTransform ta = TimeAttackBanner.GetComponent<RectTransform>();
        // ta.anchoredPosition = new Vector2(-1300,0);
        if (timeLeft > 8.2 && timeLeft <= 8.8)
        {
            carInfoPosition = carInfoBanner.anchoredPosition.x - 45f;
        }
        else if (timeLeft > 5.3 && timeLeft <= 8.2)
        {
            carInfoPosition = carInfoBanner.anchoredPosition.x - 4f;
        }
        else if (timeLeft > 5 && timeLeft <= 5.3)
        {
            carInfoPosition = carInfoBanner.anchoredPosition.x - 70f;
        }
        else
        {
            carInfoPosition = carInfoBanner.anchoredPosition.x;
        }
        carInfoBanner.anchoredPosition = new Vector2(carInfoPosition,0);
        timaAttackBanner.anchoredPosition = new Vector2(-carInfoPosition, 344);

    }

    public void loadCar(int n)
    {
        if( n == 0 )
        {
            Porsche.SetActive(false);
            Chiron.SetActive(false);
            Supra.SetActive(true);
            Destroy(Porsche);
            Destroy(Chiron);
        }
        else if (n == 1 )
        {
            Chiron.SetActive(false);
            Supra.SetActive(false);
            Porsche.SetActive(true);
            Destroy(Chiron);
            Destroy(Supra);
        }
        else if ( n == 2 )
        {
            Supra.SetActive(false);
            Porsche.SetActive(false);
            Chiron.SetActive(true);
            Destroy(Supra);
            Destroy(Porsche);
        }
    }
    public void setGhosts()
    {
        if(activateGhostCars)
        {
            if (selectedCarNum == 0)
            {
                PorscheGhost.SetActive(false);
                ChironGhost.SetActive(false);
                SupraGhost.SetActive(true);
                Destroy(ChironGhost);
                Destroy(PorscheGhost);
            }
            else if (selectedCarNum == 1)
            {
                ChironGhost.SetActive(false);
                SupraGhost.SetActive(false);
                PorscheGhost.SetActive(true);
                Destroy(ChironGhost);
                Destroy(SupraGhost);
            }
            else if (selectedCarNum == 2)
            {
                SupraGhost.SetActive(false);
                PorscheGhost.SetActive(false);
                ChironGhost.SetActive(true);
                Destroy(SupraGhost);
                Destroy(PorscheGhost);
            }
        }
    }
    public void CompareGhostRecord()
    {
        if((savedData.data.ghostDatas.savedGhostRecordTime == -1f) || (savedData.data.ghostDatas.savedGhostRecordTime > m_TotalSeconds))
        {
            savedData.data.ghostDatas.UpdateSavedGhostDatas();
        }
    }

    public void SetWings()
    {
        if (savedData.data.savedDownforce != 0)
        {
            supraWing.SetActive(true);
            porscheWing.SetActive(true);
            chironWingOpen.SetActive(true);
            chironWingClosed.SetActive(false);
        }
        else
        {
            supraWing.SetActive(false);
            porscheWing.SetActive(false);
            chironWingOpen.SetActive(false);
            chironWingClosed.SetActive(true);
        }
    }

    public void UpdateLap()
    {
        currentLap++;
    }

    public int CheckLap()
    {
        if(currentLap == 1)
        {
            return 1;
        }
        else if (currentLap == 2)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    public void isFirstPositionCheck(bool isFirst)
    {
        if(isFirst)
        {
            firstPosition.SetActive(true);
            secondPosition.SetActive(false);
            Debug.Log(isFirst);
        }
        else
        {
            firstPosition.SetActive(false);
            secondPosition.SetActive(true);
            Debug.Log(isFirst);
        }
    }

    public void CurrentPosition(int masterScore, int otherScore)
    {
        if(masterScore > otherScore)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                firstPosition.SetActive(true);
                secondPosition.SetActive(false);
            }
            else
            {
                firstPosition.SetActive(false);
                secondPosition.SetActive(true);
            }
            
        }
        else if(masterScore < otherScore)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                firstPosition.SetActive(false);
                secondPosition.SetActive(true);
            }
            else
            {
                firstPosition.SetActive(true);
                secondPosition.SetActive(false);
            }
        }
    }
    string StockwatchTimer()
    {


        //if (PhotonNetwork.IsMasterClient || isOnline == false)
        if (true)
        {
            m_TotalSeconds += Time.deltaTime;
            TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
            string timer = string.Format("{1:00}:{2:00}.{3:000}",
                timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
            timerToSend = timer;

            /*
            if (isOnline)
            {
                if (photonView.IsMine)
                {
                    PhotonView pv = GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<PhotonView>();
                    pv.RPC("SetRPC_Timer", RpcTarget.All, timer);
                }
                //GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<RPC_CheckLoad>().SetRPC_Timer(timer);
                return GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<RPC_CheckLoad>().GetRPC_Timer();
            }
            else
            {
                return timer;
            }
            */
            return timerToSend;

        }
        else
        {
            //return GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<RPC_CheckLoad>().GetRPC_Timer();
            return timerToSend;
        }
        //return timer;
    }

    public void SetCarColors()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            rpcOb = GameObject.FindGameObjectWithTag("CheckUsers");
            PhotonView pv = rpcOb.GetComponent<PhotonView>();
            Color c1 = new Color((float)PhotonNetwork.PlayerList[0].CustomProperties["colorR"], (float)PhotonNetwork.PlayerList[0].CustomProperties["colorG"],
                        (float)PhotonNetwork.PlayerList[0].CustomProperties["colorB"]);

            Color c2 = new Color((float)PhotonNetwork.PlayerList[1].CustomProperties["colorR"], (float)PhotonNetwork.PlayerList[1].CustomProperties["colorG"],
                (float)PhotonNetwork.PlayerList[1].CustomProperties["colorB"]);

            /*
            pv.RPC("SetColorToServer", RpcTarget.All, PhotonNetwork.PlayerList[0].ActorNumber, (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCar"],
                (Color)PhotonNetwork.PlayerList[0].CustomProperties["color"], PhotonNetwork.PlayerList[1].ActorNumber, (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCar"],
                (Color)PhotonNetwork.PlayerList[1].CustomProperties["color"]);*/

            Debug.Log((float)PhotonNetwork.PlayerList[0].CustomProperties["colorR"] +"---"+ (float)PhotonNetwork.PlayerList[0].CustomProperties["colorG"]+"---"+
                        (float)PhotonNetwork.PlayerList[0].CustomProperties["colorB"]);
            pv.RPC("SetColorToServer", RpcTarget.All, PhotonNetwork.PlayerList[0].ActorNumber, (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCar"],
                (float)PhotonNetwork.PlayerList[0].CustomProperties["colorR"], (float)PhotonNetwork.PlayerList[0].CustomProperties["colorG"],
                        (float)PhotonNetwork.PlayerList[0].CustomProperties["colorB"], 
                PhotonNetwork.PlayerList[1].ActorNumber, (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCar"], 
                (float)PhotonNetwork.PlayerList[1].CustomProperties["colorR"], (float)PhotonNetwork.PlayerList[1].CustomProperties["colorG"],
                        (float)PhotonNetwork.PlayerList[1].CustomProperties["colorB"]);
        }
        //readyFlag = false;
        PhotonView pv2 = rpcOb.GetComponent<PhotonView>();
        pv2.RPC("ColorLoaded", RpcTarget.All);
    }

    public void ReduceBGMVolume()
    {
        bgmScript.ReduceMusicVolume();
    }

    public void IncreaseBGMVolume()
    {
        bgmScript.IncreaseMusicVolume();
    }

    public void PreviousMusic()
    {
        bgmScript.PreviousMusicPlay();
        UpdateMusicName();
    }

    public void NextMusic()
    {
        bgmScript.NextMusicPlay();
        UpdateMusicName();
    }

    public void PauseMusic()
    {
        bgmScript.PauseMusic();
        playMusicBtn.SetActive(true);
        pauseMusicBtn.SetActive(false);
        UpdateMusicName();
    }

    public void ResumeMusic()
    {
        bgmScript.ResumeMusic();
        pauseMusicBtn.SetActive(true);
        playMusicBtn.SetActive(false);
        UpdateMusicName();
    }

    public void UpdateMusicName()
    {
        musicName.text = bgmScript.ReturnMusicName();
    }

    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext((float)m_TotalSeconds);
            stream.SendNext((string)timerToSend);
        }
        else
        {
            m_TotalSeconds = (float)stream.ReceiveNext();
            timerToSend = (string)stream.ReceiveNext();
        }
    }*/
}