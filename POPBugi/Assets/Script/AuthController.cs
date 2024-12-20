using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AuthController : MonoBehaviour // 안 씀
{

    [SerializeField] string email;
    [SerializeField] string password;

    public InputField inputTextEmail;
    public InputField inputTextPassword;

    FirebaseAuth auth;
    FirebaseUser user;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SendBtnOnClick()
    {
        email = inputTextEmail.text;
        password = inputTextPassword.text;

        Debug.Log("email: " + email + ", password: " + password);

        CreateUser();
    }
    
    public void LoginBtnOnClick()
    {
        email = inputTextEmail.text;
        password = inputTextPassword.text;

        Debug.Log("email: " + email + ", password: " + password);

        LoginUser();
    }

    void CreateUser()
    {
       auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (!task.IsCanceled && !task.IsFaulted)
            {
                FirebaseUser newUser = task.Result;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                Debug.Log("회원가입 성공!");
                return;
            }
            else
            {
               // 회원가입 실패한 경우
               // 1. 이메일이 비정상인 경우
               // 2. 비밀번호가 너무 간단한 경우
               // 3. 이미 가입된 이메일인 경우
                Debug.Log("회원가입 실패");
                return;
            }
        });
    }

    void LoginUser()
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                Debug.Log("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                Debug.Log("로그인 실패");
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            Debug.Log("로그인 성공!");
        });

        user = auth.CurrentUser;
        Debug.Log(user.UserId);

        if (user != null)
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
    }
}
