using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class card1vfx : MonoBehaviour
{
    private Image imageComponent;
    public GameObject battle;
    void Start()
    {
        // Image ������Ʈ ��������
        imageComponent = GetComponent<Image>();
        battle = GameObject.Find("battlemgr");
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
        imageComponent.fillAmount = 1f;
        battle.GetComponent<battlemgr>().applycker = false;
        Destroy(gameObject);
    }
}
