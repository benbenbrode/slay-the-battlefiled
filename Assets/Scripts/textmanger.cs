using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class textmanger : MonoBehaviour
{
    public Text targetText; // 변경할 텍스트를 연결합니다.
    private Coroutine textCoroutine;

    public void ShowTextWithDelay(int a)
    {
        // 만약 코루틴이 이미 실행 중이면 멈추고 새로 시작
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }
         textCoroutine = StartCoroutine(ChangeAndClearText(a));
        
    }

    // 텍스트를 변경하고 2초 후에 지우는 코루틴
    private IEnumerator ChangeAndClearText(int a)
    {
        if (a == 1)
        {
            targetText.text = "더이상 카드를 추가할 수 없습니다";
        }
        if (a == 2)
        {
            targetText.text = "이미 같은 이름의 카드가 3개 이상 존재합니다.";
        }
        if (a == 3)
        {
            targetText.text = "덱 저장 완료.";
        }
        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 텍스트를 지움
        targetText.text = "";
    }
}
