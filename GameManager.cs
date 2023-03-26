using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
    public GameObject car;
    //public vehicleList list;
    public controller RR;
    public GameObject neeedle;
    public GameObject startPosition;
    public Text kph;
    public Text currentPosition;
    public Text gearNum;
    private float startPosiziton = 32f, endPosition = -211f;
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
    public float timeLeft = 7;
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
    public Camera[] Cameras;

    [Header("ReSpawn")]
    public GameObject SpawnPoint;
    private bool countdownFlag = false;
    private void Awake () {

        RR = GameObject.FindGameObjectWithTag ("Player").GetComponent<controller> ();
        RaceUI.SetActive(false);
        SpeedUI.SetActive(false);
        StartCoroutine (timedLoop ());
    }

    private void FixedUpdate () {
        kph.text = RR.KPH.ToString ("0"); //숫자만
        updateNeedle ();
        coundDownTimer();

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
        
        if (timeLeftFloat == 4.0 && count4 == 0) { startScript.countSoundPlay(); count4 = 1; timeAttack.SetActive(false); RL1.SetActive(true); }
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

    }

    public void loadAwakeScene(){
        SceneManager.LoadScene("awakeScene");
    }

    public void finishGame()
    {
        RR.isFinished = true;
        Finished = true;
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

    public void cameraChange(int num)
    {
        for(int i = 0; i < Cameras.Length; i++)
        {
            if(i == num)
            {
                Cameras[num].enabled = true;
            }
            else
            {
                Cameras[i].enabled = false;
            }
        }
    }

    string StockwatchTimer()
    {
        m_TotalSeconds += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{1:00}:{2:00}.{3:000}",
            timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);

        return timer;
    }
}