using UnityEngine;
using UnityEngine.EventSystems;

public class mousehover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private GameObject effObject;
    private RectTransform effRectTransform;
    private RectTransform parentRectTransform;
    private Canvas canvas;
    private Camera uiCamera;

    void Start()
    {
        // ĵ������ ã���ϴ�.
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas�� ã�� �� �����ϴ�. ��ũ��Ʈ�� ĵ���� �Ǵ� �� �ڽ� ������Ʈ�� �پ� �ִ��� Ȯ���ϼ���.");
            return;
        }

        // "eff"��� �̸��� �ڽ� ������Ʈ�� ã���ϴ�.
        Transform effTransform = transform.Find("eff");
        if (effTransform == null)
        {
            Debug.LogError("'eff' ������Ʈ�� �� ���� ������Ʈ�� �ڽĿ��� ã�� �� �����ϴ�.");
            return;
        }
        effObject = effTransform.gameObject;
        // �ʱ� ���¿��� ��Ȱ��ȭ�մϴ�.
        effObject.SetActive(false);

        effRectTransform = effObject.GetComponent<RectTransform>();
        parentRectTransform = GetComponent<RectTransform>();

        // ĵ������ ���� ��忡 ���� ī�޶� �����մϴ�.
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            uiCamera = canvas.worldCamera;
        }
        else
        {
            uiCamera = null; // Screen Space - Overlay�� ���
        }

        // eff ������Ʈ�� ��Ŀ�� �ǹ��� �߾����� �����մϴ�.
        effRectTransform.anchorMin = new Vector2(-0.1f, -0.1f);
        effRectTransform.anchorMax = new Vector2(-0.1f, -0.1f);
        effRectTransform.pivot = new Vector2(-0.1f, -0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (effObject != null)
        {
            // eff ������Ʈ�� Ȱ��ȭ�ϰ� ��ġ�� ������Ʈ�մϴ�.
            effObject.SetActive(true);
            UpdateEffPosition(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (effObject != null)
        {
            // eff ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            effObject.SetActive(false);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (effObject != null && effObject.activeSelf)
        {
            UpdateEffPosition(eventData);
        }
    }

    void UpdateEffPosition(PointerEventData eventData)
    {
        if (effRectTransform == null || parentRectTransform == null)
            return;

        Vector2 localPoint;
        // ���콺 ��ġ�� �θ� RectTransform�� ���� ��ǥ�� ��ȯ�մϴ�.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform, eventData.position, uiCamera, out localPoint);

        // �θ� RectTransform�� �߽��� �������� ��ǥ�� �Ǵ��մϴ�.
        float offsetX = localPoint.x < 0 ? 50f : 50f; // x ������ �� ����
        float offsetY = localPoint.y < 0 ? 50f : 50f; // y ������ �� ����
        localPoint.x += offsetX;
        localPoint.y += offsetY;

        // eff ������Ʈ�� ��ġ�� �����մϴ�.
        effRectTransform.anchoredPosition = localPoint;
    }
}
