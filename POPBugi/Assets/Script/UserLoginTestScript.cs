﻿using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Threading.Tasks;

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

    private FirebaseAuth auth;  // 로그인 / 회원가입 등에 사용
    private FirebaseUser user;  // 인증이 완료된 유저 정보
    private DatabaseReference m_Reference;  // 데이터베이스 접근을 위한 객체

    public string UserId => user.UserId;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        m_Reference = FirebaseDatabase.DefaultInstance.RootReference;

        // 임시처리
        if (auth.CurrentUser != null)
        {
            LogOut();
        }

        auth.StateChanged += OnChanged; // 계정 상태가 바뀔 때마다 호출됨
    }

    private void OnChanged(object sender, EventArgs e)
    // 이벤트 핸들러에 대한 함수 EventArgs는 System에 속하기 때문에 using System 추가
    {
        if(auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if(!signed && user != null)
            {
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("로그인");
                LoginState?.Invoke(true);
            }
        }
    }

    public void CreateUser(string email, string password, string nickname)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.IsCanceled)
            {
                // 회원가입 실패한 경우
                // 1. 이메일이 비정상인 경우
                // 2. 비밀번호가 너무 간단한 경우
                // 3. 이미 가입된 이메일인 경우
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
            }
            else
            {
                FirebaseUser newUser = task.Result;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                Debug.Log("회원가입 성공!");

                // 닉네임 설정
                m_Reference.Child("users").Child(newUser.UserId).Child("nickname").SetValueAsync(nickname);

                SceneManager.LoadSceneAsync("LoginScene");
                return;
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
        // 비동기 처리 중(서브스레드에서 실행 중인)인 task를 메인 스레드로 갖고 오기 위한 코드
        // using System.Threading.Tasks 필요
        // 해당 코드를 통해 Scene 전환이 가능해짐
    }

    public void LoginUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
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

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}
