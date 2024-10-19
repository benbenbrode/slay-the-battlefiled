using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card17 : MonoBehaviour
{
    public Vector3 objectPosition;
    public Text eff;
    public GameObject drop;

    public Outline outline; // Outline ������Ʈ
    public Color glowColor = Color.green; // �׵θ��� �ʷϻ����� ������ ����
    public GameObject me;

    private void Start()
    {
        me = GameObject.Find("Canvas/me_drop");
        Transform effTransform = transform.Find("eff");
        Transform effTransform2 = transform.Find("cost_txt");
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();

            eff.text = "�������ȯ";
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
        grandChildText.text = "4";
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
        if (me.GetComponent<PlayerState>().cost >= 4)
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
        Debug.Log(gameObject.GetComponent<Target>().drop);
        Debug.Log(gameObject.GetComponent<Target>().tag);
        if (gameObject.GetComponent<Target>().tag == "ally_mon3" || gameObject.GetComponent<Target>().tag == "ally_mon2" || gameObject.GetComponent<Target>().tag == "ally_mon1")
        {
            ActivateEffect();
        }
        else
        {
            ActivateEffect2(drop);
        }


    }

    public void ActivateEffect()
    {
        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_17");

        objectPosition = gameObject.GetComponent<Target>().objectPosition;
        Vector3 spawnPosition = objectPosition;
        Debug.Log(objectPosition);

        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
        effectInstance.tag = gameObject.GetComponent<Target>().tag;


    }

    public void ActivateEffect2(GameObject target)
    {
        // Canvas ã��
        GameObject canvasObject = GameObject.Find("Canvas");

        // ������ �ε�
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_17");

        Debug.Log(target);

        // Ÿ���� ��ġ�� VFX ����
        Vector3 spawnPosition = target.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
        if (spawnPosition.x < -1)
        {
            effectInstance.tag = "opp_mon1";
        }
        else if (Mathf.Approximately(spawnPosition.x, 0)) // X ��ǥ�� 0�� ���
        {
            effectInstance.tag = "opp_mon2";
        }
        else if (spawnPosition.x > 1)
        {
            effectInstance.tag = "opp_mon3";
        }
    }

}