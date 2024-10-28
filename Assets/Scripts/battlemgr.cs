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


    public Button targetButton; // ������ ��ư
    public Sprite originalSprite; // ���� ��������Ʈ
    public Sprite pressedSprite; // ������ ���� ��������Ʈ
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
            // attack �� skill ����Ʈ���� �̸� ����� �����ɴϴ�.
            string[] attackNames = GetObjectNames(attack);
            string[] skillNames = GetObjectNames2(skill);
            string[] monNames = GetObjectNames3(mon);

            // �ٸ� �÷��̾�� ���� �� ��ų ����Ʈ�� ����
            photonView.RPC("SendListsToOtherPlayer", RpcTarget.Others, attackNames, skillNames, monNames, target_atk, target_skill, target_mon);

            // Coroutine�� ���� ����Ʈ ���� �� ���� ����
            StartCoroutine(StartRemoveObjectsWithDelay());
            isConditionMet = true;
        }
    }



    IEnumerator CallResetButtonWithDelay() // ����������
    {
        // 1�� ���
        turnend = true;
        yield return new WaitForSeconds(1f);
        // Ư�� �ڷ�ƾ�� ���� ȣ���ϰ� �ش� �ڷ�ƾ�� ���� ������ ����մϴ�.
        yield return StartCoroutine(InvokeEffCoroutineOnTaggedObjects()); // 
        Debug.Log("�ϳ� ���� �ҷ���");
        // �÷��̾� ������ �ڽ�Ʈ�� ����
        me.GetComponent<PlayerState>().cost = me.GetComponent<PlayerState>().maxcost + me.GetComponent<PlayerState>().tempcost;
        me.GetComponent<PlayerState>().tempcost = 0;
        turnend = false;
        int Myhp = me.GetComponent<PlayerState>().hp;
        monstate[] allMonstates = FindObjectsOfType<monstate>();

        // monstate�� �ϳ��� ���� ��� �ٷ� ResetButton ȣ��
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
            yield return null; // �� ������ ���
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
                return false; // �ϳ��� false�� false ��ȯ
            }
        }
        return true; // ��� true�� �� true ��ȯ
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
        // ��� Ȱ��ȭ�� ���� ������Ʈ�� �����ɴϴ�.
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // ������Ʈ�� �±׿� "_mon"�� ���ԵǾ� �ִ��� Ȯ���մϴ�.
            if (obj.tag.Contains("_mon"))
            {
                // �ش� ������Ʈ�� ��� MonoBehaviour ��ũ��Ʈ�� �����ɴϴ�.
                MonoBehaviour[] monoBehaviours = obj.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour mb in monoBehaviours)
                {
                    // "eff" �ڷ�ƾ�� ������ �ִ��� Ȯ���մϴ�.
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
        // target_skill �迭�� null�̸� �ʱ�ȭ
        if (target_atk == null)
        {
            target_atk = new string[1]; // �迭 ũ�⸦ 1�� ����
            target_atk[0] = newatk;   // ù ��° ��ҿ� �� ��ų�� �߰�
        }
        else
        {
            // ���� �迭���� ũ�Ⱑ 1 �� ū �� �迭�� ����
            string[] newArray = new string[target_atk.Length + 1];

            // ���� �迭�� ���� �� �迭�� ����
            for (int i = 0; i < target_atk.Length; i++)
            {
                newArray[i] = target_atk[i];
            }

            // ���ο� ��Ҹ� �������� �߰�
            newArray[newArray.Length - 1] = newatk;

            // ���� �迭�� ���ο� �迭�� ��ü
            target_atk = newArray;
        }
    }

    void AddToTargetSkill(string newSkill)
    {
        // target_skill �迭�� null�̸� �ʱ�ȭ
        if (target_skill == null)
        {
            target_skill = new string[1]; // �迭 ũ�⸦ 1�� ����
            target_skill[0] = newSkill;   // ù ��° ��ҿ� �� ��ų�� �߰�
        }
        else
        {
            // ���� �迭���� ũ�Ⱑ 1 �� ū �� �迭�� ����
            string[] newArray = new string[target_skill.Length + 1];

            // ���� �迭�� ���� �� �迭�� ����
            for (int i = 0; i < target_skill.Length; i++)
            {
                newArray[i] = target_skill[i];
            }

            // ���ο� ��Ҹ� �������� �߰�
            newArray[newArray.Length - 1] = newSkill;

            // ���� �迭�� ���ο� �迭�� ��ü
            target_skill = newArray;
        }
    }

    void AddToTargetmon(string newmon)
    {
        // target_skill �迭�� null�̸� �ʱ�ȭ
        if (target_mon == null)
        {
            target_mon = new string[1]; // �迭 ũ�⸦ 1�� ����
            target_mon[0] = newmon;   // ù ��° ��ҿ� �� ��ų�� �߰�
        }
        else
        {
            // ���� �迭���� ũ�Ⱑ 1 �� ū �� �迭�� ����
            string[] newArray = new string[target_mon.Length + 1];

            // ���� �迭�� ���� �� �迭�� ����
            for (int i = 0; i < target_mon.Length; i++)
            {
                newArray[i] = target_mon[i];
            }

            // ���ο� ��Ҹ� �������� �߰�
            newArray[newArray.Length - 1] = newmon;

            // ���� �迭�� ���ο� �迭�� ��ü
            target_mon = newArray;
        }
    }

    IEnumerator StartRemoveObjectsWithDelay()
    {
        // 2�� ���
        yield return new WaitForSeconds(2f);

        // RemoveObjectsWithDelay �ڷ�ƾ ����
        StartCoroutine(RemoveObjectsWithDelay());
    }

    IEnumerator RemoveObjectsWithDelay()
    {
        // applycker�� false�� ���� �۵�
        while ((skill.Count > 0 || attack.Count > 0) || (skill2.Count > 0 || attack2.Count > 0))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // �÷��̾��� ����: skill -> attack ������ ����
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

                // ������ ����: skill2 -> attack2 ������ ����
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
                // �ִϸ��̼��� ���� ������ ���
                while (applycker)
                {
                    yield return null;
                }
            }
            else
            {
                // ������ ����: skill2 -> attack2 ������ ����
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

                // �÷��̾��� ����: skill -> attack ������ ����
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
                // �ִϸ��̼��� ���� ������ ���
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
                Destroy(mon[i]);  // ������Ʈ ����
            }
        }
        Debug.Log(mon2.Count);
        for (int i = 0; i < mon2.Count; i++)
        {
            if (mon2[i] != null)
            {
                Destroy(mon2[i]);  // ������Ʈ ����
                Debug.Log(mon2.Count+ "����");

            }
        }
        Debug.Log("skill, attack, skill2, attack2 ����Ʈ�� ��� ��������ϴ�.");
        StartCoroutine(CallResetButtonWithDelay());
    }
    IEnumerator PlayCardDestroyAnimation(GameObject cardObject)
    {
        if (cardView != null)
        {
            Debug.Log("�ִϸ��̼� ���� ���� ī��: " + cardObject.name);
            Debug.Log("cardView ��ġ: " + cardView.transform.position);

            // cardObject�� cardView�� ��ġ�� �̵�
            cardObject.transform.position = cardView.transform.position;
            Debug.Log("ī�尡 cardView ��ġ�� �̵���");

            // ī�� ������Ʈ�� 0���� 2���� ������ ����
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = new Vector3(2, 2, 2);
            float duration = 0.5f;
            float time = 0f;

            // 0.5�� ���� ������ ��ȭ
            while (time < duration)
            {
                cardObject.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            // ���� ������ ����
            cardObject.transform.localScale = endScale;
            Debug.Log("ī�� ������ �ִϸ��̼� �Ϸ�");

            // 0.5�� ���� ����
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.LogError("cardView�� null�Դϴ�. cardView�� �������ּ���.");
        }
    }

    IEnumerator PlayCardDestroyAnimation2(GameObject cardObject)
    {
        if (cardView2 != null)
        {
            // ���� ī�尡 ĵ���� �ۿ� �ִٸ�, cardObject�� ĵ���� ������ �̵���ŵ�ϴ�.
            cardObject.transform.SetParent(cardView2.transform.parent, false); // Canvas ���� �ùٸ� �θ�� �̵�

            // cardObject�� cardView�� ��ġ�� �̵�
            cardObject.transform.position = cardView2.transform.position;

            // ī�� ������Ʈ�� 0���� 2���� ������ ����
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = new Vector3(2, 2, 2);
            float duration = 0.5f;
            float time = 0f;

            // 0.5�� ���� ������ ��ȭ
            while (time < duration)
            {
                cardObject.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            // ���� ������ ����
            cardObject.transform.localScale = endScale;

            // 0.5�� ���� ����
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.LogError("cardView�� null�Դϴ�. cardView�� �������ּ���.");
        }
    }


    void OnButtonPressed() // ��ư ������ ��
    {
        if (!isButtonPressed)
        {
            mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(1);
            // ��ư ��������Ʈ ����
            targetButton.image.sprite = pressedSprite;
            isButtonPressed = true;

            // ��ư�� ��Ȱ��ȭ�Ͽ� ������ �������� �ʵ��� ����
            targetButton.interactable = false;
            mgr.GetComponent<CardMgr>().mycurturn = turnstate.battle;
            photonView.RPC("Setturn", RpcTarget.Others, turnstate.battle);
        }
    }
    [PunRPC]
    public void ResetButton() // ��ư ����
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
        // ��ư�� �ٽ� Ȱ��ȭ
        targetButton.interactable = true;
        mgr.GetComponent<CardMgr>().mycurturn = turnstate.isActionable;
        mgr.GetComponent<CardMgr>().encurturn = turnstate.isActionable;
        isConditionMet = false;
        mgr.GetComponent<CardMgr>().DrawCardsAndArrange();
        monstate[] allMonstates = FindObjectsOfType<monstate>();

        // �� monstate�� animacker�� false�� ����
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
                objects[i].name = names[i]; // Instantiate �� �̸��� ���� �̸����� ����
                target[i]= SwapMeAndOpp(target[i]);
                objects[i].GetComponent<Target>().drop = target[i];
                objects[i].GetComponent<Target>().opcker = true;
            }
            else
            {
                Debug.LogWarning($"Prefab with name {names[i]} could not be found in Resources folder.");
                objects[i] = null; // Prefab�� ���� ��� null�� ����
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
        // ���� "me"�� �ӽ� ���ڿ��� ��ü
        if (input.Contains("me"))
        {
            input = input.Replace("me", "TEMP");
        }
        // "opp"�� "me"�� ����
        if (input.Contains("opp"))
        {
            input = input.Replace("opp", "me");
        }
        // �ӽ� ���ڿ� "TEMP"�� �ٽ� "opp"�� ����
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

        Debug.Log(input); // ���� ���ڿ� ���
        return input;
    }

}
