using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginSystemTest : MonoBehaviour    // 데이터를 입력 받고 UserLoginTestScript 에 던져서 로그인, 회원가입 수행
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

    public void Create()    // 회원 가입
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
        SceneManager.LoadSceneAsync("LoginScene");
    }
}
