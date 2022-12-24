using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "0";
    }

    public void CountPlus()
    {
        int n = int.Parse(text.text) + 1;
        text.text = n.ToString();
    }
}
