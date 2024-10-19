using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class textmanger : MonoBehaviour
{
    public Text targetText; // ������ �ؽ�Ʈ�� �����մϴ�.
    private Coroutine textCoroutine;

    public void ShowTextWithDelay(int a)
    {
        // ���� �ڷ�ƾ�� �̹� ���� ���̸� ���߰� ���� ����
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }
         textCoroutine = StartCoroutine(ChangeAndClearText(a));
        
    }

    // �ؽ�Ʈ�� �����ϰ� 2�� �Ŀ� ����� �ڷ�ƾ
    private IEnumerator ChangeAndClearText(int a)
    {
        if (a == 1)
        {
            targetText.text = "���̻� ī�带 �߰��� �� �����ϴ�";
        }
        if (a == 2)
        {
            targetText.text = "�̹� ���� �̸��� ī�尡 3�� �̻� �����մϴ�.";
        }
        if (a == 3)
        {
            targetText.text = "�� ���� �Ϸ�.";
        }
        // 2�� ���
        yield return new WaitForSeconds(2f);

        // �ؽ�Ʈ�� ����
        targetText.text = "";
    }
}
