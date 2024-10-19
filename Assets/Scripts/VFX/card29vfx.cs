using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class card29vfx : MonoBehaviour
{
    private Image imageComponent;
    public GameObject battle;
    public GameObject eff;
    public GameObject eff2;
    private Text effText;
    public GameObject prefab;  // 생성할 프리팹 (UI용)
    public Transform target;   // 목표 오브젝트의 위치 (UI 오브젝트)
    public Canvas canvas;      // 프리팹이 생성될 캔버스
    public float duration = 1.5f;  // 이동에 걸리는 시간

    void Start()
    {

        eff2 = transform.Find("eff")?.gameObject;

        if (eff2 != null)
        {
            // 'hp_txt'의 Text 컴포넌트 가져오기
            Transform hpTxtTransform = eff2.transform.Find("효과");
            if (hpTxtTransform != null)
            {
                effText = hpTxtTransform.GetComponent<Text>();
            }
        }
        if (gameObject.tag.Contains("opp"))
        {
            // opp_drop이라는 이름의 오브젝트의 위치를 target으로 설정
            GameObject oppDrop = GameObject.Find("opp_drop");
            if (oppDrop != null)
            {
                target = oppDrop.transform;
            }
            else
            {
                Debug.LogError("opp_drop 오브젝트를 찾을 수 없습니다.");
            }
        }
        // 내 태그에 "opp"가 포함되어 있는 경우
        else if (gameObject.tag.Contains("ally"))
        {
            // me_drop이라는 이름의 오브젝트의 위치를 target으로 설정
            GameObject meDrop = GameObject.Find("me_drop");
            if (meDrop != null)
            {
                target = meDrop.transform;
            }
            else
            {
                Debug.LogError("me_drop 오브젝트를 찾을 수 없습니다.");
            }
        }

        battle = GameObject.Find("battlemgr");
        eff = Resources.Load<GameObject>("vfx/CFXR3 Hit Misc F Smoke");
        prefab = Resources.Load<GameObject>("vfx/risk");
        canvas = FindObjectOfType<Canvas>();
        Instantiate(eff, transform.position, transform.rotation);
        imageComponent = GetComponent<Image>();
        battle.GetComponent<battlemgr>().applycker = false;
    }

    private void Update()
    {
        if (eff != null && eff.activeSelf)
        {
            if (effText != null)
            {
                effText.text = "자신의 방어도를2올립니다\n";
            }
        }
    }
    public IEnumerator ApplyEffect()
    {
        yield return StartCoroutine(FlyAndRotatePrefab());
        if (gameObject.tag.Contains("opp"))
        {
            // "opp_player" 태그를 가진 오브젝트를 찾습니다.
            GameObject opponent = GameObject.FindGameObjectWithTag("opp_player");

            // opp_player 오브젝트가 존재하는지 확인합니다.
            if (opponent != null)
            {
                // opp_player 오브젝트에서 PlayerState 스크립트를 가져옵니다.
                PlayerState playerState = opponent.GetComponent<PlayerState>();

                if (playerState != null)
                {
                    if (playerState != null)
                    {
                        playerState.GetComponent<PlayerState>().shield += 3;
                    }
                }
                else
                {
                    Debug.LogError("PlayerState 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("opp_player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            }
        }
        else if (gameObject.tag.Contains("ally"))
        {
            // "ally_player" 태그를 가진 오브젝트를 찾습니다.
            GameObject ally = GameObject.FindGameObjectWithTag("ally_player");

            // ally_player 오브젝트가 존재하는지 확인합니다.
            if (ally != null)
            {
                // ally_player 오브젝트에서 PlayerState 스크립트를 가져옵니다.
                PlayerState playerState = ally.GetComponent<PlayerState>();

                if (playerState != null)
                {
                    playerState.GetComponent<PlayerState>().shield += 3;
                }
                else
                {
                    Debug.LogError("PlayerState 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("ally_player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            }
        }

        yield return null;
    }

    public IEnumerator FlyAndRotatePrefab()
    {
        // 프리팹을 캔버스 안에 생성
        GameObject instance = Instantiate(prefab, canvas.transform);

        // 프리팹의 RectTransform 가져오기
        RectTransform rectTransform = instance.GetComponent<RectTransform>();

        // 시작 위치를 현재 오브젝트의 위치로 설정
        rectTransform.position = transform.position;

        // 목표 위치 가져오기 (UI 좌표)
        Vector3 startPosition = rectTransform.position;
        Vector3 targetPosition = target.position;

        float elapsedTime = 0f;

        // 이동과 회전을 1.5초 동안 수행
        while (elapsedTime < duration)
        {
            // 시간 경과 비율 계산
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 회전 (1초당 360도 회전)
            rectTransform.Rotate(new Vector3(0, 0, 360 * Time.deltaTime));

            // 선형 보간을 사용해 시작 위치에서 목표 위치까지 부드럽게 이동
            rectTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 프레임마다 잠시 대기
            yield return null;
        }

        // 목표 위치에 도착 후 위치 고정
        rectTransform.position = targetPosition;
        gameObject.GetComponent<monstate>().animacker = true;
        // 도착 후 프리팹 삭제
        Destroy(instance);
    }




}
