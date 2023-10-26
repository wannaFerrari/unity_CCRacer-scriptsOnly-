using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{

    public AudioClip[] Music = new AudioClip[3]; // »ç¿ëÇÒ BGM
   // public AudioClip BGM1;
    AudioSource AS;
    public int currentMusicNum = -1;
    public bool isPaused = false;

    void Start()
    {
        AS = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!AS.isPlaying && !isPaused)
            RandomPlay();
    }

    void RandomPlay()
    {
        if (currentMusicNum == -1)
        {
            currentMusicNum = Random.Range(0, Music.Length);
            //AS.clip = Music[Random.Range(0, Music.Length)];
            AS.clip = Music[currentMusicNum];
            AS.Play();
        }
        else
        {
            NextMusicPlay();
        }
    }

    public void NextMusicPlay()
    {
        if (currentMusicNum == Music.Length - 1)
        {
            currentMusicNum = 0;
        }
        else
        {
            currentMusicNum++;
        }
  
        AS.clip = Music[currentMusicNum];
        AS.Play();
    }

    public void PreviousMusicPlay()
    {
        if(currentMusicNum == 0)
        {
            currentMusicNum = Music.Length - 1;
        }
        else
        {
            currentMusicNum--;
        }
        
        AS.clip = Music[currentMusicNum];
        AS.Play();
    }

    public void PauseMusic()
    {
        isPaused = true;
        AS.Pause();
    }

    public void ResumeMusic()
    {
        isPaused = false;
        AS.Play();
    }

    public void ReduceMusicVolume()
    {
        if(AS.volume > 0)
        {
            AS.volume -= 0.04f;
        }
    }

    public void IncreaseMusicVolume()
    {
        if (AS.volume < 1)
        {
            AS.volume += 0.04f;
        }
    }

    public string ReturnMusicName()
    {
        return Music[currentMusicNum].name;
    }
}