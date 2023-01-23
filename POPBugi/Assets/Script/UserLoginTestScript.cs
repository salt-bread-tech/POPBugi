using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UserLoginTestScript
{
    private static UserLoginTestScript instance = null;
    public static UserLoginTestScript Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UserLoginTestScript();
            }

            return instance;
        }
    }

    private FirebaseAuth auth;  // �α��� / ȸ������ � ���
    private FirebaseUser user;  // ������ �Ϸ�� ���� ����

    public string UserId => user.UserId;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        // �ӽ�ó��
        if(auth.CurrentUser != null)
        {
            LogOut();
        }

        auth.StateChanged += OnChanged; // ���� ���°� �ٲ� ������ ȣ���
    }

    private void OnChanged(object sender, EventArgs e)
    // �̺�Ʈ �ڵ鷯�� ���� �Լ� EventArgs�� System�� ���ϱ� ������ using System �߰�
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("�α׾ƿ�");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("�α���");
                LoginState?.Invoke(true);
            }
        }
    }

    public void CreateUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.IsCanceled)
            {
                // ȸ������ ������ ���
                // 1. �̸����� �������� ���
                // 2. ��й�ȣ�� �ʹ� ������ ���
                // 3. �̹� ���Ե� �̸����� ���
                Debug.LogError("ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
            }
            else
            {
                FirebaseUser newUser = task.Result;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                Debug.Log("ȸ������ ����!");
                return;
            }
        });
    }

    public void LoginUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            Debug.Log("�α��� ����!");
        });

        user = auth.CurrentUser;
        Debug.Log(user.UserId);

        
        if (user != null)
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
        
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }
}
