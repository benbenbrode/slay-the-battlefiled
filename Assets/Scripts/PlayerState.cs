using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public int hp = 30;
    public int shield = 0;
    public int cost = 3;
    public int maxcost = 3;
    public int tempcost = 0;
    public int atk = 0;
    public int agility = 0;
    public int fire = 0;
    public int poison = 0;
    public bool endcker = false;
    private int previousHp;
    public bool protect = false;
    public Text hp_text;
    public Text shild_text;
    public Sprite me_img;
    public Sprite opp_img;
    public GameObject mgr;
    public GameObject me_obj;
    public GameObject opp_obj;
    public GameObject battle;
    public GameObject fireeff;
    public GameObject poieff;
    public GameObject canvas; // ĵ���� ������Ʈ�� �޾ƿɴϴ�.

    private void Start()
    {
        previousHp = hp;
    }

    private void Update()
    {
        if (shield < 0)
        {
            hp += shield;
            shield = 0;
        }
        if (hp != previousHp && protect == true)
        {
            hp = previousHp;
            protect = false;
        }


        hp_text.text = hp.ToString();
        shild_text.text = shield.ToString();

        if (battle.GetComponent<battlemgr>().turnend == true && endcker == false)
        {
            if (fire > 0)
            {
                StartCoroutine(HandleFireEffect());
            }
            if(poison > 0)
            {
                StartCoroutine(HandlepoisonEffect());
            }
            endcker = true;
        }
        if (battle.GetComponent<battlemgr>().turnend == false)
        {
            endcker = false;
        }
    }

    IEnumerator HandleFireEffect()
    {
        // fireeff ������Ʈ�� ĵ���� �ȿ� ����
        GameObject effect = Instantiate(fireeff, canvas.transform);  // ĵ������ �θ�� ����

        // fireeff�� ��ġ�� �� ��ũ��Ʈ�� �޸� ���� ������Ʈ�� ��ġ�� ���� (���� ��ǥ ����)
        effect.transform.position = transform.position;  // ��ũ��Ʈ�� �޸� ���� ������Ʈ�� ��ġ

        // 0.1�� ���
        yield return new WaitForSeconds(0.5f);

        // fireeff ������Ʈ ����
        Destroy(effect);

        // fire ������ ����
        hp = hp - fire;
        fire = 0;
    }

    IEnumerator HandlepoisonEffect()
    {
        GameObject effect = Instantiate(poieff, canvas.transform);  // ĵ������ �θ�� ����

        effect.transform.position = transform.position;  // ��ũ��Ʈ�� �޸� ���� ������Ʈ�� ��ġ

        // 0.1�� ���
        yield return new WaitForSeconds(0.5f);

        Destroy(effect);
        if(shield > 0)
        {
            shield -= poison;
        }
        else 
        {
            hp -= poison;
        }
        poison -= 1;
    }
}
