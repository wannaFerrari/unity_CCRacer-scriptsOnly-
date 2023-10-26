using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class lapUIManager : MonoBehaviourPun
{

    public Text Lap;
    public Text Record;
    public Text currentTimer;
    public Text currentLapRecord;
    private int laps = 0;
    public GameManager GManager;
    private float FirstLapSeconds;
    private float SecondLapSeconds;
    public float TotalBest;
    public GameObject rpcObj;



    // Start is called before the first frame update
    
    void Start()
    {
        Lap = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").transform.Find("LapUI").transform.Find("Lap").gameObject.GetComponent<Text>();
        Record = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").transform.Find("LapUI").transform.Find("record").gameObject.GetComponent<Text>();
        currentTimer = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").transform.Find("Time").transform.Find("Timer").gameObject.GetComponent<Text>();
        currentLapRecord = GameObject.Find("PlayerUICanvas").transform.Find("Lap and Time").transform.Find("Time").transform.Find("CurrentLapRecord").gameObject.GetComponent<Text>();
        GManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        rpcObj = GameObject.FindGameObjectWithTag("CheckUsers");

    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.m_TotalSeconds >= FirstLapSeconds + 4)
        {
            currentLapRecord.text = "";
        }
    }

    public void TestFinish()
    {
        TotalBest = GManager.m_TotalSeconds;
        TimeSpan timespan2 = TimeSpan.FromSeconds(TotalBest);
        string timer2 = string.Format("{1:00}:{2:00}.{3:00}",
            timespan2.Hours, timespan2.Minutes, timespan2.Seconds, timespan2.Milliseconds);
        PhotonView pv = rpcObj.GetComponent<PhotonView>();
        pv.RPC("FirstPlayerEntered", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, "11:22:333");
        // GManager.finishGame();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (GManager.isOnline)
        {
            if (col.gameObject.CompareTag("finishTrigger") && col.gameObject.transform.parent.GetComponent<PhotonView>().IsMine)
            {
                if (laps == 0)
                {
                    laps++;
                    Lap.text = "Lap: 1/2";
                    GManager.UpdateLap();
                }
                else if (laps == 2)
                {
                    /*
                    SecondLapSeconds= GManager.m_TotalSeconds;

                    if (FirstLapSeconds >= SecondLapSeconds)
                    {
                        TotalBest = SecondLapSeconds;
                    }
                    else
                    {
                        TotalBest = FirstLapSeconds;
                    }*/
                    if (col.gameObject.transform.parent.GetComponent<controller>().ReturnIsReadyToNextLap())
                    {
                        GManager.UpdateLap();
                        TotalBest = GManager.m_TotalSeconds;
                        TimeSpan timespan2 = TimeSpan.FromSeconds(TotalBest);
                        string timer2 = string.Format("{1:00}:{2:00}.{3:00}",
                            timespan2.Hours, timespan2.Minutes, timespan2.Seconds, timespan2.Milliseconds);
                        PhotonView pv = rpcObj.GetComponent<PhotonView>();
                        pv.RPC("FirstPlayerEntered", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, timer2);
                       // GManager.finishGame();
                    }

                }
                else
                {
                    if (col.gameObject.transform.parent.GetComponent<controller>().ReturnIsReadyToNextLap())
                    {
                        laps++;
                        Lap.text = "Lap: 2/2";
                        GManager.UpdateLap();
                        //Record.text = GManager.m_Timer;
                        FirstLapSeconds = GManager.m_TotalSeconds;
                        Record.text = currentTimer.text;
                        currentLapRecord.text = Record.text;
                        //GManager.m_TotalSeconds = 0;
                    }
                }
            }
        }
        else
        {

            if (col.gameObject.CompareTag("finishTrigger") )
            {
                if (laps == 0)
                {
                    laps++;
                    Lap.text = "Lap: 1/2";
                    GManager.UpdateLap();
                }
                else if (laps == 2)
                {
                    /*
                    SecondLapSeconds= GManager.m_TotalSeconds;

                    if (FirstLapSeconds >= SecondLapSeconds)
                    {
                        TotalBest = SecondLapSeconds;
                    }
                    else
                    {
                        TotalBest = FirstLapSeconds;
                    }*/
                    if (col.gameObject.transform.parent.GetComponent<controller>().ReturnIsReadyToNextLap())
                    {
                        GManager.UpdateLap();
                        TotalBest = GManager.m_TotalSeconds;

                        GManager.finishGame();
                    }

                }
                else
                {
                    if (col.gameObject.transform.parent.GetComponent<controller>().ReturnIsReadyToNextLap())
                    {
                        laps++;
                        Lap.text = "Lap: 2/2";
                        GManager.UpdateLap();
                        //Record.text = GManager.m_Timer;
                        FirstLapSeconds = GManager.m_TotalSeconds;
                        Record.text = currentTimer.text;
                        currentLapRecord.text = Record.text;
                        //GManager.m_TotalSeconds = 0;
                    }
                }
            }
        }
    }
    
}
