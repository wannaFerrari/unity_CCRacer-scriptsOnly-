using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostRecorder : MonoBehaviour
{
    public ghost ghost;
    private float timer;
    private float timeValue;
    private bool start = false;

    // Start is called before the first frame update
    private void Awake()
    {
        ghost = savedData.data.ghostDatas;

        if (ghost.isPlay)
        {
            ghost.ResetData();
            ghost.currentCar = savedData.data.currentCar;
            timeValue = 0;
            timer = 0;
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            timer += Time.unscaledDeltaTime;
            timeValue += Time.unscaledDeltaTime;

            if (ghost.isPlay & timer >= 1 / ghost.recordFrequancy)
            {
                ghost.timeStamp.Add(timeValue);
                ghost.position.Add(this.transform.position);
                ghost.rotation.Add(this.transform.eulerAngles);

                timer = 0;
            }
        }
    }
    
    public void StartRecording()
    {
        start = true;
    }
}
