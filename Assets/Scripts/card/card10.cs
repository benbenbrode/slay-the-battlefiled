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

    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
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

            eff.text = "�� ��ü���� " + a + "\n�� ���غο�";
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

        eff.text = "�� ��ü���� " + a + "\n�� ���غο�";
        if (outline == null)
        {
            return; // Outline ������Ʈ�� ������ ������Ʈ ���� ����
        }
        if (me.GetComponent<PlayerState>().cost >= 3)
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
        // �±� ����Ʈ ����
        string[] tags = { "opp_player", "opp_mon1", "opp_mon2", "opp_mon3" };

        // �� �±׿� ���� ������Ʈ �˻�
        foreach (string tag in tags)
        {
            GameObject target = GameObject.FindWithTag(tag);
            if (target != null)
            {
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
                        Debug.LogError(tag + " ������Ʈ�� PlayerState�� monstate ������Ʈ�� �������� �ʽ��ϴ�.");
                    }
                }
            }
            else
            {
                Debug.Log(tag + " �±׸� ���� ������Ʈ�� �������� �ʽ��ϴ�.");
            }
        }
        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_10");

        // Ÿ���� ��ġ�� VFX ����
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
        // �±� ����Ʈ ����
        string[] tags = { "ally_player", "ally_mon1", "ally_mon2", "ally_mon3" };

        // �� �±׿� ���� ������Ʈ �˻�
        foreach (string tag in tags)
        {
            GameObject target = GameObject.FindWithTag(tag);
            if (target != null)
            {
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
                        Debug.LogError(tag + " ������Ʈ�� PlayerState�� monstate ������Ʈ�� �������� �ʽ��ϴ�.");
                    }
                }
            }
            else
            {
                Debug.Log(tag + " �±׸� ���� ������Ʈ�� �������� �ʽ��ϴ�.");
            }
        }
        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_10");

        // Ÿ���� ��ġ�� VFX ����
        Vector3 spawnPosition = postion2.transform.position;
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


