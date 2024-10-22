using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.U2D;
using Photon.Pun.Demo.PunBasics;

public class CardCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Quaternion originalRotation; // �巡�� ���� �� �ʱ� ���� ����
    private Canvas canvas;
    public int handnum = 100;

    public GameObject dropTarget; // ����� Ÿ��

    private bool activeck = true; // �ùٸ� Ÿ�� Ȯ�ο�
    public bool viewcker = false; // ������ �巡�� ����

    public int Cost = 0; // ī�� �ڽ�Ʈ
    public GameObject canvans;
    public GameObject mgr;
    public GameObject me;

    public GameObject pos1;
    public GameObject pos2;
    public GameObject pos3;
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        me = GameObject.Find("Canvas/me_drop");
        mgr = GameObject.Find("mgr");

        originalRotation = transform.rotation;

        if (canvas != null)
        {

            pos1 = canvas.transform.Find("pos1")?.gameObject;
            pos2 = canvas.transform.Find("pos2")?.gameObject;
            pos3 = canvas.transform.Find("pos3")?.gameObject;

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (mgr.GetComponent<CardMgr>().mycurturn == turnstate.battle)
        {
            return;
        }

        if (viewcker == true)
            return;

        originalPosition = transform.position;
        // �巡�� ���� �� �������� 0���� �ʱ�ȭ
        transform.rotation = Quaternion.identity;

        if (gameObject.CompareTag("mon_card") || gameObject.CompareTag("mon_card4"))
        {
            // ������Ʈ�� null���� Ȯ���� �� Ȱ��ȭ
            TryActivateObject(pos1);
            TryActivateObject(pos2);
            TryActivateObject(pos3);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        // ���� ���� 'battle' �����̸� �巡�׸� ����
        if (mgr.GetComponent<CardMgr>().mycurturn == turnstate.battle)
        {
            return;
        }

        if (viewcker == true)
            return;
        // ���콺 ��ǥ�� ���� ��ǥ�� ��ȯ (Camera ��忡��)
        Vector2 screenPoint = eventData.position;  // ���콺�� ȭ�� ��ǥ
        Vector3 worldPoint;

        // ĵ������ ���� ��ǥ��� ��ȯ
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out worldPoint);

        // ���� ��ǥ�� ���� ��ǥ�� ��ȯ�Ͽ� UI ������Ʈ ��ġ ����
      

        string objectTag = gameObject.tag;

        // �ڽ�Ʈ�� ���� �±׺� ���� üũ
        if (me.GetComponent<PlayerState>().cost > 0 && (objectTag == "attack" || objectTag == "skill" || objectTag == "mon_card" || objectTag == "skill_player" || objectTag == "attack_player"))
        {
            transform.position = worldPoint;
        }
        if (me.GetComponent<PlayerState>().cost > 1 && (objectTag == "attack_2" || objectTag == "skill_2" || objectTag == "attack_2player"))
        {
            transform.position = worldPoint;
        }
        if (me.GetComponent<PlayerState>().cost > 2 && (objectTag == "attack_3" || objectTag == "skill_3"))
        {
            transform.position = worldPoint;
        }
        if (me.GetComponent<PlayerState>().cost > 3 && (objectTag == "mon_card4"))
        {
            transform.position = worldPoint;
        }
        if (objectTag == "attack_0" || objectTag == "skill_0")
        {
            transform.position = worldPoint;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (mgr.GetComponent<CardMgr>().mycurturn == turnstate.battle)
        {
            return;
        }

        dropTarget = eventData.pointerCurrentRaycast.gameObject;

        if (gameObject.CompareTag("attack"))
        {
            if (dropTarget != null && dropTarget.tag.Contains("opp"))
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null)
                {
                    // PlayerState ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }      
                battleManager.AddToAttack(this.gameObject);
            }
        }
        if (gameObject.CompareTag("attack_0"))
        {
            if (dropTarget != null && dropTarget.tag.Contains("opp"))
            {
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null)
                {
                    // PlayerState ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }
                battleManager.AddToAttack(this.gameObject);
            }
        }
        if (gameObject.CompareTag("attack_3"))
        {
            if (dropTarget != null && dropTarget.tag.Contains("opp"))
            {
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                me.GetComponent<PlayerState>().cost -= 3;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null)
                {
                    // PlayerState ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }
                battleManager.AddToAttack(this.gameObject);
            }
        }

        if (gameObject.CompareTag("attack_2player"))
        {
            if (dropTarget != null && dropTarget.CompareTag("opp_player"))
            {
                me.GetComponent<PlayerState>().cost -= 2;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                battleManager.AddToAttack(this.gameObject);
            }
        }



        if (gameObject.CompareTag("attack_player"))
        {
            if (dropTarget != null && dropTarget.CompareTag("opp_player"))
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                battleManager.AddToAttack(this.gameObject);
            }
        }

        if (gameObject.CompareTag("skill"))
        {
            if (dropTarget != null && dropTarget.tag.Contains("ally"))
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null)
                {
                    // PlayerState ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }
                battleManager.AddToSkill(this.gameObject);
            }
        }

        if (gameObject.CompareTag("skill_0"))
        {
            if (dropTarget != null && dropTarget.tag.Contains("ally"))
            {
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null)
                {
                    // PlayerState ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }
                battleManager.AddToSkill(this.gameObject);
            }
        }

        if (gameObject.CompareTag("skill_2"))
        {
            if (dropTarget != null && dropTarget.tag.Contains("ally"))
            {
                me.GetComponent<PlayerState>().cost -= 2;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null)
                {
                    // PlayerState ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate ��ũ��Ʈ�� ������ ���� ��
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }
                battleManager.AddToSkill(this.gameObject);
            }
        }

        if (gameObject.CompareTag("skill_player"))
        {
            if (dropTarget != null && dropTarget.CompareTag("ally_player"))
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                battleManager.AddToSkill(this.gameObject);
            }
        }

        if (gameObject.CompareTag("mon_card"))
        {
            if (dropTarget != null && dropTarget.tag == "pos1")
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                gameObject.GetComponent<Target>().objectPosition = dropTarget.transform.position;
                gameObject.GetComponent<Target>().tag = "ally_mon1";
                battleManager.AddTomon(this.gameObject);
                battleManager.poscker[1] = false;
            }

            if (dropTarget != null && dropTarget.tag == "pos2")
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                gameObject.GetComponent<Target>().objectPosition = dropTarget.transform.position;
                gameObject.GetComponent<Target>().tag = "ally_mon2";
                battleManager.AddTomon(this.gameObject);
                battleManager.poscker[2] = false;
            }

            if (dropTarget != null && dropTarget.tag == "pos3")
            {
                me.GetComponent<PlayerState>().cost -= 1;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                gameObject.GetComponent<Target>().objectPosition = dropTarget.transform.position;
                gameObject.GetComponent<Target>().tag = "ally_mon3";
                battleManager.AddTomon(this.gameObject);
                battleManager.poscker[3] = false;
            }

            TryActivateObject(pos1);
            TryActivateObject(pos2);
            TryActivateObject(pos3);
        }

        if (gameObject.CompareTag("mon_card4"))
        {
            if (dropTarget != null && dropTarget.tag == "pos1")
            {
                me.GetComponent<PlayerState>().cost -= 4;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                gameObject.GetComponent<Target>().objectPosition = dropTarget.transform.position;
                gameObject.GetComponent<Target>().tag = "ally_mon1";
                battleManager.AddTomon(this.gameObject);
                battleManager.poscker[1] = false;
            }

            if (dropTarget != null && dropTarget.tag == "pos2")
            {
                me.GetComponent<PlayerState>().cost -= 4;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                gameObject.GetComponent<Target>().objectPosition = dropTarget.transform.position;
                gameObject.GetComponent<Target>().tag = "ally_mon2";
                battleManager.AddTomon(this.gameObject);
                battleManager.poscker[2] = false;
            }

            if (dropTarget != null && dropTarget.tag == "pos3")
            {
                me.GetComponent<PlayerState>().cost -= 4;
                transform.Translate(1000f, 0f, 0f);
                activeck = false;
                viewcker = true;
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject);
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                gameObject.GetComponent<Target>().drop = dropTarget.name;
                gameObject.GetComponent<Target>().objectPosition = dropTarget.transform.position;
                gameObject.GetComponent<Target>().tag = "ally_mon3";
                battleManager.AddTomon(this.gameObject);
                battleManager.poscker[3] = false;
            }

            TryActivateObject(pos1);
            TryActivateObject(pos2);
            TryActivateObject(pos3);
        }

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(eventData, raycastResults);

        if (activeck == true)
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
    private void TryActivateObject(GameObject obj)
    {
        {
            // parentObject�� ��� ���� ������Ʈ�� �ݺ�
            foreach (Transform child in obj.transform)
            {
                // ���� ������Ʈ�� Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ, ��Ȱ��ȭ�Ǿ� ������ Ȱ��ȭ
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
    }
}
