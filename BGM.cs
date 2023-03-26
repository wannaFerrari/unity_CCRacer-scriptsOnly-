using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{

    public AudioClip[] Music = new AudioClip[3]; // ����� BGM
   // public AudioClip BGM1;
    AudioSource AS;

    void Awake()
    {
        AS = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!AS.isPlaying)
            RandomPlay();
    }

    void RandomPlay()
    {
         AS.clip = Music[Random.Range(0, Music.Length)];
        //AS.clip = BGM1;
        AS.Play();
    }
}