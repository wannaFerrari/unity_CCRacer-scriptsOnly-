using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lapUIManager : MonoBehaviour
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



    // Start is called before the first frame update
    /*
    void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        if (GManager.m_TotalSeconds >= 4)
        {
            currentLapRecord.text = "";
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("finishTrigger"))
        {
            if (laps == 0)
            {
                laps++;
                Lap.text = "Lap: 1/2";
            }
            else if(laps == 2)
            {
                SecondLapSeconds= GManager.m_TotalSeconds;
                if (FirstLapSeconds >= SecondLapSeconds)
                {
                    TotalBest = SecondLapSeconds;
                }
                else
                {
                    TotalBest = FirstLapSeconds;
                }
                    
                GManager.finishGame();
                
            }
            else
            {
                laps++;
                Lap.text = "Lap: 2/2";
                //Record.text = GManager.m_Timer;
                FirstLapSeconds = GManager.m_TotalSeconds;
                Record.text = currentTimer.text;
                currentLapRecord.text = Record.text;
                GManager.m_TotalSeconds = 0;
            }
        }  
    }
}
