using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class SignUpState : MonoBehaviour
{
    [Header("SubNotice")]
    public GameObject subbox;
    public Text subintext;
    public Animator subani;

    // �ڷ�ƾ ������
    private WaitForSeconds _UIDelay1 = new WaitForSeconds(1.0f);
    private WaitForSeconds _UIDelay2 = new WaitForSeconds(0.3f);

    void Start()
    {
        subbox.SetActive(false);
    }

    // ���� �޼��� >> string ���� �Ű� ������ �޾ƿͼ� 1�ʰ� ���
    // ���� : _notice.SUB("���ڿ�");
    public void SUB(string message)
    {
        subintext.text = message;
        subbox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(SUBDelay());
    }

    // �ݺ� ���� �ʰ� �ϱ� ���ؼ� ������ ����
    IEnumerator SUBDelay()
    {
        subbox.SetActive(true);
        subani.SetBool("isOn", true);
        yield return _UIDelay1;
        subani.SetBool("isOn", false);
        yield return _UIDelay2;
        subbox.SetActive(false);
    }
}
