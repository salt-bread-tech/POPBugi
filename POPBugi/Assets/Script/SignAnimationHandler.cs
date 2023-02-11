using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignAnimationHandler : MonoBehaviour
{
    #region Methods
    public void OnEnterNextScene()
    {
        if (UserLoginTestScript.Instance.signupSuccess)
            SceneManager.LoadSceneAsync("LoginScene");
    }
    #endregion Methods
}
