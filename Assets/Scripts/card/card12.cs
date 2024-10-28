using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class card12 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;

    public Outline outline; // Outline 컴포넌트
    public Color glowColor = Color.green; // 테두리가 초록색으로 빛나는 색상
    public GameObject me;
    public GameObject opp;
    public GameObject mgr;
    private Transform effTransform;
    private Transform effTransform2;
    public GameObject battle;
    private void Start()
    {
        mgr = GameObject.Find("mgr");
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        battle = GameObject.Find("battlemgr");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();
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
        grandChildText.text = "0";
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline component not found on this GameObject.");
        }

    }


    void Update()
    {
        eff.text = "다음턴에 자신의\n마나1증가";
        if (outline == null)
        {
            return; // Outline 컴포넌트가 없으면 업데이트 하지 않음
        }

        // cost가 1 이상일 때 테두리 색상을 초록색으로 설정
        if (me.GetComponent<PlayerState>().cost >= 0)
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
        if (gameObject.GetComponent<Target>().drop == "opp_drop" || gameObject.GetComponent<Target>().drop == "me_drop")
        {
            // PlayerState 스크립트를 가지고 있을 때
            drop = GameObject.Find(gameObject.GetComponent<Target>().drop);
        }
        else
        {
            // monstate 스크립트를 가지고 있을 때
            string targetTag = gameObject.GetComponent<Target>().drop; // drop 필드에 있는 값이 태그라고 가정
            if (gameObject.GetComponent<Target>().opcker == true)
                drop = GameObject.FindWithTag(Swap(targetTag)); // 해당 태그를 가진 오브젝트를 찾음
            if (gameObject.GetComponent<Target>().opcker == false)
                drop = GameObject.FindWithTag(targetTag); // 해당 태그를 가진 오브젝트를 찾음
        }

        // drop에 찾은 오브젝트가 있으면 ActivateEffect 호출
        if (drop != null)
        {
            ActivateEffect(drop);
        }
        else
        {
            Debug.LogError("Drop object not found!");
        }
    }
    public void ActivateEffect(GameObject target)
    {
        if (target.GetComponent<Target>().opcker == true)
            opp.GetComponent<PlayerState>().tempcost += 1;
        else
            me.GetComponent<PlayerState>().tempcost += 1;

        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        battle.GetComponent<battlemgr>().applycker = false;
        mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(10);
    }

    string Swap(string input)
    {
        // "me"를 "ally"로만 바꾸는 로직
        if (input.Contains("ally"))
        {
            input = input.Replace("ally", "opp");
        }

        // 숫자 치환 추가: 6은 3, 5는 2, 4는 1
        input = input.Replace('6', '3');
        input = input.Replace('5', '2');
        input = input.Replace('4', '1');

        return input;
    }
}

