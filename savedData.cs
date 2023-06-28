using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savedData : MonoBehaviour
{
    public static savedData data;

    public int currentCar = 0;
    public bool activateGhost;
    public bool playerIsPlaying = true;
    public ghost ghostDatas;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(data == null)
        {
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }
}
