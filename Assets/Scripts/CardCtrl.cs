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
    private Quaternion originalRotation; // 드래그 시작 시 초기 각도 저장
    private Canvas canvas;
    public int handnum = 100;

    public GameObject dropTarget; // 드랍된 타켓

    private bool activeck = true; // 올바른 타켓 확인용
    public bool viewcker = false; // 강조때 드래그 막기

    public int Cost = 0; // 카드 코스트
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
        // 드래그 시작 시 각도값을 0으로 초기화
        transform.rotation = Quaternion.identity;

        if (gameObject.CompareTag("mon_card") || gameObject.CompareTag("mon_card4"))
        {
            // 오브젝트가 null인지 확인한 후 활성화
            TryActivateObject(pos1);
            TryActivateObject(pos2);
            TryActivateObject(pos3);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        // 현재 턴이 'battle' 상태이면 드래그를 막음
        if (mgr.GetComponent<CardMgr>().mycurturn == turnstate.battle)
        {
            return;
        }

        if (viewcker == true)
            return;
        // 마우스 좌표를 월드 좌표로 변환 (Camera 모드에서)
        Vector2 screenPoint = eventData.position;  // 마우스의 화면 좌표
        Vector3 worldPoint;

        // 캔버스의 월드 좌표계로 변환
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out worldPoint);

        // 월드 좌표를 로컬 좌표로 변환하여 UI 오브젝트 위치 설정
      

        string objectTag = gameObject.tag;

        // 코스트에 따른 태그별 조건 체크
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
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate 스크립트를 가지고 있을 때
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
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate 스크립트를 가지고 있을 때
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
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate 스크립트를 가지고 있을 때
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
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate 스크립트를 가지고 있을 때
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
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate 스크립트를 가지고 있을 때
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
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)
                {
                    // monstate 스크립트를 가지고 있을 때
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
            // parentObject의 모든 하위 오브젝트를 반복
            foreach (Transform child in obj.transform)
            {
                // 하위 오브젝트가 활성화되어 있으면 비활성화, 비활성화되어 있으면 활성화
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
    }
}
