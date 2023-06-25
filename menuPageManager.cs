using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuPageManager : MonoBehaviour
{

    public GameObject iButtons;
    public GameObject trackSelect;
    public GameObject carInfoPanel;
    public GameObject supPanel;
    public GameObject porPanel;
    public GameObject chiPanel;
    public GameObject supCarNamePanel;
    public GameObject porCarNamePanel;
    public GameObject chiCarNamePanel;
    public GameObject closeBtn;
    public GameObject openBtn;
    public GameObject imgBtnContainer;
    public GameObject selectBtn;
    public GameObject Supra;
    public GameObject Porsche;
    public GameObject Chiron;
    public GameObject spinningPlate;

    public ghost L_supra;
    public ghost L_porsche;
    public ghost L_chiron;
    public ghost S_supra;
    public ghost S_porsche;
    public ghost S_chiron;

    private Toggle ghostToggle;
    private bool ischecked = false;

    private bool rotating;
    float rotateSpeed = 20.0f;
    Vector3 mousePos, offset, rotation;


    public AudioClip clickSound;

    [HideInInspector] public int selectedCar = 0;
    AudioSource AS2;
 

    void Awake()
    {
        selectedCar = savedData.data.currentCar;
        AS2 = this.GetComponent<AudioSource>();

       

    }
    private void Start()
    {
        //ghostToggle = GetComponent<Toggle>();
    }
    private void Update()
    {
        updateCarSelected();
        mouseControll();
        if( rotating)
        {
            offset = (Input.mousePosition - mousePos);
            rotation.y = -(offset.x + offset.y) * Time.deltaTime * rotateSpeed;
            Supra.transform.Rotate(rotation);
            Porsche.transform.Rotate(rotation);
            Chiron.transform.Rotate(rotation);
            spinningPlate.transform.Rotate(rotation);
            mousePos = Input.mousePosition;
        }
    }

    /*public static class savedData
    {
        public static int currentCar= 0;
    }*/

    public void initialRaceClicked()
    {
        clickSoundPlay();
        iButtons.SetActive(false);
        trackSelect.SetActive(true);

    }

    public void initialQuitClicked()
    {
        clickSoundPlay();
        //Debug.Log("quit");
        Application.Quit();
    }

    public void LongClicked()
    {
        clickSoundPlay();
        //if(checkGhostAvailable(1))
        loadingSceneController.LoadScene("LongTrack");
        Debug.Log("Long clicked");
    }

    public void ShortClicked()
    {
        clickSoundPlay();
        //if(checkGhostAvailable(2))
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

    public void supraClicked()
    {
        supPanel.SetActive(true);
        supCarNamePanel.SetActive(true);
        porPanel.SetActive(false);
        porCarNamePanel.SetActive(false);
        chiPanel.SetActive(false);
        chiCarNamePanel.SetActive(false);
        selectedCar = 0;
        savedData.data.currentCar= selectedCar;

    }
    public void porscheClicked()
    {
        supPanel.SetActive(false);
        supCarNamePanel.SetActive(false);
        porPanel.SetActive(true);
        porCarNamePanel.SetActive(true);
        chiPanel.SetActive(false);
        chiCarNamePanel.SetActive(false);
        selectedCar = 1;
        savedData.data.currentCar = selectedCar;

    }
    public void chirionClicked()
    {
        supPanel.SetActive(false);
        supCarNamePanel.SetActive(false);
        porPanel.SetActive(false);
        porCarNamePanel.SetActive(false);
        chiPanel.SetActive(true);
        chiCarNamePanel.SetActive(true);
        selectedCar = 2;
        savedData.data.currentCar = selectedCar;

    }

    public void closeBtnClicked()
    {
        //Debug.Log("fffffffffffffffff");
        //float cp;
        //RectTransform ci = carInfoPanel.GetComponent<RectTransform>();
        //cp = ci.anchoredPosition.x
        //closePanels();
        //test();
        if ( selectedCar == 0)
        {
            supPanel.SetActive(false);
            supCarNamePanel.SetActive(false);
        }
        else if(selectedCar == 1)
        {
            porPanel.SetActive(false);
            porCarNamePanel.SetActive(false);
        }
        else if(selectedCar == 2)
        {
            chiPanel.SetActive(false);
            chiCarNamePanel.SetActive(false);
        }
        imgBtnContainer.SetActive(false);
        closeBtn.SetActive(false);
        selectBtn.SetActive(false);
        openBtn.SetActive(true);
        
    }

    public void openBtnClicked()
    {
        if( selectedCar == 0)
        {
            supPanel.SetActive(true);
            supCarNamePanel.SetActive(true);
        }
        else if(selectedCar == 1)
        {
            porPanel.SetActive(true);
            porCarNamePanel.SetActive(true);
        }
        else if (selectedCar == 2)
        {
            chiPanel.SetActive(true);
            chiCarNamePanel.SetActive(true);
        }
        imgBtnContainer.SetActive(true);
        closeBtn.SetActive(true);
        selectBtn.SetActive(true);
        openBtn.SetActive(false);
    }
    
    public void selectBtnClicked()
    {
        supPanel.SetActive(false);
        supCarNamePanel.SetActive(false);
        porPanel.SetActive(false);
        porCarNamePanel.SetActive(false);
        chiPanel.SetActive(false);
        chiCarNamePanel.SetActive(false);
        selectBtn.SetActive(false);
        closeBtn.SetActive(false);
        openBtn.SetActive(false);
        carInfoPanel.SetActive(false);
        iButtons.SetActive(true);
    }

    public void changeBtnClicked()
    {
        iButtons.SetActive(false);

        carInfoPanel.SetActive(true);
        selectBtn.SetActive(true);
        closeBtn.SetActive(true);

    }
    public void ghostToggleValueChange(Toggle ghosttoggle)
    {
        //Debug.Log("111111111111111111111" + ghosttoggle.isOn);
        //savedData.data.activateGhost = ghosttoggle.isOn;
       // savedData.data.activateGhost = true;
        if (!ischecked)
        {
            savedData.data.activateGhost= true;
            ischecked = !ischecked;
            //Debug.Log("zzzzzzzzzzzz");
        }
        else
        {
            savedData.data.activateGhost= false;
            ischecked = !ischecked;
        }
    }
    public void test()
    {
        Debug.Log("test");
    }
    public void updateCarSelected()
    {
        if( selectedCar == 0)
        {
            Porsche.SetActive(false);
            Chiron.SetActive(false);
            Supra.SetActive(true);
        }
        else if(selectedCar == 1)
        {
            Supra.SetActive(false);
            Chiron.SetActive(false);
            Porsche.SetActive(true);
        }
        else if(selectedCar == 2)
        {
            Supra.SetActive(false);
            Porsche.SetActive(false);
            Chiron.SetActive(true);
        }
    }
    public void mouseControll()
    {
        if(Input.GetMouseButtonDown(0))
        {
            rotating = true;
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) { 
        
            rotating = false;
        }
    }
    /*
    public bool checkGhostAvailable(int track)
    {
        if (ischecked)
        {
            if (track == 1)
            {
                if (selectedCar == 0)
                {
                    if (L_supra.timeStamp.Count == 0)
                    {
                        //  .setActive(true);
                        return false;
                    }
                    else return true;
                }
                else if (selectedCar == 1)
                {
                    if (L_porsche.timeStamp.Count == 0)
                    {
                        //  .setActive(true);
                        return false;
                    }
                    else return true;
                }
                else
                {
                    if (L_chiron.timeStamp.Count == 0)
                    {
                        //  .setActive(true);
                        return false;
                    }
                    else return true;
                }
            }
            else
            {
                if (selectedCar == 0)
                {
                    if (S_supra.timeStamp.Count == 0)
                    {
                        //  .setActive(true);
                        return false;
                    }
                    else return true;
                }
                else if (selectedCar == 1)
                {
                    if (S_porsche.timeStamp.Count == 0)
                    {
                        //  .setActive(true);
                        return false;
                    }
                    else return true;
                }
                else
                {
                    if (S_chiron.timeStamp.Count == 0)
                    {
                        //  .setActive(true);
                        return false;
                    }
                    else return true;
                }
            }
        }
        else return true;
    }
    */
    /*
    IEnumerator closePanels()
    {
        Debug.Log("zzzzzzzzzz");
        RectTransform ci = carInfoPanel.GetComponent<RectTransform>();
        float cur = 0;
        while (true)
        {
            cur += Time.deltaTime;
            ci.anchoredPosition = Vector3.Lerp(ci.anchoredPosition, new Vector3(100, 0), cur / 2);
            //ci.anchoredPosition = new Vector2(Mathf.Lerp(ci.anchoredPosition, new Vector2(100,0),))
           // carInfoPanel.localPosition = Vector3.Lerp
            yield return null;
        }
    }*/


}
