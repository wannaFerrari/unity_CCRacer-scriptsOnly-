using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login_BGM : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip loginBGM;
    public AudioSource ASource;

   
    void Update()
    {
        if (!ASource.isPlaying)
            BgmPlay();
    }

    void BgmPlay()
    {

        ASource.clip = loginBGM;
        ASource.Play();
    }
}
