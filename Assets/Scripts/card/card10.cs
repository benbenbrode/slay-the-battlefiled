using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class card10 : MonoBehaviour
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

    public GameObject postion1;
    public GameObject postion2;
    private void Start()
    {
        battle = GameObject.Find("battlemgr");
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        postion1 = GameObject.Find("Canvas/opp_drop");
        postion2 = GameObject.Find("Canvas/center");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        a = me.GetComponent<PlayerState>().atk + 5;

        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "적 전체에게 " + a + "\n의 피해부여";
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
        grandChildText.text = "2";

        outline = GetComponent<Outline>();

        if (outline == null)
        {
            Debug.LogError("Outline component not found on this GameObject.");
        }
    }
    void Update()
    {

        eff.text = "적 전체에게 " + a + "\n의 피해부여";
        if (outline == null)
        {
            return; // Outline 컴포넌트가 없으면 업데이트 하지 않음
        }
        if (me.GetComponent<PlayerState>().cost >= 3)
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
        if (gameObject.GetComponent<Target>().drop == "opp_drop")
        {
            ActivateEffect(opp);
        }
        else if (gameObject.GetComponent<Target>().drop == "me_drop")
        {
            ActivateEffect2(me);
        }

    }


    public void ActivateEffect(GameObject target2)
    {

        if (target2.GetComponent<Target>().opcker == true)
        {
            a = me.GetComponent<PlayerState>().atk + 5;
        }
        else
        {
            a = opp.GetComponent<PlayerState>().atk + 5;
        }
        // 태그 리스트 정의
        string[] tags = { "opp_player", "opp_mon1", "opp_mon2", "opp_mon3" };

        // 각 태그에 대해 오브젝트 검사
        foreach (string tag in tags)
        {
            GameObject target = GameObject.FindWithTag(tag);
            if (target != null)
            {
                // PlayerState 컴포넌트가 있는지 확인
                PlayerState playerState = target.GetComponent<PlayerState>();
                if (playerState != null)
                {
                    // PlayerState가 있을 경우 실행
                    if (playerState.shield > 0)
                    {
                        playerState.shield -= a;
                    }
                    else
                    {
                        playerState.hp -= a;
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
                            monsterState.shield -= a;
                        }
                        else
                        {
                            monsterState.hp -= a;
                        }
                    }
                    else
                    {
                        Debug.LogError(tag + " 오브젝트에 PlayerState나 monstate 컴포넌트가 존재하지 않습니다.");
                    }
                }
            }
            else
            {
                Debug.Log(tag + " 태그를 가진 오브젝트가 존재하지 않습니다.");
            }
        }
        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        // 프리팹 로드
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_10");

        // 타겟의 위치에 VFX 생성
        Vector3 spawnPosition = postion1.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
    }

    public void ActivateEffect2(GameObject target2)
    {

        if (target2.GetComponent<Target>().opcker == true)
        {
            a = me.GetComponent<PlayerState>().atk + 5;
        }
        else
        {
            a = opp.GetComponent<PlayerState>().atk + 5;
        }
        // 태그 리스트 정의
        string[] tags = { "ally_player", "ally_mon1", "ally_mon2", "ally_mon3" };

        // 각 태그에 대해 오브젝트 검사
        foreach (string tag in tags)
        {
            GameObject target = GameObject.FindWithTag(tag);
            if (target != null)
            {
                // PlayerState 컴포넌트가 있는지 확인
                PlayerState playerState = target.GetComponent<PlayerState>();
                if (playerState != null)
                {
                    // PlayerState가 있을 경우 실행
                    if (playerState.shield > 0)
                    {
                        playerState.shield -= a;
                    }
                    else
                    {
                        playerState.hp -= a;
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
                            monsterState.shield -= a;
                        }
                        else
                        {
                            monsterState.hp -= a;
                        }
                    }
                    else
                    {
                        Debug.LogError(tag + " 오브젝트에 PlayerState나 monstate 컴포넌트가 존재하지 않습니다.");
                    }
                }
            }
            else
            {
                Debug.Log(tag + " 태그를 가진 오브젝트가 존재하지 않습니다.");
            }
        }
        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        // 프리팹 로드
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_10");

        // 타겟의 위치에 VFX 생성
        Vector3 spawnPosition = postion2.transform.position;
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


