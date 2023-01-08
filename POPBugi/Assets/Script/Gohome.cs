using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gohome : MonoBehaviour
{

    public void MainToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void HomeToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}

