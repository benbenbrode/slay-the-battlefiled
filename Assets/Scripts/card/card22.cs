using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card22 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;

    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
    public GameObject battle;
    public GameObject me;
    public GameObject opp;
    private Transform effTransform;
    private Transform effTransform2;
    public GameObject mgr;
    private void Start()
    {
        mgr = GameObject.Find("mgr");
        battle = GameObject.Find("battlemgr");
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "����� �������\n�������� ����";
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
        eff.text = "����� �������\n�������� ����";
        if (outline == null)
        {
            return; // Outline ������Ʈ�� ������ ������Ʈ ���� ����
        }

        // PlayerState ������Ʈ���� cost ���� ������ Ȯ��
        if (me.GetComponent<PlayerState>().cost >= 2)
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
        // PlayerState ������Ʈ�� �ִ��� Ȯ��
        PlayerState playerState = target.GetComponent<PlayerState>();
        if (playerState != null)
        {
            target.GetComponent<PlayerState>().hp = target.GetComponent<PlayerState>().hp / 2;
        }
      

        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_22");

        // Ÿ���� ��ġ�� VFX ����
        Vector3 spawnPosition = target.transform.position;
        spawnPosition = new Vector3(spawnPosition.x, target.transform.position.y - 3, spawnPosition.z);
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
        mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(3);
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

