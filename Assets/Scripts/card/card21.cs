using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card21 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;
    private int a = 5;

    public Outline outline; // Outline 컴포넌트
    public Color glowColor = Color.green; // 테두리가 초록색으로 빛나는 색상
    public GameObject battle;
    public GameObject me;
    public GameObject opp;
    private Transform effTransform;
    private Transform effTransform2;

    private void Start()
    {
        battle = GameObject.Find("battlemgr");
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        a = me.GetComponent<PlayerState>().atk;
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "적에게 죽은 몬스터\n수 *3의 피해부여";
        }

        Transform child = transform.Find("cost");
        if (child == null)
        {
            Debug.Log("Child 'cost' not found under this GameObject.");
            return;
        }

        Text grandChildText = child.GetComponentInChildren<Text>();
        if (grandChildText == null)
        {
            Debug.Log("Text component not found in children of 'cost'.");
            return;
        }
        grandChildText.text = "1";

        outline = GetComponent<Outline>();

        if (outline == null)
        {
            Debug.LogError("Outline component not found on this GameObject.");
        }
    }
    void Update()
    {
       
        eff.text = "죽은 몬스터 수*3\n의 피해를줍니다.";
        if (outline == null)
        {
            return; // Outline 컴포넌트가 없으면 업데이트 하지 않음
        }

        // PlayerState 컴포넌트에서 cost 값을 가져와 확인
        if (me.GetComponent<PlayerState>().cost >= 1)
        {
            // cost가 1 이상일 때 테두리 색상을 초록색으로 설정
            outline.effectColor = glowColor;
        }
        else
        {
            // cost가 1보다 작을 때 테두리를 투명하게 설정
            outline.effectColor = Color.clear;
        }
    }

    void OnDestroy()
    {
        if (gameObject.GetComponent<Target>().drop == "opp_drop" || gameObject.GetComponent<Target>().drop == "me_drop")
        {
            // PlayerState 스크립트를 가지고 있을 때
            drop = GameObject.Find(gameObject.GetComponent<Target>().drop);
        }
        else
        {
            // monstate 스크립트를 가지고 있을 때
            string targetTag = gameObject.GetComponent<Target>().drop; // drop 필드에 있는 값이 태그라고 가정
            Debug.Log(targetTag);
            drop = GameObject.FindWithTag(Swap(targetTag)); // 해당 태그를 가진 오브젝트를 찾음
        }

        // drop에 찾은 오브젝트가 있으면 ActivateEffect 호출
        if (drop != null)
        {
            ActivateEffect(drop);
        }
        else
        {
            Debug.LogError("Drop object not found!");
            battle.GetComponent<battlemgr>().applycker = false;
        }
    }


    public void ActivateEffect(GameObject target)
    {
        Debug.Log(battle.GetComponent<battlemgr>().monkillcount);
        if (target.GetComponent<Target>().opcker == true)
        {
            a = me.GetComponent<PlayerState>().atk;
        }
        else
        {
            a = opp.GetComponent<PlayerState>().atk;
        }
        // PlayerState 컴포넌트가 있는지 확인
        PlayerState playerState = target.GetComponent<PlayerState>();
        if (playerState != null)
        {
            // PlayerState가 있을 경우 실행
            if (playerState.shield > 0)
            {
                playerState.shield -= a + battle.GetComponent<battlemgr>().monkillcount *3;
            }
            else
            {
                playerState.hp -= a + battle.GetComponent<battlemgr>().monkillcount * 3;
            }
        }
        else
        {
            // PlayerState가 없으면 monstate를 확인
            monstate monsterState = target.GetComponent<monstate>();
            if (monsterState != null)
            {
                // monstate가 있을 경우 실행
                if (monsterState.shield > 0)
                {
                    monsterState.shield -= a + battle.GetComponent<battlemgr>().monkillcount * 3;
                }
                else
                {
                    monsterState.hp -= a + battle.GetComponent<battlemgr>().monkillcount * 3;
                }
            }
            else
            {
                Debug.LogError("Target does not have PlayerState or monstate.");
            }
        }

        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        // 프리팹 로드
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_21");

        // 타겟의 위치에 VFX 생성
        Vector3 spawnPosition = target.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
    }

    string Swap(string input)
    {
        // "me"를 "ally"로만 바꾸는 로직
        if (input.Contains("me"))
        {
            input = input.Replace("me", "ally");
        }

        // 숫자 치환 추가: 6은 3, 5는 2, 4는 1
        input = input.Replace('6', '3');
        input = input.Replace('5', '2');
        input = input.Replace('4', '1');

        return input;
    }

}

