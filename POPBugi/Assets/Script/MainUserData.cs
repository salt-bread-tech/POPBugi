using UnityEngine;

using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MainUserData : MonoBehaviour   // ���� ������ ���� ��ũ��Ʈ
{
    public Text scoreText; // ������ ���� �ؽ�Ʈ
    DatabaseReference databaseReference;
    
    void Start()
    {
        // �����ͺ��̽� �ʱ� ����
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
                Debug.Log("���� �����͸� �ҷ����� ���߽��ϴ�.");
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                var n = "";
                foreach(var item in snapshot.Children)
                {
                    if (item.Key.Equals(UserLoginTestScript.Instance.user.UserId))
                    {
                        n = item.Child("score").Value.ToString();
                        break;
                    }
                }
                scoreText.text = n;
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
