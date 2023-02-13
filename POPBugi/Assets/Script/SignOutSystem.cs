using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignOutSystem : MonoBehaviour
{
    public InputField signoutString;

    void Start()
    {
        UserLoginTestScript.Instance.LoginState += OnChangedState;
        UserLoginTestScript.Instance.Init();
        UserLoginTestScript.Instance.signupstate = FindObjectOfType<SignUpState>();
    }

    public void OnChangedState(bool sign)
    {
        //outputText.text = sign ? "로그인 : " : "로그아웃 : ";
        //outputText.text += UserLoginTestScript.Instance.UserId;
    }
    public void SignOutUser()
    {
        UserLoginTestScript.Instance.DestroyUser(signoutString.text);
    }
}
