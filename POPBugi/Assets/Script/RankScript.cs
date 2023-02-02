using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;
using System.Linq;

public class RankScript : MonoBehaviour
{
    public Text[] rankText = new Text[RANK_SIZE];
    public Text[] touchText = new Text[RANK_SIZE];
    public Text myRankText;

    DatabaseReference databaseReference;

    private static int RANK_SIZE = 5;
    private Dictionary<string, int> myRankDictionary = new Dictionary<string, int>();   // 내 순위 기록을 위한 딕셔너리
    private Dictionary<string, int> topRankDictionary = new Dictionary<string, int>();  // 상위 순위 기록을 위한 딕셔너리

    void Awake()
    {
        // 데이터베이스 초기 설정
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (UserLoginTestScript.Instance.user != null)
        {
            UpdateRank();
        }
    }

    public void UpdateRank()
    {
        myRankDictionary = new Dictionary<string, int>();
        topRankDictionary = new Dictionary<string, int>();

        databaseReference.Child("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("유저 데이터를 불러오지 못했습니다.");
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                int myRank = 1; // 내 순위 기록에 사용하는 변수
                int rankCnt = 0;    // 상위 RANK_SIZE 만큼 반복하기 위해 카운팅하는 변수

                foreach (var item in snapshot.Children)
                {
                    // 내 랭크를 위한 데이터 가져오기
                    myRankDictionary.Add(item.Key.ToString(), int.Parse(item.Child("score").Value.ToString()));

                    // 상위 랭크를 위한 데이터 가져오기
                    topRankDictionary.Add(item.Child("nickname").Value.ToString(), int.Parse(item.Child("score").Value.ToString()));
                }

                // 내림차순 정렬
                var sortedMyRankDictionary = myRankDictionary.OrderByDescending(item => item.Value);
                var sortedTopRankDictionary = topRankDictionary.OrderByDescending(item => item.Value);

                // 내 순위 띄우기
                foreach (var item in sortedMyRankDictionary)
                {
                    if (item.Key.Equals(UserLoginTestScript.Instance.UserId))
                    {
                        myRankText.text = myRank + " 위";
                        break;
                    }

                    myRank++;
                }

                // 상위 RANK_SIZE 만큼 순위 띄우기
                foreach (var item in sortedTopRankDictionary)
                {
                    if (rankCnt >= RANK_SIZE) break;

                    rankText[rankCnt].text = item.Key;
                    touchText[rankCnt].text = item.Value.ToString();

                    rankCnt++;
                }
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
