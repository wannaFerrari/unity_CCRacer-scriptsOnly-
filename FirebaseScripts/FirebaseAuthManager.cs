using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;


public class FirebaseAuthManager : MonoBehaviour
{

    private FirebaseAuth auth;
    private FirebaseUser user;
    public InputField email;
    public InputField password;


    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Create()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
                return;
            }

           // FirebaseUser newUser = task.Result;
            Debug.LogError("ȸ������ �Ϸ�");
        });
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
                return;
            }

            // FirebaseUser newUser = task.Result;
            Debug.LogError("�α��� �Ϸ�");
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
