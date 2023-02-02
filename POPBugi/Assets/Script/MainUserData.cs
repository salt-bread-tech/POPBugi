using UnityEngine;

using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MainUserData : MonoBehaviour   // 유저 데이터 관리 스크립트
{
    public Text scoreText; // 점수를 띄우는 텍스트
    public Text nicknameText;   // 소셜에서 닉네임 띄우는 텍스트
    DatabaseReference databaseReference;
    
    void Start()
    {
        // 데이터베이스 초기 설정
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (UserLoginTestScript.Instance.user != null)
        {
            UpdateUser();
        }
    }

    private void UpdateUser()
    {
        databaseReference.Child("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("유저 데이터를 불러오지 못했습니다.");
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                var touchCount = "";
                var nickname = "";

                foreach (var item in snapshot.Children)
                {
                    if (item.Key.Equals(UserLoginTestScript.Instance.user.UserId))
                    {
                        touchCount = item.Child("score").Value.ToString();  // 터치 수 가져오기
                        nickname = item.Child("nickname").Value.ToString(); // 닉네임 가져오기
                        break;
                    }
                }
                scoreText.text = touchCount;    // 터치 수 띄우기
                nicknameText.text = nickname;   // 닉네임 띄우기
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void OnClickUpdateScore()
    {
        int n = int.Parse(scoreText.text) + 1;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/users/" + UserLoginTestScript.Instance.user.UserId + "/" + "score"] = n;
        scoreText.text = n.ToString();
        databaseReference.UpdateChildrenAsync(childUpdates);
    }
}
