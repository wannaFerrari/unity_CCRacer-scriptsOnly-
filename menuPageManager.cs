using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

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

    [Header("ColorPanel")]
    public GameObject colorPanel;
    public GameObject colorBack;
    public GameObject colorReset;
    public GameObject colorSave;

    [Header("sliderControll")]
    public colorSliderControll csc;

    private Toggle ghostToggle;
    private bool ischecked = false;

    private bool rotating;
    float rotateSpeed = 20.0f;
    Vector3 mousePos, offset, rotation;

    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject itemCanvas;

    [Header("Wings")]
    public GameObject supWing;
    public GameObject porWing;
    public GameObject chiWingOn;
    public GameObject chiWingOff;

    [Header("GarageDoor")]
    public GameObject garageDoor;
    public float timeValue = 0f;
    public float positionValue;
    public bool needStartEffect = true;

    [Header("ButtonControl")]
    public GameObject modeSelect;



    public AudioClip clickSound;

    public int selectedCar;
    AudioSource AS2;

    Ray ray;
    RaycastHit hit;
    GraphicRaycaster gr;
    public Canvas cv;

    public colorSliderControll colorController;

    

    void Awake()
    {
        selectedCar = savedData.data.currentCar;
        AS2 = this.GetComponent<AudioSource>();

        gr = cv.GetComponent<GraphicRaycaster>();
        colorController = this.GetComponent<colorSliderControll>();
    }

    private void FixedUpdate()
    {
       // TimeValueChanger();
        if (needStartEffect) 
        {
            TimeValueChanger();
            GarageDoorOpeningEffect();
        }
        //TimeValueChanger();
       // GarageDoorOpeningEffect();
    }
    private void Start()
    {

        savedData.data.isRaining = false;
        savedData.data.isOnline = false;
        Debug.Log("isNewAccount from loginSystem" + ReturnIsNewAccountFromLoginSystem());
        
        //GarageDoorOpeningEffect();
        if (ReturnIsNewAccountFromLoginSystem())
        {
            Invoke("dl", 0.1f);
            ChangeNewAccountToNormalInLoginSystem();
        }
        else
        {
            if (savedData.data.CheckNeedUpdate())
            {
                Invoke("ReturnedToMenuAfterGame", 0.1f);
            }
            
            //Invoke("LoadClicked", 0.3f);
        }
        Invoke("LoadClicked", 0.3f);
        //Invoke("SaveClicked", 0.4f);
        selectedCar = savedData.data.currentCar;
        Debug.Log(selectedCar + "----------" + savedData.data.currentCar);

        /*
        if (savedData.data.currentCar == 0)
        {
            colorController.SetColor(savedData.data.supColor.r, savedData.data.supColor.g, savedData.data.supColor.b);
        }
        else if (savedData.data.currentCar == 1)
        {
            colorController.SetColor(savedData.data.porColor.r, savedData.data.porColor.g, savedData.data.porColor.b);
        }
        else if (savedData.data.currentCar == 2)
        {
            colorController.SetColor(savedData.data.chiColor.r, savedData.data.chiColor.g, savedData.data.chiColor.b);
        }
        */
        colorController.GetColor();
        //LoadClicked();

        //ghostToggle = GetComponent<Toggle>();
    }
    private void Update()
    {
        updateCarSelected();
        mouseControll();
        WingStatus();

        //////////
        if (Input.GetKeyDown(KeyCode.M))
        {

            //dl();
        }
        //////////

        //Debug.Log(EventSystem.current.IsPointerOverGameObject());
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if(results.Count <=0)
            {
                return;
            }
            if (results[0].gameObject.CompareTag("InvenItemImage"))
            {
                Debug.Log(results[1].gameObject.GetComponent<slot>().item.itemName);

            }
            else
            {
                //Debug.Log(results[0].gameObject.GetComponent<slot>().item.itemName);
            }
            //results[0].gameObject.transform.position = ped.position;
            /*
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 5000))
            {
                Debug.Log("name : " + hit.collider.name);
                //PlayMakerFSM fsm = hit.collider.GetComponent<PlayMakerFSM>();
                //fsm.SendEvent("이벤트명");
            }*/
        //EventSystem.current.RaycastAll()
        //}*/

        if ( rotating)
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
        //trackSelect.SetActive(true);
        modeSelect.SetActive(true);

    }

    public void initialQuitClicked()
    {
        clickSoundPlay();
        //Debug.Log("quit");
        SaveClicked();
        Application.Quit();
    }

    public void LongClicked()
    {
        clickSoundPlay();
        //if(checkGhostAvailable(1))
        this.GetComponent<ItemManager>().RequestUpLoadToSavedData();
        //loadingSceneController.LoadScene("LongTrack");
        loadingSceneController.LoadScene("Lobby");
        Debug.Log("Long clicked");
    }

    public void ShortClicked()
    {
        clickSoundPlay();
        //if(checkGhostAvailable(2))
        //loadingSceneController.LoadScene("ShortTrack");
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
        clickSoundPlay();
        supPanel.SetActive(true);
        supCarNamePanel.SetActive(true);
        porPanel.SetActive(false);
        porCarNamePanel.SetActive(false);
        chiPanel.SetActive(false);
        chiCarNamePanel.SetActive(false);
        selectedCar = 0;
        savedData.data.currentCar= selectedCar;
        csc.carChanged();

    }
    public void porscheClicked()
    {
        clickSoundPlay();
        //csc.carChanging();
        supPanel.SetActive(false);
        supCarNamePanel.SetActive(false);
        porPanel.SetActive(true);
        porCarNamePanel.SetActive(true);
        chiPanel.SetActive(false);
        chiCarNamePanel.SetActive(false);
        selectedCar = 1;
        savedData.data.currentCar = selectedCar;
        csc.carChanged();

    }
    public void chirionClicked()
    {
        clickSoundPlay();
        supPanel.SetActive(false);
        supCarNamePanel.SetActive(false);
        porPanel.SetActive(false);
        porCarNamePanel.SetActive(false);
        chiPanel.SetActive(true);
        chiCarNamePanel.SetActive(true);
        selectedCar = 2;
        savedData.data.currentCar = selectedCar;
        csc.carChanged();

    }

    public void closeBtnClicked()
    {
        clickSoundPlay();
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
        clickSoundPlay();
        if ( selectedCar == 0)
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
        clickSoundPlay();
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
        SaveClicked();
    }

    public void changeBtnClicked()
    {
        clickSoundPlay();
        iButtons.SetActive(false);

        carInfoPanel.SetActive(true);
        selectBtn.SetActive(true);
        closeBtn.SetActive(true);

    }

    public void ColorChangeBtnClicked()
    {
        clickSoundPlay();
        csc.GetColor();
        iButtons.SetActive(false);
        colorPanel.SetActive(true);
    }

    public void ColorResetBtnClicked()
    {
        clickSoundPlay();
        //csc.GetColor();
        csc.ResetColorToDefault();
    }

    public void ColorSaveBtnClicked()
    {
        clickSoundPlay();
        //csc.GetColor();
        colorPanel.SetActive(false);
        iButtons.SetActive(true);
        SaveClicked();
    }

    public void ColorBackBtnClicked()
    {
        clickSoundPlay();
        //csc.GetColor();
        csc.RecoverBackColor();
        colorPanel.SetActive(false);
        iButtons.SetActive(true);
    }

    public void ItemPanelOpenBtnClicked()
    {
        clickSoundPlay();
        mainCanvas.SetActive(false);
        itemCanvas.SetActive(true);
        
        //mainCanvas.SetActive(true);
    }

    public void ItemBackBtnClicked()
    {
        clickSoundPlay();
        itemCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        SaveClicked();

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
        if(Input.GetMouseButtonDown(1))
        {
            rotating = true;
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1)) { 
        
            rotating = false;
        }
    }

    public void WingStatus()
    {
        if(savedData.data.savedDownforce != 0)
        {
            supWing.SetActive(true);
            porWing.SetActive(true);
            chiWingOn.SetActive(true);
            chiWingOff.SetActive(false);
        }
        else
        {
            supWing.SetActive(false);
            porWing.SetActive(false);
            chiWingOn.SetActive(false);
            chiWingOff.SetActive(true);
        }
    }

    public void dl()
    {
        Debug.Log("DLDLDLDLDLDLDL");
        //this.GetComponent<ItemManager>().RequestDownLoadFromSavedData();
        this.GetComponent<ItemManager>().RequestMakeItems();
        this.GetComponent<colorSliderControll>().SetColorToInitialColor();
        ChangeCarAndChangeColorToLastData();
        SaveClicked();
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

    public void LoadClicked()
    {
        GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<UserDataController>().OnClickLoadButton();
        Debug.Log(" load clicked");
        
    }

    public void SaveClicked()
    {
        GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<UserDataController>().OnClickSaveButton();
        Debug.Log(" savedClicked");
    }

    public void ReturnedToMenuAfterGame()
    {
        GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<UserDataController>().DontUpdateInven();
        GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<UserDataController>().OnClickSaveButton();
        Debug.Log("returned to menu after game");
    }
    public void ChangeNewAccountToNormalInLoginSystem()
    {
        GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<LoginSystem>().ChangeNewAccountToNormal();
    }

    public bool ReturnIsNewAccountFromLoginSystem()
    {
        return GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<LoginSystem>().ReturnIsNewAccount();
    }

    public void GarageDoorOpeningEffect()
    {
        //timeValue = 0f;
        //timeValue += Time.deltaTime;
        positionValue = garageDoor.GetComponent<RectTransform>().anchoredPosition.y;
        if(timeValue > 1f && timeValue < 3f)
        {
            positionValue += 20f;
            garageDoor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, positionValue);

        }
        else if (timeValue >= 3f)
        {
            needStartEffect = false;
            //garageDoor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        }
        else if (timeValue < 1f)
        {
            garageDoor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        }

    }

    public void TimeValueChanger()
    {
        timeValue += Time.deltaTime;
    }

    public void MultiplayClicked() 
    {
        clickSoundPlay();
        //if(checkGhostAvailable(1))
        this.GetComponent<ItemManager>().RequestUpLoadToSavedData();
        //loadingSceneController.LoadScene("LongTrack");
        loadingSceneController.LoadScene("Lobby");
        Debug.Log("Long clicked");
    }

    public void BackBtnInModeSelect()
    {
        clickSoundPlay();
        modeSelect.SetActive(false);
        iButtons.SetActive(true);
    }

    public void SinglePlayClicked()
    {
        clickSoundPlay();
        loadingSceneController.LoadScene("Single_Lobby");
    }

    public void ChangeCarAndChangeColorToLastData()
    {
        selectedCar = savedData.data.currentCar;
        
        if (savedData.data.currentCar == 0)
        {
            colorController.SetSlider(savedData.data.supColor.r, savedData.data.supColor.g, savedData.data.supColor.b);
        }
        else if (savedData.data.currentCar == 1)
        {
            colorController.SetSlider(savedData.data.porColor.r, savedData.data.porColor.g, savedData.data.porColor.b);
        }
        else if (savedData.data.currentCar == 2)
        {
            colorController.SetSlider(savedData.data.chiColor.r, savedData.data.chiColor.g, savedData.data.chiColor.b);
        }
        /*

        colorController.SetColor(savedData.data.supColor.r, savedData.data.supColor.g, savedData.data.supColor.b);
        colorController.SetColor(savedData.data.porColor.r, savedData.data.porColor.g, savedData.data.porColor.b);
        colorController.SetColor(savedData.data.chiColor.r, savedData.data.chiColor.g, savedData.data.chiColor.b);*/
    }
}
