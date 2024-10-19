using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class card27 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;

    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
    public GameObject me;
    private Transform effTransform;
    private Transform effTransform2;
    public GameObject opp;
    public GameObject battle;
    private void Start()
    {
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        battle = GameObject.Find("battlemgr");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "�ڽ��ǰ��ݷ�\n1������ ��ø��\n2���";
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
        eff.text = "�ڽ��ǰ��ݷ�\n1������ ��ø��\n2���";
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
        if (gameObject.GetComponent<Target>().drop == "opp_drop" || gameObject.GetComponent<Target>().drop == "me_drop")
        {
            // PlayerState ��ũ��Ʈ�� ������ ���� ��
            drop = GameObject.Find(gameObject.GetComponent<Target>().drop);
        }
        else
        {
            // monstate ��ũ��Ʈ�� ������ ���� ��
            string targetTag = gameObject.GetComponent<Target>().drop; // drop �ʵ忡 �ִ� ���� �±׶�� ����
            if (gameObject.GetComponent<Target>().opcker == true)
                drop = GameObject.FindWithTag(Swap(targetTag)); // �ش� �±׸� ���� ������Ʈ�� ã��
            if (gameObject.GetComponent<Target>().opcker == false)
                drop = GameObject.FindWithTag(targetTag); // �ش� �±׸� ���� ������Ʈ�� ã��
        }

        // drop�� ã�� ������Ʈ�� ������ ActivateEffect ȣ��
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
        if (target == null)
        {
            Debug.LogError("ActivateEffect: target�� null�Դϴ�.");
            return;
        }

        if (target.tag.Contains("opp"))
        {
            opp.GetComponent<PlayerState>().atk -= 1;
            opp.GetComponent<PlayerState>().agility += 2;
        }
        else
        {
            me.GetComponent<PlayerState>().atk -= 1;
            me.GetComponent<PlayerState>().agility += 2;
        }

        battle.GetComponent<battlemgr>().applycker = false;

    }

    string Swap(string input)
    {
        // "me"�� "ally"�θ� �ٲٴ� ����
        if (input.Contains("ally"))
        {
            input = input.Replace("ally", "opp");
        }

        // ���� ġȯ �߰�: 6�� 3, 5�� 2, 4�� 1
        input = input.Replace('6', '3');
        input = input.Replace('5', '2');
        input = input.Replace('4', '1');

        return input;
    }
}
