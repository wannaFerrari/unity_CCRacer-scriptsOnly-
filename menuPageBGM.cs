using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuPageBGM : MonoBehaviour
{
    //public AudioClip[] Music = new AudioClip[3]; // »ç¿ëÇÒ BGM
    public AudioClip menuBGM;
    AudioSource ASource;

    void Awake()
    {
        ASource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!ASource.isPlaying)
            BgmPlay();
    }

    void BgmPlay()
    {
        
        ASource.clip = menuBGM;
        ASource.Play();
    }
}
