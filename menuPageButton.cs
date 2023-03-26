using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuPageButton : MonoBehaviour
{

    public GameObject iButtons;
    public GameObject trackSelect;
    public AudioClip clickSound;
    AudioSource AS2;
 

    void Awake()
    {
        AS2 = this.GetComponent<AudioSource>();
    }

    public void initialRaceClicked()
    {
        clickSoundPlay();
        iButtons.SetActive(false);
        trackSelect.SetActive(true);

    }

    public void initialQuitClicked()
    {
        clickSoundPlay();
        Debug.Log("quit");
        Application.Quit();
    }

    public void LongClicked()
    {
        clickSoundPlay();
        loadingSceneController.LoadScene("LongTrack");
        Debug.Log("Long clicked");
    }

    public void ShortClicked()
    {
        clickSoundPlay();
        loadingSceneController.LoadScene("ShortTrack");
        Debug.Log("short clicked");
    }

    public void BackClicked()
    {
        clickSoundPlay();
        trackSelect.SetActive(false);
        iButtons.SetActive(true);
    }
    void clickSoundPlay()
    {

        AS2.clip = clickSound;
        AS2.Play();
    }


}
