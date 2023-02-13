using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UserLoginTestScript    // 파이어베이스를 통한 직접적인 로그인, 회원가입 로직 스크립트
{
    public bool signupSuccess = false;

    public SignUpState signupstate;
    // 싱글톤 패턴
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

    private FirebaseAuth auth;  // 로그인, 회원가입 등에 사용
    public FirebaseUser user;  // 유저 정보
    private DatabaseReference databaseReference;  // 데이터베이스 접근을 위한 객체

    public string UserId => user.UserId;
    
    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth.StateChanged += OnChanged; // 계정 상태가 바뀔 때마다 호출됨
    }

    private void OnChanged(object sender, EventArgs e)
    // 이벤트 핸들러에 대한 함수 EventArgs는 System에 속하기 때문에 using System 추가
    {
        if(auth.CurrentUser != user)
        {
            bool signed = user != auth.CurrentUser && auth.CurrentUser != null;
            if(!signed && user != null)
            {
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("로그인" + user.UserId);
                LoginState?.Invoke(true);
            }
        }
    }

    public void CreateUser(string email, string password, string nickname, int score)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                // 회원가입 실패: 이메일이 비정상인 경우
                signupstate.SUB("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                signupstate.SUB("회원가입 실패");
            }
            else
            {
                signupSuccess = true;
                FirebaseUser newUser = task.Result;

                signupstate.SUB("회원가입 성공");

                databaseReference.Child("users").Child(newUser.UserId).Child("nickname").SetValueAsync(nickname); // 닉네임 설정 
                databaseReference.Child("users").Child(newUser.UserId).Child("score").SetValueAsync(score);   // 스코어 기본 값

             
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
            Debug.Log("로그인 성공!");

            SceneManager.LoadSceneAsync("MainScene");
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }

    public void DestroyUser(string str)
    {
        string signOut = "회원탈퇴";
        string cUser = user.UserId;
        if (user == null || str.ToString() != signOut.ToString())
        {
            Debug.Log("입력한 문자와 일치하지 않습니다. 다시 입력해주세요.");
            return;
        }
        user.DeleteAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("회원 탈퇴에 실패했습니다.");
                return;
            }
            databaseReference.Child("users").Child(cUser).RemoveValueAsync();
            Debug.Log("회원 탈퇴가 성공적으로 이루어졌습니다.");
            SceneManager.LoadSceneAsync("LoginScene");

        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
