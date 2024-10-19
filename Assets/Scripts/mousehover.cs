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
        // 캔버스를 찾습니다.
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다. 스크립트가 캔버스 또는 그 자식 오브젝트에 붙어 있는지 확인하세요.");
            return;
        }

        // "eff"라는 이름의 자식 오브젝트를 찾습니다.
        Transform effTransform = transform.Find("eff");
        if (effTransform == null)
        {
            Debug.LogError("'eff' 오브젝트를 이 게임 오브젝트의 자식에서 찾을 수 없습니다.");
            return;
        }
        effObject = effTransform.gameObject;
        // 초기 상태에서 비활성화합니다.
        effObject.SetActive(false);

        effRectTransform = effObject.GetComponent<RectTransform>();
        parentRectTransform = GetComponent<RectTransform>();

        // 캔버스의 렌더 모드에 따라 카메라를 설정합니다.
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            uiCamera = canvas.worldCamera;
        }
        else
        {
            uiCamera = null; // Screen Space - Overlay인 경우
        }

        // eff 오브젝트의 앵커와 피벗을 중앙으로 설정합니다.
        effRectTransform.anchorMin = new Vector2(-0.1f, -0.1f);
        effRectTransform.anchorMax = new Vector2(-0.1f, -0.1f);
        effRectTransform.pivot = new Vector2(-0.1f, -0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (effObject != null)
        {
            // eff 오브젝트를 활성화하고 위치를 업데이트합니다.
            effObject.SetActive(true);
            UpdateEffPosition(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (effObject != null)
        {
            // eff 오브젝트를 비활성화합니다.
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
        // 마우스 위치를 부모 RectTransform의 로컬 좌표로 변환합니다.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform, eventData.position, uiCamera, out localPoint);

        // 부모 RectTransform의 중심을 기준으로 좌표를 판단합니다.
        float offsetX = localPoint.x < 0 ? 50f : 50f; // x 오프셋 값 조정
        float offsetY = localPoint.y < 0 ? 50f : 50f; // y 오프셋 값 조정
        localPoint.x += offsetX;
        localPoint.y += offsetY;

        // eff 오브젝트의 위치를 설정합니다.
        effRectTransform.anchoredPosition = localPoint;
    }
}
