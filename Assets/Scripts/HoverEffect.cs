using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform; // 이미지의 RectTransform
    private Vector3 originalScale; // 원래 크기
    private int originalSiblingIndex; // 원래의 캔버스 계층 순서

    public float scaleFactor = 1.2f; // 마우스 커서를 올렸을 때 커지는 크기 배율

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    // 마우스 커서를 올렸을 때 실행되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardCtrl>().viewcker == true)
            return;
        // 이미지 크기를 확대
        rectTransform.localScale = originalScale * scaleFactor;

        // 원래의 계층 순서를 저장하고, 캔버스에서 가장 앞으로 이동
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
    }

    // 마우스 커서가 이미지에서 벗어났을 때 실행되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardCtrl>().viewcker == true)
            return;
        // 이미지 크기를 원래대로 되돌림
        rectTransform.localScale = originalScale;

        // 원래의 계층 순서로 되돌림
        rectTransform.SetSiblingIndex(originalSiblingIndex);
    }
}

