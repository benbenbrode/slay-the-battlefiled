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
    public GameObject canvas; // 캔버스 오브젝트를 받아옵니다.

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
        // fireeff 오브젝트를 캔버스 안에 생성
        GameObject effect = Instantiate(fireeff, canvas.transform);  // 캔버스를 부모로 설정

        // fireeff의 위치를 이 스크립트가 달린 게임 오브젝트의 위치로 설정 (월드 좌표 기준)
        effect.transform.position = transform.position;  // 스크립트가 달린 게임 오브젝트의 위치

        // 0.1초 대기
        yield return new WaitForSeconds(0.5f);

        // fireeff 오브젝트 삭제
        Destroy(effect);

        // fire 데미지 적용
        hp = hp - fire;
        fire = 0;
    }

    IEnumerator HandlepoisonEffect()
    {
        GameObject effect = Instantiate(poieff, canvas.transform);  // 캔버스를 부모로 설정

        effect.transform.position = transform.position;  // 스크립트가 달린 게임 오브젝트의 위치

        // 0.1초 대기
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
