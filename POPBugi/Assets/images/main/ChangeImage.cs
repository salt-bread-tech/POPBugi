using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image TestImage; // 기존에 존재 이미지
    public Sprite TestSprite; // 변경될 이미지
    public void Change()
    {
        TestImage.sprite = TestSprite;
    }
}
