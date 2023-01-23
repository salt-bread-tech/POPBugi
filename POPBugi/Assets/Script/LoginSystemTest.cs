using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginSystemTest : MonoBehaviour
{
    public InputField email;
    public InputField password;

    public Text outputText;

    void Start()
    {
        UserLoginTestScript.Instance.LoginState += OnChangedState;
        UserLoginTestScript.Instance.Init();
    }

    public void OnChangedState(bool sign)
    {
        //outputText.text = sign ? "로그인 : " : "로그아웃 : ";
        //outputText.text += UserLoginTestScript.Instance.UserId;
    }

    public void Create()
    {
        string e = email.text;
        string p = password.text;

        UserLoginTestScript.Instance.CreateUser(e, p);
    }

    public void LogIn()
    {
        UserLoginTestScript.Instance.LoginUser(email.text, password.text);
    }

    public void LogOut()
    {
        UserLoginTestScript.Instance.LogOut();
    }
}
