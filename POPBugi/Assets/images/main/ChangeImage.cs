using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image TestImage; // ������ ���� �̹���
    public Sprite TestSprite; // ����� �̹���
    public void Change()
    {
        TestImage.sprite = TestSprite;
    }
}
