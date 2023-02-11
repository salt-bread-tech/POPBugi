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

    // 코루틴 딜레이
    private WaitForSeconds _UIDelay1 = new WaitForSeconds(1.0f);
    private WaitForSeconds _UIDelay2 = new WaitForSeconds(0.3f);

    void Start()
    {
        subbox.SetActive(false);
    }

    // 서브 메세지 >> string 값을 매개 변수로 받아와서 1초간 출력
    // 사용법 : _notice.SUB("문자열");
    public void SUB(string message)
    {
        subintext.text = message;
        subbox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(SUBDelay());
    }

    // 반복 되지 않게 하기 위해서 딜레이 설정
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
