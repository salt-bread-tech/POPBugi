using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void LoginToSignUp()
    {
        SceneManager.LoadSceneAsync("SignUpScene");
    }

    public void SignUpToLogin()
    {
        SceneManager.LoadSceneAsync("LoginScene");
    }

    public void MainToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void HomeToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

}
