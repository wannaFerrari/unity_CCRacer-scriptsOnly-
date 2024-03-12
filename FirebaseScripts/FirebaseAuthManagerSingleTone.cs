using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class FirebaseAuthManagerSingleTone
{
    private static FirebaseAuthManagerSingleTone instance = null;

    public static FirebaseAuthManagerSingleTone Instance
    {
        get 
        {
            if(instance == null)
            {
                instance = new FirebaseAuthManagerSingleTone();
            }
            return instance;
        }
    }
    private FirebaseAuth auth;
    private FirebaseUser user;
    public string UserId => user.UserId;

    private string emailForReturn = "";

    public Action<bool> LoginState;

    public Action<bool> loginFailed;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        //임시 처리
        if(auth.CurrentUser != null)
        {
            LogOut();
        }

        auth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e) 
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;

            if(signed)
            {
                Debug.Log("로그인");
                loginFailed?.Invoke(false);
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //Debug.LogError("회원가입 실패");
                return;
            }

            // FirebaseUser newUser = task.Result;
            //Debug.LogError("회원가입 완료");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                loginFailed?.Invoke(true);
                //loginFailed = true;
                //LoginSystem g = GameObject.FindGameObjectWithTag("LoginAndData").GetComponent<LoginSystem>();
                //g.nonono();
                return;
            }

            // FirebaseUser newUser = task.Result;
            //Debug.LogError("로그인 완료");
            //SceneManager.LoadScene("GarageScene");
            emailForReturn = email;
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        //Debug.Log("로그아웃");
    }

    public string ReturnEmail()
    {
        return emailForReturn;
    }

    public string ReturnUserID()
    {
        return UserId;
    }

  



    // Update is called once per frame
    void Update()
    {

    }
}
