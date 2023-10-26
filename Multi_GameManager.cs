using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
//using UnityEditor.Rendering.LookDev;

public class Multi_GameManager : MonoBehaviour {
    public GameObject car;
    //public vehicleList list;
    public controller RR;
    public GameObject neeedle;
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
    public GameObject timeAttack;
    public GameObject BestRecord;
    public Text BestRecordTime;

    public lapUIManager lpM;

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

    private GameObject SelectedBanner;

    private bool isStarted = false;
    private float carInfoPosition;


    private RectTransform timaAttackBanner, carInfoBanner;

    private bool countdownFlag = false;
    private void Awake () {
        /*selectedCarNum = savedData.data.currentCar;
        savedData.data.ghostDatas.currentCar = selectedCarNum;
        activateGhostCars = savedData.data.activateGhost;
        //Debug.Log(activateGhostCars);
       
        
        Debug.Log(savedData.data.ghostDatas.savedGhostRecordTime);*/
        /*savedData.data.ghostDatas.isGhost = true;
        savedData.data.ghostDatas.isPlay = true;*/
        //Instantiate(zz, savedData.data.ghostDatas.savedGhostPosition[0], Quaternion.identity);
        selectedCarNum= 0;
        loadCar(selectedCarNum);
        //SetWings();
        //setGhosts();
        RR = GameObject.FindGameObjectWithTag ("Player").GetComponent<controller> ();
        car = GameObject.FindGameObjectWithTag("Player");
        gRecorder = GameObject.FindGameObjectWithTag("Player").GetComponent<ghostRecorder>();
        //moveBanner();
        //selectedCarNum = savedData
        //selectedCarNum = savedData.data.currentCar;
        //Debug.Log(selectedCarNum);
        setCameras();
        setBanner();
        RaceUI.SetActive(false);
        SpeedUI.SetActive(false);
        StartCoroutine (timedLoop ());
    }

    private void FixedUpdate () {
        kph.text = RR.KPH.ToString ("0"); //숫자만
        updateNeedle ();

        if (!isStarted)
        {
            coundDownTimer();
            moveBanner();
        }

        if (m_IsPlaying)
        {
            m_Timer = StockwatchTimer();
        }

        if (m_Text&& m_IsPlaying)
            m_Text.text = m_Timer;

        if (Finished)
        {
            finishedTimer();
        }

    }

    public void updateNeedle () {
        desiredPosition = startPosiziton - endPosition;
        float temp = RR.engineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3 (0, 0, (startPosiziton - temp * desiredPosition));

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
            timeAttack.SetActive(false); 
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
        /*savedData.data.ghostDatas.isGhost = false;
        savedData.data.ghostDatas.isPlay = false;*/
    }

    private void finishedTimer()
    {
        RaceUI.SetActive(false);
        SpeedUI.SetActive(false);
        finishCounter += Time.deltaTime;

        if (finishCounter >= 1 && finishSound==0)
        {
            startScript.finishedSoundPlay();
            finishSound = 1;
            BestRecord.SetActive(true);
        }
        if(finishCounter >= 2 && recordSound==0)
        {
            TimeSpan timespan2 = TimeSpan.FromSeconds(lpM.TotalBest);
            string timer2 = string.Format("{1:00}:{2:00}.{3:00}",
                timespan2.Hours, timespan2.Minutes, timespan2.Seconds, timespan2.Milliseconds);

            startScript.recordSoundPlay();
            recordSound = 1;
            BestRecordTime.text = timer2;

            ghostPlayer.DestroyGhostCar();
            //savedData.data.ghostDatas.currentClearedTime = m_TotalSeconds;
            //CompareGhostRecord();
        }

        if(finishCounter >= 6)
        {
            loadingSceneController.LoadScene("GarageScene");
        }

    }

    public void resetCar()
    {
        
        RR.resetCar();
        
    }

    public void setCameras()
    {
        leftCam = GameObject.FindWithTag("leftCam").GetComponent<Camera>();
        frontCam = GameObject.FindWithTag("frontCam").GetComponent<Camera>();
        rightCam = GameObject.FindWithTag("rightCam").GetComponent<Camera>();
        //leftCam.GetComponent<Camera>().enabled = true;
        /*Cameras[0] = mainCamera;
        Cameras[1] = leftCam;
        Cameras[2] = frontCam;
        Cameras[3] = rightCam;*/


    }

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
        }*/
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
    }
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
    /*
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
    }*/
    string StockwatchTimer()
    {
        m_TotalSeconds += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{1:00}:{2:00}.{3:000}",
            timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);

        return timer;
    }
}