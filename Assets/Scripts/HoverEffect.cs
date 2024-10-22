using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform; // �̹����� RectTransform
    private Vector3 originalScale; // ���� ũ��
    private int originalSiblingIndex; // ������ ĵ���� ���� ����

    public float scaleFactor = 1.2f; // ���콺 Ŀ���� �÷��� �� Ŀ���� ũ�� ����

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    // ���콺 Ŀ���� �÷��� �� ����Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardCtrl>().viewcker == true)
            return;
        // �̹��� ũ�⸦ Ȯ��
        rectTransform.localScale = originalScale * scaleFactor;

        // ������ ���� ������ �����ϰ�, ĵ�������� ���� ������ �̵�
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
    }

    // ���콺 Ŀ���� �̹������� ����� �� ����Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardCtrl>().viewcker == true)
            return;
        // �̹��� ũ�⸦ ������� �ǵ���
        rectTransform.localScale = originalScale;

        // ������ ���� ������ �ǵ���
        rectTransform.SetSiblingIndex(originalSiblingIndex);
    }
}

