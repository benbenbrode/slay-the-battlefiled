using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class battlemgr : MonoBehaviourPunCallbacks
{
    public GameObject mgr;
    public List<GameObject> attack = new List<GameObject>();
    public List<GameObject> skill = new List<GameObject>();
    public List<GameObject> mon = new List<GameObject>();
    public List<GameObject> attack2 = new List<GameObject>();
    public List<GameObject> skill2 = new List<GameObject>();
    public List<GameObject> mon2 = new List<GameObject>();
    public string[] target_atk;
    public string[] target_skill;
    public string[] target_mon;
    private bool isConditionMet = false;
    private int callCount = 0;


    public Button targetButton; // 변경할 버튼
    public Sprite originalSprite; // 원래 스프라이트
    public Sprite pressedSprite; // 눌렀을 때의 스프라이트
    private bool isButtonPressed = false;

    public bool applycker = false;
    public GameObject me;
    public GameObject opp;

    public bool[] poscker;
    public GameObject[] pos;
    public GameObject cardView;
    public GameObject cardView2;

    public bool turnend = false;
    public int monkillcount = 0;

    public GameObject net;

    public GameObject winpannel;
    public GameObject winbg;

    public GameObject losepannel;
    public GameObject losebg;

    public GameObject drawpannel;
    public GameObject drawbg;
    public GameObject drawbg2;

    public Sprite wwin;
    public Sprite swin;
    public Sprite wlose;
    public Sprite slose;
    private bool endcker = false; 
    void Start()
    {
        net = GameObject.Find("netmgr(Clone)");
        targetButton.onClick.AddListener(OnButtonPressed);
        targetButton.image.sprite = originalSprite;
    }

    void Update()
    {

        if(poscker[1] == false)
        {
            pos[1].SetActive(false);
        }
        if (poscker[1] == true)
        {
            pos[1].SetActive(true);
        }
        if (poscker[2] == false)
        {
            pos[2].SetActive(false);
        }
        if (poscker[2] == true)
        {
            pos[2].SetActive(true);
        }
        if (poscker[3] == false)
        {
            pos[3].SetActive(false);
        }
        if (poscker[3] == true)
        {
            pos[3].SetActive(true);
        }

        if (mgr.GetComponent<CardMgr>().mycurturn == turnstate.battle && mgr.GetComponent<CardMgr>().encurturn == turnstate.battle && !isConditionMet)
        {
            // attack 및 skill 리스트에서 이름 목록을 가져옵니다.
            string[] attackNames = GetObjectNames(attack);
            string[] skillNames = GetObjectNames2(skill);
            string[] monNames = GetObjectNames3(mon);

            // 다른 플레이어에게 공격 및 스킬 리스트를 전송
            photonView.RPC("SendListsToOtherPlayer", RpcTarget.Others, attackNames, skillNames, monNames, target_atk, target_skill, target_mon);

            // Coroutine을 통해 리스트 전송 후 제거 실행
            StartCoroutine(StartRemoveObjectsWithDelay());
            isConditionMet = true;
        }
    }



    IEnumerator CallResetButtonWithDelay() // 엔드페이즈
    {
        // 1초 대기
        turnend = true;
        yield return new WaitForSeconds(1f);
        // 특정 코루틴을 먼저 호출하고 해당 코루틴이 끝날 때까지 대기합니다.
        yield return StartCoroutine(InvokeEffCoroutineOnTaggedObjects()); // 
        Debug.Log("턴끝 리셋 불러와");
        // 플레이어 상태의 코스트를 리셋
        me.GetComponent<PlayerState>().cost = me.GetComponent<PlayerState>().maxcost + me.GetComponent<PlayerState>().tempcost;
        me.GetComponent<PlayerState>().tempcost = 0;
        turnend = false;
        int Myhp = me.GetComponent<PlayerState>().hp;
        monstate[] allMonstates = FindObjectsOfType<monstate>();

        // monstate가 하나도 없는 경우 바로 ResetButton 호출
        if (allMonstates.Length == 0)
        {
            if (me.GetComponent<PlayerState>().hp < 1 && opp.GetComponent<PlayerState>().hp < 1)
            {
   
                drawpannel.SetActive(true);
                if (net.GetComponent<Network>().type2 == 1)
                {
                    drawbg.GetComponent<Image>().sprite = wlose;
                }
                else if (net.GetComponent<Network>().type2 == 2) 
                {
                    drawbg.GetComponent<Image>().sprite = slose;
                }
                if (net.GetComponent<Network>().type == 1)
                {
                    drawbg2.GetComponent<Image>().sprite = wlose;
                }
                else if (net.GetComponent<Network>().type == 2)
                {
                    drawbg2.GetComponent<Image>().sprite = slose;
                }
                endcker = true;
            }
            else if (me.GetComponent<PlayerState>().hp < 1) 
            {

                losepannel.SetActive(true);
                if (net.GetComponent<Network>().type == 1)
                {
                    losebg.GetComponent<Image>().sprite = wlose;
                }
                else if (net.GetComponent<Network>().type == 2)
                {
                    losebg.GetComponent<Image>().sprite = slose;
                }
                endcker = true;
            }
            else if (opp.GetComponent<PlayerState>().hp < 1)
            {

                winpannel.SetActive(true);
                if (net.GetComponent<Network>().type == 1)
                {
                    winbg.GetComponent<Image>().sprite = wwin;
                }
                else if (net.GetComponent<Network>().type == 2)
                {
                    winbg.GetComponent<Image>().sprite = swin;
                }
                endcker = true;
            }
            else if(me.GetComponent<PlayerState>().hp > 0 && opp.GetComponent<PlayerState>().hp > 0 )
            {
                photonView.RPC("ResetButton", RpcTarget.All);
            }
        
            yield break;
        }
        while (!AllMonstatesAnimackerTrue(allMonstates))
        {
            yield return null; // 한 프레임 대기
        }

        if (me.GetComponent<PlayerState>().hp < 1 && opp.GetComponent<PlayerState>().hp < 1)
        {

            drawpannel.SetActive(true);
            if (net.GetComponent<Network>().type2 == 1)
            {
                drawbg.GetComponent<Image>().sprite = wlose;
            }
            else if (net.GetComponent<Network>().type2 == 2)
            {
                drawbg.GetComponent<Image>().sprite = slose;
            }
            if (net.GetComponent<Network>().type == 1)
            {
                drawbg2.GetComponent<Image>().sprite = wlose;
            }
            else if (net.GetComponent<Network>().type == 2)
            {
                drawbg2.GetComponent<Image>().sprite = slose;
            }
            endcker = true;
        }
        else if (me.GetComponent<PlayerState>().hp < 1)
        {

            losepannel.SetActive(true);
            if (net.GetComponent<Network>().type == 1)
            {
                losebg.GetComponent<Image>().sprite = wlose;
            }
            else if (net.GetComponent<Network>().type == 2)
            {
                losebg.GetComponent<Image>().sprite = slose;
            }
            endcker = true;
        }
        else if (opp.GetComponent<PlayerState>().hp < 1)
        {

            winpannel.SetActive(true);
            if (net.GetComponent<Network>().type == 1)
            {
                winbg.GetComponent<Image>().sprite = wwin;
            }
            else if (net.GetComponent<Network>().type == 2)
            {
                winbg.GetComponent<Image>().sprite = swin;
            }
            endcker = true;
        }
        else if (me.GetComponent<PlayerState>().hp > 0 && opp.GetComponent<PlayerState>().hp > 0)
        {
            photonView.RPC("ResetButton", RpcTarget.All);
        }
    }
    bool AllMonstatesAnimackerTrue(monstate[] monstates)
    {
        foreach (monstate mon in monstates)
        {
            if (!mon.animacker)
            {
                return false; // 하나라도 false면 false 반환
            }
        }
        return true; // 모두 true일 때 true 반환
    }

    public void win()
    {
        if (endcker == true)
            return;
        winpannel.SetActive(true);
        if (net.GetComponent<Network>().type == 1)
        {
            winbg.GetComponent<Image>().sprite = wwin;
        }
        else if (net.GetComponent<Network>().type == 2)
        {
            winbg.GetComponent<Image>().sprite = swin;
        }
    }
    public void deadck()
    {
        if (me.GetComponent<PlayerState>().hp < 1 && opp.GetComponent<PlayerState>().hp < 1)
        {

            drawpannel.SetActive(true);
            if (net.GetComponent<Network>().type2 == 1)
            {
                drawbg.GetComponent<Image>().sprite = wlose;
            }
            else if (net.GetComponent<Network>().type2 == 2)
            {
                drawbg.GetComponent<Image>().sprite = slose;
            }
            if (net.GetComponent<Network>().type == 1)
            {
                drawbg2.GetComponent<Image>().sprite = wlose;
            }
            else if (net.GetComponent<Network>().type == 2)
            {
                drawbg2.GetComponent<Image>().sprite = slose;
            }
        }
        else if (me.GetComponent<PlayerState>().hp < 1)
        {

            losepannel.SetActive(true);
            if (net.GetComponent<Network>().type == 1)
            {
                losebg.GetComponent<Image>().sprite = wlose;
            }
            else if (net.GetComponent<Network>().type == 2)
            {
                losebg.GetComponent<Image>().sprite = slose;
            }
        }
        else if (opp.GetComponent<PlayerState>().hp < 1)
        {

            winpannel.SetActive(true);
            if (net.GetComponent<Network>().type == 1)
            {
                winbg.GetComponent<Image>().sprite = wwin;
            }
            else if (net.GetComponent<Network>().type == 2)
            {
                winbg.GetComponent<Image>().sprite = swin;
            }
        }
    }
    IEnumerator InvokeEffCoroutineOnTaggedObjects()
    {
        // 모든 활성화된 게임 오브젝트를 가져옵니다.
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 오브젝트의 태그에 "_mon"이 포함되어 있는지 확인합니다.
            if (obj.tag.Contains("_mon"))
            {
                // 해당 오브젝트의 모든 MonoBehaviour 스크립트를 가져옵니다.
                MonoBehaviour[] monoBehaviours = obj.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour mb in monoBehaviours)
                {
                    // "eff" 코루틴을 가지고 있는지 확인합니다.
                    var methodInfo = mb.GetType().GetMethod("ApplyEffect", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (methodInfo != null && methodInfo.ReturnType == typeof(IEnumerator))
                    {
                        StartCoroutine((IEnumerator)methodInfo.Invoke(mb, null));
                    }
                }
            }
        }
        yield return null;
    }

    public void AddToAttack(GameObject obj)
    {
        attack.Add(obj);
    }

    public void AddToSkill(GameObject obj)
    {
        skill.Add(obj);
    }
    public void AddTomon(GameObject obj)
    {
        mon.Add(obj);
    }
    void AddToTargetatk(string newatk)
    {
        // target_skill 배열이 null이면 초기화
        if (target_atk == null)
        {
            target_atk = new string[1]; // 배열 크기를 1로 설정
            target_atk[0] = newatk;   // 첫 번째 요소에 새 스킬을 추가
        }
        else
        {
            // 기존 배열보다 크기가 1 더 큰 새 배열을 생성
            string[] newArray = new string[target_atk.Length + 1];

            // 기존 배열의 값을 새 배열로 복사
            for (int i = 0; i < target_atk.Length; i++)
            {
                newArray[i] = target_atk[i];
            }

            // 새로운 요소를 마지막에 추가
            newArray[newArray.Length - 1] = newatk;

            // 기존 배열을 새로운 배열로 교체
            target_atk = newArray;
        }
    }

    void AddToTargetSkill(string newSkill)
    {
        // target_skill 배열이 null이면 초기화
        if (target_skill == null)
        {
            target_skill = new string[1]; // 배열 크기를 1로 설정
            target_skill[0] = newSkill;   // 첫 번째 요소에 새 스킬을 추가
        }
        else
        {
            // 기존 배열보다 크기가 1 더 큰 새 배열을 생성
            string[] newArray = new string[target_skill.Length + 1];

            // 기존 배열의 값을 새 배열로 복사
            for (int i = 0; i < target_skill.Length; i++)
            {
                newArray[i] = target_skill[i];
            }

            // 새로운 요소를 마지막에 추가
            newArray[newArray.Length - 1] = newSkill;

            // 기존 배열을 새로운 배열로 교체
            target_skill = newArray;
        }
    }

    void AddToTargetmon(string newmon)
    {
        // target_skill 배열이 null이면 초기화
        if (target_mon == null)
        {
            target_mon = new string[1]; // 배열 크기를 1로 설정
            target_mon[0] = newmon;   // 첫 번째 요소에 새 스킬을 추가
        }
        else
        {
            // 기존 배열보다 크기가 1 더 큰 새 배열을 생성
            string[] newArray = new string[target_mon.Length + 1];

            // 기존 배열의 값을 새 배열로 복사
            for (int i = 0; i < target_mon.Length; i++)
            {
                newArray[i] = target_mon[i];
            }

            // 새로운 요소를 마지막에 추가
            newArray[newArray.Length - 1] = newmon;

            // 기존 배열을 새로운 배열로 교체
            target_mon = newArray;
        }
    }

    IEnumerator StartRemoveObjectsWithDelay()
    {
        // 2초 대기
        yield return new WaitForSeconds(2f);

        // RemoveObjectsWithDelay 코루틴 실행
        StartCoroutine(RemoveObjectsWithDelay());
    }

    IEnumerator RemoveObjectsWithDelay()
    {
        // applycker가 false일 때만 작동
        while ((skill.Count > 0 || attack.Count > 0) || (skill2.Count > 0 || attack2.Count > 0))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 플레이어의 차례: skill -> attack 순으로 삭제
                if (skill.Count > 0)
                {
                    GameObject firstSkillObject = skill[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation(firstSkillObject));
                    callCount++;
                    firstSkillObject.name = firstSkillObject.name + "_" + "kill" + callCount.ToString();
                    skill.RemoveAt(0);
                    Destroy(firstSkillObject);
                    applycker = true;
                }
                else if (attack.Count > 0)
                {
                    GameObject firstAttackObject = attack[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation(firstAttackObject));
                    callCount++;
                    firstAttackObject.name = firstAttackObject.name + "_" + "kill" + callCount.ToString();
                    attack.RemoveAt(0);
                    Destroy(firstAttackObject);
                    applycker = true;
                }

                while (applycker)
                {
                    yield return null;
                }

                // 상대방의 차례: skill2 -> attack2 순으로 삭제
                if (skill2.Count > 0)
                {
                    GameObject firstSkill2Object = skill2[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation2(firstSkill2Object));
                    skill2.RemoveAt(0);
                    Destroy(firstSkill2Object);
                    applycker = true;
                }
                else if (attack2.Count > 0)
                {
                    GameObject firstAttack2Object = attack2[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation2(firstAttack2Object));
                    attack2.RemoveAt(0);
                    Destroy(firstAttack2Object);
                    applycker = true;
                }
                // 애니메이션이 끝날 때까지 대기
                while (applycker)
                {
                    yield return null;
                }
            }
            else
            {
                // 상대방의 차례: skill2 -> attack2 순으로 삭제
                if (skill2.Count > 0)
                {
                    GameObject firstSkill2Object = skill2[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation2(firstSkill2Object));
                    skill2.RemoveAt(0);
                    Destroy(firstSkill2Object);
                    applycker = true;
                }
                else if (attack2.Count > 0)
                {
                    GameObject firstAttack2Object = attack2[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation2(firstAttack2Object));
                    attack2.RemoveAt(0);
                    Destroy(firstAttack2Object);
                    applycker = true;
                }
                while (applycker)
                {
                    yield return null;
                }

                // 플레이어의 차례: skill -> attack 순으로 삭제
                if (skill.Count > 0)
                {
                    GameObject firstSkillObject = skill[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation(firstSkillObject));
                    skill.RemoveAt(0);
                    Destroy(firstSkillObject);
                    applycker = true;
                }
                else if (attack.Count > 0)
                {
                    GameObject firstAttackObject = attack[0];
                    yield return StartCoroutine(PlayCardDestroyAnimation(firstAttackObject));
                    attack.RemoveAt(0);
                    Destroy(firstAttackObject);
                    applycker = true;
                }
                // 애니메이션이 끝날 때까지 대기
                while (applycker)
                {
                    yield return null;
                }
            }
        }
        for (int i = 0; i < mon.Count; i++)
        {
            if (mon[i] != null)
            {
                Destroy(mon[i]);  // 오브젝트 삭제
            }
        }
        Debug.Log(mon2.Count);
        for (int i = 0; i < mon2.Count; i++)
        {
            if (mon2[i] != null)
            {
                Destroy(mon2[i]);  // 오브젝트 삭제
                Debug.Log(mon2.Count+ "삭제");

            }
        }
        Debug.Log("skill, attack, skill2, attack2 리스트가 모두 비워졌습니다.");
        StartCoroutine(CallResetButtonWithDelay());
    }
    IEnumerator PlayCardDestroyAnimation(GameObject cardObject)
    {
        if (cardView != null)
        {
            Debug.Log("애니메이션 실행 중인 카드: " + cardObject.name);
            Debug.Log("cardView 위치: " + cardView.transform.position);

            // cardObject를 cardView의 위치로 이동
            cardObject.transform.position = cardView.transform.position;
            Debug.Log("카드가 cardView 위치로 이동함");

            // 카드 오브젝트가 0에서 2까지 스케일 증가
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = new Vector3(2, 2, 2);
            float duration = 0.5f;
            float time = 0f;

            // 0.5초 동안 스케일 변화
            while (time < duration)
            {
                cardObject.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            // 최종 스케일 적용
            cardObject.transform.localScale = endScale;
            Debug.Log("카드 스케일 애니메이션 완료");

            // 0.5초 동안 유지
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.LogError("cardView가 null입니다. cardView를 설정해주세요.");
        }
    }

    IEnumerator PlayCardDestroyAnimation2(GameObject cardObject)
    {
        if (cardView2 != null)
        {
            // 상대방 카드가 캔버스 밖에 있다면, cardObject를 캔버스 안으로 이동시킵니다.
            cardObject.transform.SetParent(cardView2.transform.parent, false); // Canvas 내의 올바른 부모로 이동

            // cardObject를 cardView의 위치로 이동
            cardObject.transform.position = cardView2.transform.position;

            // 카드 오브젝트가 0에서 2까지 스케일 증가
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = new Vector3(2, 2, 2);
            float duration = 0.5f;
            float time = 0f;

            // 0.5초 동안 스케일 변화
            while (time < duration)
            {
                cardObject.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            // 최종 스케일 적용
            cardObject.transform.localScale = endScale;

            // 0.5초 동안 유지
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.LogError("cardView가 null입니다. cardView를 설정해주세요.");
        }
    }


    void OnButtonPressed() // 버튼 눌렀을 때
    {
        if (!isButtonPressed)
        {
            mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(1);
            // 버튼 스프라이트 변경
            targetButton.image.sprite = pressedSprite;
            isButtonPressed = true;

            // 버튼을 비활성화하여 눌러도 반응하지 않도록 설정
            targetButton.interactable = false;
            mgr.GetComponent<CardMgr>().mycurturn = turnstate.battle;
            photonView.RPC("Setturn", RpcTarget.Others, turnstate.battle);
        }
    }
    [PunRPC]
    public void ResetButton() // 버튼 리셋
    {
        mon.Clear();
        mon2.Clear();
        attack.Clear();
        attack2.Clear();
        skill.Clear();
        skill2.Clear();
        target_atk = null;
        target_skill = null;
        target_mon = null;
        targetButton.image.sprite = originalSprite;
        isButtonPressed = false;
        // 버튼을 다시 활성화
        targetButton.interactable = true;
        mgr.GetComponent<CardMgr>().mycurturn = turnstate.isActionable;
        mgr.GetComponent<CardMgr>().encurturn = turnstate.isActionable;
        isConditionMet = false;
        mgr.GetComponent<CardMgr>().DrawCardsAndArrange();
        monstate[] allMonstates = FindObjectsOfType<monstate>();

        // 각 monstate의 animacker를 false로 설정
        foreach (monstate mon in allMonstates)
        {
            mon.animacker = false;
        }
    }

    [PunRPC]
    public void Setturn(turnstate value)
    {
        mgr.GetComponent<CardMgr>().encurturn = value;
    }

    [PunRPC]
    public void SendListsToOtherPlayer(string[] attackNames, string[] skillNames, string[] monNames, string[] target_atk, string[] target_skill, string[] target_mon)
    {
        attack2.Clear();
        skill2.Clear();
        mon2.Clear();
        attack2.AddRange(InstantiateObjectsByNames(attackNames, target_atk));
        skill2.AddRange(InstantiateObjectsByNames(skillNames, target_skill));
        mon2.AddRange(InstantiateObjectsByNames(monNames, target_mon));
    }

    // Helper method to instantiate GameObjects from a list of prefab names in the Resources folder
    private GameObject[] InstantiateObjectsByNames(string[] names, string[]target)
    {
        GameObject[] objects = new GameObject[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            GameObject prefab = Resources.Load<GameObject>(names[i]);
            if (prefab != null)
            {
                objects[i] = Instantiate(prefab);
                objects[i].name = names[i]; // Instantiate 후 이름을 원래 이름으로 설정
                target[i]= SwapMeAndOpp(target[i]);
                objects[i].GetComponent<Target>().drop = target[i];
                objects[i].GetComponent<Target>().opcker = true;
            }
            else
            {
                Debug.LogWarning($"Prefab with name {names[i]} could not be found in Resources folder.");
                objects[i] = null; // Prefab이 없는 경우 null로 설정
            }
        }
        return objects;
    }

    private string[] GetObjectNames(List<GameObject> objects)
    {
        string[] names = new string[objects.Count];
        for (int i = 0; i < objects.Count; i++)
        {
            AddToTargetatk(attack[i].GetComponent<Target>().drop);
            names[i] = objects[i].name;
        }

        return names;
    }

    private string[] GetObjectNames2(List<GameObject> objects)
    {
        string[] names = new string[objects.Count];
        for (int i = 0; i < objects.Count; i++)
        {
            AddToTargetSkill(skill[i].GetComponent<Target>().drop);
            names[i] = objects[i].name;
        }

        return names;
    }

    private string[] GetObjectNames3(List<GameObject> objects)
    {
        string[] names = new string[objects.Count];
        for (int i = 0; i < objects.Count; i++)
        {
            AddToTargetmon(mon[i].GetComponent<Target>().drop);
            names[i] = objects[i].name;
        }
        return names;
    }

    string SwapMeAndOpp(string input)
    {
        // 먼저 "me"를 임시 문자열로 대체
        if (input.Contains("me"))
        {
            input = input.Replace("me", "TEMP");
        }
        // "opp"를 "me"로 변경
        if (input.Contains("opp"))
        {
            input = input.Replace("opp", "me");
        }
        // 임시 문자열 "TEMP"를 다시 "opp"로 변경
        if (input.Contains("TEMP"))
        {
            input = input.Replace("TEMP", "opp");
        }

        if (input.Contains("1"))
        {
            input = input.Replace("1", "TEMP");
        }
   
        if (input.Contains("4"))
        {
            input = input.Replace("4", "1");
        }
 
        if (input.Contains("TEMP"))
        {
            input = input.Replace("TEMP", "4");
        }

        if (input.Contains("2"))
        {
            input = input.Replace("2", "TEMP");
        }

        if (input.Contains("5"))
        {
            input = input.Replace("5", "2");
        }

        if (input.Contains("TEMP"))
        {
            input = input.Replace("TEMP", "5");
        }

        if (input.Contains("3"))
        {
            input = input.Replace("3", "TEMP");
        }

        if (input.Contains("6"))
        {
            input = input.Replace("6", "3");
        }

        if (input.Contains("TEMP"))
        {
            input = input.Replace("TEMP", "6");
        }

        Debug.Log(input); // 최종 문자열 출력
        return input;
    }

}
