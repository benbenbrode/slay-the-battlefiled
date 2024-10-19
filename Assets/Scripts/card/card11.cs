using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class card11 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;
    private int a = Global.armor + 5;

    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
    public GameObject me;
    public GameObject opp;
    private Transform effTransform;
    private Transform effTransform2;
    private void Start()
    {
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        a = me.GetComponent<PlayerState>().agility + 7;
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "�Ʊ�����" + a + "�� ���\n���ο�, ��뿡��\nȭ��3 �ο�";
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
        grandChildText.text = "2";
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline component not found on this GameObject.");
        }

    }

    void Update()
    {

        eff.text = "�Ʊ�����" + a + "�� ���\n���ο�, ��뿡��\nȭ��3 �ο�";
        if (outline == null)
        {
            return; // Outline ������Ʈ�� ������ ������Ʈ ���� ����
        }

        // cost�� 1 �̻��� �� �׵θ� ������ �ʷϻ����� ����
        if (me.GetComponent<PlayerState>().cost >= 2)
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
        if (target.GetComponent<Target>().opcker == true)
        {
            a = opp.GetComponent<PlayerState>().agility + 7;
        }
        else
        {
            a = me.GetComponent<PlayerState>().agility + 7;
        }
        if (target == null)
        {
            Debug.LogError("ActivateEffect: target�� null�Դϴ�.");
            return;
        }

        // PlayerState ������Ʈ�� �ִ��� Ȯ���ϰ�, ���� ���� �����ɴϴ�.
        PlayerState playerState;
        if (target.TryGetComponent<PlayerState>(out playerState))
        {
            playerState.shield += a;
            Debug.Log("����");
        }
        else
        {
            // PlayerState�� ���� ��� monstate�� �����ɴϴ�.
            monstate monstate;
            if (target.TryGetComponent<monstate>(out monstate))
            {
                monstate.shield += a;
            }
            else
            {
                Debug.LogError("ActivateEffect: target�� PlayerState�� monstate ������Ʈ�� �����ϴ�.");
                return;
            }
        }

        if (target.tag.Contains("opp"))
            me.GetComponent<PlayerState>().fire += 3;
        else
            opp.GetComponent<PlayerState>().fire += 3;

        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_11");

        // Ÿ���� ��ġ�� VFX ����
        Vector3 spawnPosition = target.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
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
