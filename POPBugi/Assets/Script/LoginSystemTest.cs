using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginSystemTest : MonoBehaviour
{
    public InputField nickname;
    public InputField email;
    public InputField password;
    public InputField rePassword;

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
        string n = nickname.text;
        string e = email.text;
        string p = password.text;
        string rp = rePassword.text;
        int s = 0;

        if (p == rp)
        {
            UserLoginTestScript.Instance.CreateUser(e, p, n, s);
        }
        else
        {
            Debug.Log("비밀번호가 일치하지 않습니다.");
        }
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
