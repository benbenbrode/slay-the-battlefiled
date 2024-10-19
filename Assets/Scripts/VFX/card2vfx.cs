using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class card2vfx : MonoBehaviour
{
    private Image imageComponent;
    public GameObject battle;
    void Start()
    {
        battle = GameObject.Find("battlemgr");
        // Image ������Ʈ ��������
        imageComponent = GetComponent<Image>();

        if (imageComponent != null)
        {
            // fillAmount�� 0���� �ʱ�ȭ
            imageComponent.fillAmount = 0f;

            // Fill �ִϸ��̼� ����
            StartCoroutine(FillImage());

        }
        else
        {
            Debug.LogError("No Image component found on this GameObject.");
        }
    }

    private IEnumerator FillImage()
    {
        float duration = 1f; // Fill�� �Ϸ�Ǵ� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �ð��� ���� fillAmount�� ������Ŵ
            imageComponent.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // �ִϸ��̼� ���� �� fillAmount�� 1�� ����
        imageComponent.fillAmount = 1f;
        battle.GetComponent<battlemgr>().applycker = false;
        Destroy(gameObject);
    }
}
