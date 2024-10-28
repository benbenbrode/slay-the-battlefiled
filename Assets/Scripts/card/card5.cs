using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class card5 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;
    public Outline outline; // Outline 컴포넌트
    public Color glowColor = Color.green; // 테두리가 초록색으로 빛나는 색상
    public GameObject me;
    public GameObject battle;
    public GameObject mgr;
    private void Start()
    {
        mgr = GameObject.Find("mgr");
        me = GameObject.Find("Canvas/me_drop");
        battle = GameObject.Find("battlemgr");
        Transform effTransform = transform.Find("eff");
        Transform effTransform2 = transform.Find("cost_txt");
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
        grandChildText.text = "1";
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
        if (me.GetComponent<PlayerState>().cost >= 1)
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
        ActivateEffect(drop);
    }
    public void ActivateEffect(GameObject target)
    {
        if (target == null)
        {
            Debug.LogError("ActivateEffect: target이 null입니다.");
            return;
        }
        target.GetComponent<PlayerState>().agility += 1;
        Debug.Log("민1업");

        mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(5);
        battle.GetComponent<battlemgr>().applycker = false;

    }


}

