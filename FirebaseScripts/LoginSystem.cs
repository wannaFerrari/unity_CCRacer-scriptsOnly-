using Firebase.Auth;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoginSystem : MonoBehaviour
{

    public InputField email;
    public InputField password;

    public InputField emailToCreate;
    public InputField passwordToCreate;
    public InputField passwordConfirm;

    public InputField createNickName;
    public Text outputText;
    // Start is called before the first frame update

    private string emailForReturn;

    public GameObject LoginCanvas;
    public GameObject CreateCanvas;
    public GameObject CreatePanel;

    public GameObject pwIsNotCorrect;

    public string NickName;
    public string Uid;

    private bool isNewAccount = false;
    
    
    void Start()
    {
        FirebaseAuthManagerSingleTone.Instance.LoginState += OnChangedState;
        FirebaseAuthManagerSingleTone.Instance.Init();
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
            SceneManager.LoadScene("GarageScene");
        }
        
    }

    public void CreateAccountBtnClickedInLoginPanel()
    {
        /*
        string e = email.text;
        string p = password.text;

        FirebaseAuthManagerSingleTone.Instance.Create(e, p);*/

        LoginCanvas.SetActive(false);
        CreateCanvas.SetActive(true);

    }

    public void CreateAccountWithEnteredInfo()
    {
        if (passwordToCreate.text.Equals(passwordConfirm.text))
        {
            string e = emailToCreate.text;
            string p = passwordToCreate.text;

            FirebaseAuthManagerSingleTone.Instance.Create(e, p);
            pwIsNotCorrect.SetActive(false);
            NickName = createNickName.text;
            isNewAccount = true;
        }
        else
        {
            pwIsNotCorrect.SetActive(true);
        }

        
    }
    
    public void LogIn()
    {
        FirebaseAuthManagerSingleTone.Instance.Login(email.text, password.text);
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
}
