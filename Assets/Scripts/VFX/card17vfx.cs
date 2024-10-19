using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class card17vfx : MonoBehaviour
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
        if (gameObject.tag.Contains("ally"))
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
        else if (gameObject.tag.Contains("opp"))
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
        prefab = Resources.Load<GameObject>("vfx/rifire");
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
                effText.text = "상대 플레이어에\r\n게 10의 화염을\r\n부여합니다\r\n";
            }
        }
    }
    public IEnumerator ApplyEffect()
    {
        yield return StartCoroutine(FlyAndScalePrefab());
        // 자기 자신의 태그에 "ally"가 포함된 경우
        if (gameObject.tag.Contains("ally"))
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
                        playerState.GetComponent<PlayerState>().fire += 10;
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
        // 자기 자신의 태그에 "opp"가 포함된 경우
        else if (gameObject.tag.Contains("opp"))
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
                    playerState.GetComponent<PlayerState>().fire += 10;
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

    public IEnumerator FlyAndScalePrefab()
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
        float scaleSpeedMultiplier = 6f;  // 더 빠른 스케일 변화 주기를 위해 값을 증가

        // 이동과 스케일 변경을 1.5초 동안 수행
        while (elapsedTime < duration)
        {
            // 시간 경과 비율 계산
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 선형 보간을 사용해 시작 위치에서 목표 위치까지 부드럽게 이동
            rectTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 스케일 조정 (빠른 주기로 커졌다가 작아지게)
            float scale = Mathf.PingPong(t * scaleSpeedMultiplier, 2.0f) + 0.5f;  // 더 빠르게 커지고 작아짐
            rectTransform.localScale = new Vector3(scale, scale, scale);

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
