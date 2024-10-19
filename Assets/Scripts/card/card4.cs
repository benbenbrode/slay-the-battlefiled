using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class card4 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;
    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
    public GameObject me;
    public GameObject battle;
    private void Start()
    {
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
            return; // Outline ������Ʈ�� ������ ������Ʈ ���� ����
        }

        // cost�� 1 �̻��� �� �׵θ� ������ �ʷϻ����� ����
        if (me.GetComponent<PlayerState>().cost >= 1)
        {
            outline.effectColor = glowColor;
        }
        else
        {
            // cost�� 1 �̸��� �� �׵θ� ������ ���� �������� �����ϰų� �׵θ��� �� �� ����
            outline.effectColor = Color.clear; // �׵θ��� ������ �ʰ� ���� (�Ǵ� ���� �������� ���� ����)
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
            Debug.LogError("ActivateEffect: target�� null�Դϴ�.");
            return;
        }
        target.GetComponent<PlayerState>().atk += 1;
        Debug.Log("��1��");
        

        battle.GetComponent<battlemgr>().applycker = false;

    }


}

