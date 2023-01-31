using UnityEngine;

using Firebase;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class MainUserData : MonoBehaviour
{
    public Text text;
    class PlayerData
    {
        public string nickName;
        public int score;

        public PlayerData(string nickName, int score)
        {
            this.nickName = nickName;
            this.score = score;
        }
    }

    DatabaseReference reference;
    

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        if(UserLoginTestScript.Instance.user != null)
        {
            UpdateUser();
        }
    }

    private void UpdateUser()
    {
        reference.Child("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else
            {
                DataSnapshot snapshot = task.Result;
                var n = "";
                foreach(var item in snapshot.Children)
                {
                    Debug.Log("당신의 스코어: " + item.Child("score").Value);
                    n = (string)item.Child("score").Value;
                }
                text.text = n;
                
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
        
    }

    public void OnClickUpdateScore()
    {
        int n = int.Parse(text.text) + 1;
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/users/" + UserLoginTestScript.Instance.user.UserId + "/" + "score"] = n;
        text.text = n.ToString();
        reference.UpdateChildrenAsync(childUpdates);
    }
}
