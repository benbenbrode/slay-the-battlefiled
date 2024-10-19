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
    public GameObject prefab;  // ������ ������ (UI��)
    public Transform target;   // ��ǥ ������Ʈ�� ��ġ (UI ������Ʈ)
    public Canvas canvas;      // �������� ������ ĵ����
    public float duration = 1.5f;  // �̵��� �ɸ��� �ð�

    void Start()
    {

        eff2 = transform.Find("eff")?.gameObject;

        if (eff2 != null)
        {
            // 'hp_txt'�� Text ������Ʈ ��������
            Transform hpTxtTransform = eff2.transform.Find("ȿ��");
            if (hpTxtTransform != null)
            {
                effText = hpTxtTransform.GetComponent<Text>();
            }
        }
        if (gameObject.tag.Contains("ally"))
        {
            // opp_drop�̶�� �̸��� ������Ʈ�� ��ġ�� target���� ����
            GameObject oppDrop = GameObject.Find("opp_drop");
            if (oppDrop != null)
            {
                target = oppDrop.transform;
            }
            else
            {
                Debug.LogError("opp_drop ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        // �� �±׿� "opp"�� ���ԵǾ� �ִ� ���
        else if (gameObject.tag.Contains("opp"))
        {
            // me_drop�̶�� �̸��� ������Ʈ�� ��ġ�� target���� ����
            GameObject meDrop = GameObject.Find("me_drop");
            if (meDrop != null)
            {
                target = meDrop.transform;
            }
            else
            {
                Debug.LogError("me_drop ������Ʈ�� ã�� �� �����ϴ�.");
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
                effText.text = "��� �÷��̾\r\n�� 10�� ȭ����\r\n�ο��մϴ�\r\n";
            }
        }
    }
    public IEnumerator ApplyEffect()
    {
        yield return StartCoroutine(FlyAndScalePrefab());
        // �ڱ� �ڽ��� �±׿� "ally"�� ���Ե� ���
        if (gameObject.tag.Contains("ally"))
        {
            // "opp_player" �±׸� ���� ������Ʈ�� ã���ϴ�.
            GameObject opponent = GameObject.FindGameObjectWithTag("opp_player");

            // opp_player ������Ʈ�� �����ϴ��� Ȯ���մϴ�.
            if (opponent != null)
            {
                // opp_player ������Ʈ���� PlayerState ��ũ��Ʈ�� �����ɴϴ�.
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
                    Debug.LogError("PlayerState ������Ʈ�� ã�� �� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError("opp_player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        // �ڱ� �ڽ��� �±׿� "opp"�� ���Ե� ���
        else if (gameObject.tag.Contains("opp"))
        {
            // "ally_player" �±׸� ���� ������Ʈ�� ã���ϴ�.
            GameObject ally = GameObject.FindGameObjectWithTag("ally_player");

            // ally_player ������Ʈ�� �����ϴ��� Ȯ���մϴ�.
            if (ally != null)
            {
                // ally_player ������Ʈ���� PlayerState ��ũ��Ʈ�� �����ɴϴ�.
                PlayerState playerState = ally.GetComponent<PlayerState>();

                if (playerState != null)
                {
                    playerState.GetComponent<PlayerState>().fire += 10;
                }
                else
                {
                    Debug.LogError("PlayerState ������Ʈ�� ã�� �� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError("ally_player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }

        yield return null;
    }

    public IEnumerator FlyAndScalePrefab()
    {
        // �������� ĵ���� �ȿ� ����
        GameObject instance = Instantiate(prefab, canvas.transform);

        // �������� RectTransform ��������
        RectTransform rectTransform = instance.GetComponent<RectTransform>();

        // ���� ��ġ�� ���� ������Ʈ�� ��ġ�� ����
        rectTransform.position = transform.position;

        // ��ǥ ��ġ �������� (UI ��ǥ)
        Vector3 startPosition = rectTransform.position;
        Vector3 targetPosition = target.position;

        float elapsedTime = 0f;
        float scaleSpeedMultiplier = 6f;  // �� ���� ������ ��ȭ �ֱ⸦ ���� ���� ����

        // �̵��� ������ ������ 1.5�� ���� ����
        while (elapsedTime < duration)
        {
            // �ð� ��� ���� ���
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // ���� ������ ����� ���� ��ġ���� ��ǥ ��ġ���� �ε巴�� �̵�
            rectTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // ������ ���� (���� �ֱ�� Ŀ���ٰ� �۾�����)
            float scale = Mathf.PingPong(t * scaleSpeedMultiplier, 2.0f) + 0.5f;  // �� ������ Ŀ���� �۾���
            rectTransform.localScale = new Vector3(scale, scale, scale);

            // �����Ӹ��� ��� ���
            yield return null;
        }

        // ��ǥ ��ġ�� ���� �� ��ġ ����
        rectTransform.position = targetPosition;
        gameObject.GetComponent<monstate>().animacker = true;
        // ���� �� ������ ����
        Destroy(instance);
    }




}
