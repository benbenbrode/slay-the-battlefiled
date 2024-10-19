using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card1 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;
    private int a = 5;

    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
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
        a = me.GetComponent<PlayerState>().atk + 5;
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "������ " + a + "��\n���غο�";
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
        eff.text = "������ " + a + "��\n���غο�";
        if (outline == null)
        {
            return; // Outline ������Ʈ�� ������ ������Ʈ ���� ����
        }

        // PlayerState ������Ʈ���� cost ���� ������ Ȯ��
        if (me.GetComponent<PlayerState>().cost >= 1)
        {
            // cost�� 1 �̻��� �� �׵θ� ������ �ʷϻ����� ����
            outline.effectColor = glowColor;
        }
        else
        {
            // cost�� 1���� ���� �� �׵θ��� �����ϰ� ����
            outline.effectColor = Color.clear;
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
            Debug.Log(targetTag);
            drop = GameObject.FindWithTag(Swap(targetTag)); // �ش� �±׸� ���� ������Ʈ�� ã��
        }

        // drop�� ã�� ������Ʈ�� ������ ActivateEffect ȣ��
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
        if (target.GetComponent<Target>().opcker == true)
        {
            a = me.GetComponent<PlayerState>().atk + 5;
        }
        else
        {
            a = opp.GetComponent<PlayerState>().atk + 5;
        }

        // PlayerState ������Ʈ�� �ִ��� Ȯ��
        PlayerState playerState = target.GetComponent<PlayerState>();
        if (playerState != null)
        {
            // PlayerState�� ���� ��� ����
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
            // PlayerState�� ������ monstate�� Ȯ��
            monstate monsterState = target.GetComponent<monstate>();
            if (monsterState != null)
            {
                // monstate�� ���� ��� ����
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
                Debug.LogError("Target does not have PlayerState or monstate.");
            }
        }

        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_1");

        // Ÿ���� ��ġ�� VFX ����
        Vector3 spawnPosition = target.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
    }

    string Swap(string input)
    {
        // "me"�� "ally"�θ� �ٲٴ� ����
        if (input.Contains("me"))
        {
            input = input.Replace("me", "ally");
        }

        // ���� ġȯ �߰�: 6�� 3, 5�� 2, 4�� 1
        input = input.Replace('6', '3');
        input = input.Replace('5', '2');
        input = input.Replace('4', '1');

        return input;
    }

}

