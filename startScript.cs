using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip countSound;
    public AudioClip startSound;
    public AudioClip finishedSound;
    public AudioClip recordSound;
    public AudioClip multiWinSound;
    public AudioClip multiLoseSound;
    public AudioClip multiFinishedSound;
    AudioSource AS;

    void Start()
    {
        AS = this.GetComponent<AudioSource>();
    }

    public void startSoundPlay()
    {
        AS.clip = startSound;
        AS.Play();
    }
    public void countSoundPlay()
    {
        AS.clip = countSound;
        AS.Play();
    }
    public void finishedSoundPlay()
    {
        AS.clip = finishedSound;
        AS.Play();
    }
    public void recordSoundPlay()
    {
        AS.clip = recordSound;
        AS.Play();
    }

    public void multiWinSoundPlay()
    {
        AS.clip = multiWinSound;
        AS.Play();
    }

    public void multiLoseSoundPlay()
    {
        AS.clip = multiLoseSound;
        AS.Play();
    }

    public void multiFinishedSoundPlay()
    {
        AS.clip = multiFinishedSound;
        AS.Play();
    }
}
