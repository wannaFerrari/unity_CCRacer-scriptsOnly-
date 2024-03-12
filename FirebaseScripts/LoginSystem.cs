using Firebase.Auth;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class LoginSystem : MonoBehaviour
{

    public TMP_InputField email;
    public TMP_InputField password;

    public TMP_InputField emailToCreate;
    public TMP_InputField passwordToCreate;
    public TMP_InputField passwordConfirm;

    public TMP_InputField createNickName;
    public Text outputText;
    // Start is called before the first frame update

    private string emailForReturn;

    public GameObject LoginCanvas;
    public GameObject CreateCanvas;
    //public GameObject CreatePanel;

    public GameObject pwIsNotCorrect;

    public Sprite[] backgroundImgs;
    public GameObject backgroundPanel;

    public int previousBG = 0;
    public int randomBG = 0;

    public bool needToChangeBackground = true;

    public string NickName;
    public string Uid;

    private bool isNewAccount = false;

    [Header("login success or failed canvas")]
    public GameObject successPanel;
    public GameObject failedPanel;

    [Header("ClickSound")]
    public AudioClip clickSound;
    public AudioSource aSource;

   


    void Start()
    {
        FirebaseAuthManagerSingleTone.Instance.LoginState += OnChangedState;
        //FirebaseAuthManagerSingleTone.Instance.loginFailed += OnLoginFailed;
        FirebaseAuthManagerSingleTone.Instance.Init();

        StartCoroutine(FadeCoroutine());
    }

    private void Update()
    {
        if (!needToChangeBackground)
        {
            StopCoroutine(FadeCoroutine());
        }

        
    }
    private void OnChangedState(bool sign)
    {
       // outputText.text = sign ? "로그인 : " : "로그아웃 : ";
       // outputText.text += FirebaseAuthManagerSingleTone.Instance.UserId;
       if(sign)
        {
            Uid = FirebaseAuthManagerSingleTone.Instance.ReturnUserID();
            if(isNewAccount)
            {
                gameObject.GetComponent<UserDataController>().OnClickedCreateButton();
            }
            successPanel.SetActive(true);
            SceneManager.LoadScene("GarageScene");
        }
        
    }
    /*
    private void OnLoginFailed(bool failed)
    {
        if(failed)
        {
            failedPanel.SetActive(true);
        }
        else
        {
            successPanel.SetActive(true);
        }

    }*/

    public void CreateAccountBtnClickedInLoginPanel()
    {
        /*
        string e = email.text;
        string p = password.text;

        FirebaseAuthManagerSingleTone.Instance.Create(e, p);*/
        clickSoundPlay();
        LoginCanvas.SetActive(false);
        CreateCanvas.SetActive(true);

    }

    public void CreateAccountWithEnteredInfo()
    {
        clickSoundPlay();
        if (passwordToCreate.text.Equals(passwordConfirm.text))
        {
            needToChangeBackground = false;
            string e = emailToCreate.text;
            string p = passwordToCreate.text;

            FirebaseAuthManagerSingleTone.Instance.Create(e, p);
            pwIsNotCorrect.SetActive(false);
            NickName = createNickName.text;
            savedData.data.userNickName = NickName;
            isNewAccount = true;
            
        }
        else
        {
            pwIsNotCorrect.SetActive(true);
        }

        
    }
    
    public void LogIn()
    {
        clickSoundPlay();
        needToChangeBackground = false;
        FirebaseAuthManagerSingleTone.Instance.Login(email.text, password.text);
       /* if (FirebaseAuthManagerSingleTone.Instance.ReturnIsLoginfailed())
        {
            Debug.Log("실패 로그인시스템");
            failedPanel.SetActive(true);
        }
        else
        {
            Debug.Log("성공 로그인시스템");
            successPanel.SetActive(true);
        }*/
        //SceneManager.LoadScene("GarageScene");
        //emailForReturn = FirebaseAuthManagerSingleTone.Instance.ReturnEmail();
        isNewAccount = false;
        
    }

    public void LogOut()
    {
        FirebaseAuthManagerSingleTone.Instance.LogOut();

    }

    public string ReturnEmail()
    {
        emailForReturn = FirebaseAuthManagerSingleTone.Instance.ReturnEmail();
        return emailForReturn;
    }

    public string ReturnUserID()
    {
        //return FirebaseAuthManagerSingleTone.Instance.UserId;
        return Uid;
    }

    public string ReturnNickName()
    {
        return NickName;
    }

    public void zzz()
    {
        SceneManager.LoadScene("GarageScene");
    }

    public bool ReturnIsNewAccount()
    {
        return isNewAccount;
    }

    public void ChangeNewAccountToNormal()
    {
        isNewAccount = false;
    }

    public void BackBtnClickedInCreatePanel()
    {
        clickSoundPlay();
        CreateCanvas.SetActive(false);
        LoginCanvas.SetActive(true);
    }
    public void BackgroundImageChanger()
    {
        //Color color = backgroundImgs[0].GetComponent<Image>().color;
        if (needToChangeBackground)
        {
            previousBG = randomBG;
            randomBG = Random.Range(0, 9);
            if (randomBG == previousBG)
            {
                if (randomBG == 8)
                {
                    randomBG = 0;
                }
                else
                {
                    randomBG += 1;
                }
            }
            backgroundPanel.GetComponent<Image>().sprite = backgroundImgs[randomBG];
            StartCoroutine(FadeCoroutine());
        }
       

    }

    void clickSoundPlay()
    {

        aSource.clip = clickSound;
        aSource.Play();
    }





    IEnumerator FadeCoroutine()
    {
        float fadeCount = 0;
        float waitCount = 0;
        while (fadeCount < 1.0f && needToChangeBackground)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            backgroundPanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, fadeCount);
        }
        while (waitCount < 3.0f && needToChangeBackground)
        {
            waitCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        while (fadeCount > 0f && needToChangeBackground)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            backgroundPanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, fadeCount);
        }
        BackgroundImageChanger();
    }
}
