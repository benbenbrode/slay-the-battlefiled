using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card17 : MonoBehaviour
{
    public Vector3 objectPosition;
    public Text eff;
    public GameObject drop;

    public Outline outline; // Outline 컴포넌트
    public Color glowColor = Color.green; // 테두리가 초록색으로 빛나는 색상
    public GameObject me;

    private void Start()
    {
        me = GameObject.Find("Canvas/me_drop");
        Transform effTransform = transform.Find("eff");
        Transform effTransform2 = transform.Find("cost_txt");
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "검은용소환";
        }
        Transform child = transform.Find("cost");
        if (child == null)
        {
            Debug.LogError("Child 'cost' not found under this GameObject.");
            return;
        }

        Text grandChildText = child.GetComponentInChildren<Text>();
        if (grandChildText == null)
        {
            Debug.LogError("Text component not found in children of 'cost'.");
            return;
        }
        grandChildText.text = "4";
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline component not found on this GameObject.");
        }

    }

    void Update()
    {
        if (outline == null)
        {
            return; // Outline 컴포넌트가 없으면 업데이트 하지 않음
        }

        // cost가 1 이상일 때 테두리 색상을 초록색으로 설정
        if (me.GetComponent<PlayerState>().cost >= 4)
        {
            outline.effectColor = glowColor;
        }
        else
        {
            // cost가 1 미만일 때 테두리 색상을 원래 색상으로 설정하거나 테두리를 끌 수 있음
            outline.effectColor = Color.clear; // 테두리를 보이지 않게 설정 (또는 원래 색상으로 설정 가능)
        }
    }

    void OnDestroy()
    {
        drop = GameObject.Find(gameObject.GetComponent<Target>().drop);
        Debug.Log(gameObject.GetComponent<Target>().drop);
        Debug.Log(gameObject.GetComponent<Target>().tag);
        if (gameObject.GetComponent<Target>().tag == "ally_mon3" || gameObject.GetComponent<Target>().tag == "ally_mon2" || gameObject.GetComponent<Target>().tag == "ally_mon1")
        {
            ActivateEffect();
        }
        else
        {
            ActivateEffect2(drop);
        }


    }

    public void ActivateEffect()
    {
        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        // 프리팹 로드
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_17");

        objectPosition = gameObject.GetComponent<Target>().objectPosition;
        Vector3 spawnPosition = objectPosition;
        Debug.Log(objectPosition);

        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
        effectInstance.tag = gameObject.GetComponent<Target>().tag;


    }

    public void ActivateEffect2(GameObject target)
    {
        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        // 프리팹 로드
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_17");

        Debug.Log(target);

        // 타겟의 위치에 VFX 생성
        Vector3 spawnPosition = target.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
        if (spawnPosition.x < -1)
        {
            effectInstance.tag = "opp_mon1";
        }
        else if (Mathf.Approximately(spawnPosition.x, 0)) // X 좌표가 0인 경우
        {
            effectInstance.tag = "opp_mon2";
        }
        else if (spawnPosition.x > 1)
        {
            effectInstance.tag = "opp_mon3";
        }
    }

}