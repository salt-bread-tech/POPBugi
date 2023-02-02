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
    private Dictionary<string, int> myRankDictionary = new Dictionary<string, int>();   // �� ���� ����� ���� ��ųʸ�
    private Dictionary<string, int> topRankDictionary = new Dictionary<string, int>();  // ���� ���� ����� ���� ��ųʸ�

    void Awake()
    {
        // �����ͺ��̽� �ʱ� ����
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
                Debug.Log("���� �����͸� �ҷ����� ���߽��ϴ�.");
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                int myRank = 1; // �� ���� ��Ͽ� ����ϴ� ����
                int rankCnt = 0;    // ���� RANK_SIZE ��ŭ �ݺ��ϱ� ���� ī�����ϴ� ����

                foreach (var item in snapshot.Children)
                {
                    // �� ��ũ�� ���� ������ ��������
                    myRankDictionary.Add(item.Key.ToString(), int.Parse(item.Child("score").Value.ToString()));

                    // ���� ��ũ�� ���� ������ ��������
                    topRankDictionary.Add(item.Child("nickname").Value.ToString(), int.Parse(item.Child("score").Value.ToString()));
                }

                // �������� ����
                var sortedMyRankDictionary = myRankDictionary.OrderByDescending(item => item.Value);
                var sortedTopRankDictionary = topRankDictionary.OrderByDescending(item => item.Value);

                // �� ���� ����
                foreach (var item in sortedMyRankDictionary)
                {
                    if (item.Key.Equals(UserLoginTestScript.Instance.UserId))
                    {
                        myRankText.text = myRank + " ��";
                        break;
                    }

                    myRank++;
                }

                // ���� RANK_SIZE ��ŭ ���� ����
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
