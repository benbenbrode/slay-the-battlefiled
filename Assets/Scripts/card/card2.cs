using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class card2 : MonoBehaviour
{
    public GameObject drop;
    public Text eff;
    private int a = Global.armor + 5;

    public Outline outline; // Outline 컴포넌트
    public Color glowColor = Color.green; // 테두리가 초록색으로 빛나는 색상
    public GameObject me;
    public GameObject opp;
    public GameObject mgr;
    private Transform effTransform;
    private Transform effTransform2;
    private void Start()
    {
        mgr = GameObject.Find("mgr");
        me = GameObject.Find("Canvas/me_drop");
        opp = GameObject.Find("Canvas/opp_drop");
        effTransform = transform.Find("eff");
        effTransform2 = transform.Find("cost_txt");
        a = me.GetComponent<PlayerState>().agility + 5;
        if (effTransform != null)
        {
            eff = effTransform.GetComponent<Text>();
            
            eff.text = "아군에게 "+a+"의\n방어도부여";
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
        eff.text = "아군에게 " + a + "의\n방어도부여";
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
        if (gameObject.GetComponent<Target>().drop == "opp_drop" || gameObject.GetComponent<Target>().drop == "me_drop")
        {
            // PlayerState 스크립트를 가지고 있을 때
            drop = GameObject.Find(gameObject.GetComponent<Target>().drop);
        }
        else
        {
            // monstate 스크립트를 가지고 있을 때
            string targetTag = gameObject.GetComponent<Target>().drop; // drop 필드에 있는 값이 태그라고 가정
            if(gameObject.GetComponent<Target>().opcker == true)
                drop = GameObject.FindWithTag(Swap(targetTag)); // 해당 태그를 가진 오브젝트를 찾음
            if (gameObject.GetComponent<Target>().opcker == false)
                drop = GameObject.FindWithTag(targetTag); // 해당 태그를 가진 오브젝트를 찾음
        }

        // drop에 찾은 오브젝트가 있으면 ActivateEffect 호출
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
            a = opp.GetComponent<PlayerState>().agility + 5;
        }
        else
        {
            a = me.GetComponent<PlayerState>().agility + 5;
        }
        if (target == null)
        {
            Debug.LogError("ActivateEffect: target이 null입니다.");
            return;
        }

        // PlayerState 컴포넌트가 있는지 확인하고, 있을 때만 가져옵니다.
        PlayerState playerState;
        if (target.TryGetComponent<PlayerState>(out playerState))
        {
            playerState.shield += a;
            Debug.Log("쉴드");
        }
        else
        {
            // PlayerState가 없을 경우 monstate를 가져옵니다.
            monstate monstate;
            if (target.TryGetComponent<monstate>(out monstate))
            {
                monstate.shield += a;
            }
            else
            {
                Debug.LogError("ActivateEffect: target에 PlayerState나 monstate 컴포넌트가 없습니다.");
                return;
            }
        }

        // Canvas 찾기
        GameObject canvasObject = GameObject.Find("Canvas");

        // 프리팹 로드
        GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_2");

        // 타겟의 위치에 VFX 생성
        Vector3 spawnPosition = target.transform.position;
        GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
        mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(4);
    }

    string Swap(string input)
    {
        // "me"를 "ally"로만 바꾸는 로직
        if (input.Contains("ally"))
        {
            input = input.Replace("ally", "opp");
        }

        // 숫자 치환 추가: 6은 3, 5는 2, 4는 1
        input = input.Replace('6', '3');
        input = input.Replace('5', '2');
        input = input.Replace('4', '1');

        return input;
    }
}

