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
        // Image 컴포넌트 가져오기
        imageComponent = GetComponent<Image>();

        if (imageComponent != null)
        {
            // fillAmount를 0으로 초기화
            imageComponent.fillAmount = 0f;

            // Fill 애니메이션 시작
            StartCoroutine(FillImage());

        }
        else
        {
            Debug.LogError("No Image component found on this GameObject.");
        }
    }

    private IEnumerator FillImage()
    {
        float duration = 1f; // Fill이 완료되는 시간
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 fillAmount를 증가시킴
            imageComponent.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 애니메이션 종료 후 fillAmount를 1로 설정
        imageComponent.fillAmount = 1f;
        battle.GetComponent<battlemgr>().applycker = false;
        Destroy(gameObject);
    }
}
