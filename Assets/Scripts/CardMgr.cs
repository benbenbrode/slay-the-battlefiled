using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum turnstate
{
    isActionable = 0,
    battle = 1,
}

public class CardMgr : MonoBehaviourPunCallbacks
{
    public Transform handposTr;        // 카드 생성 위치
    public float radius = 300f;      // 부채꼴 반지름
    public float angleRange = 45f;   // 부채꼴 각도 범위
    public GameObject[] HandCard; // 핸드
    public GameObject handpos;
    public List<GameObject> deck; // 덱을 List로 변경하여 동적 크기 조정 가능
    private bool cardSpawned = false; // 처음 카드 생성확인
    public turnstate mycurturn = turnstate.isActionable;
    public turnstate encurturn = turnstate.isActionable;

    public Button targetButton; // 변경할 버튼
    public Sprite originalSprite; // 원래 스프라이트
    public Sprite pressedSprite; // 눌렀을 때의 스프라이트

    public GameObject net;
    void Awake()
    {
        // netmgr 오브젝트를 찾고 로그 출력
        net = GameObject.Find("netmgr(Clone)");

        // net이 null인지 확인
        if (net == null)
        {
            Debug.LogError("netmgr object not found!");
            return;
        }

        // Network 컴포넌트가 있는지 확인
        Network networkComponent = net.GetComponent<Network>();
        if (networkComponent == null)
        {
            Debug.LogError("Network component not found on netmgr object!");
            return;
        }

        // deck 리스트가 null이거나 비어있는지 확인
        if (networkComponent.deck == null || networkComponent.deck.Count == 0)
        {
            Debug.LogError("Prefab names list (deck) is null or empty.");
            return;
        }

        // 문자열 리스트에 있는 이름을 바탕으로 프리팹을 로드하고 리스트에 참조만 추가
        foreach (string prefabName in networkComponent.deck)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogWarning("Prefab name is null or empty.");
                continue;  // 이름이 null이거나 비어있으면 건너뜀
            }

            // Resources 폴더에서 프리팹 로드
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            // 프리팹 로드 성공 여부 확인
            if (prefab != null)
            {
                // 프리팹이 존재하면 리스트에 참조만 추가
                deck.Add(prefab);
                Debug.Log("Prefab added to list: " + prefabName);
            }
            else
            {
                // 프리팹이 존재하지 않으면 경고 메시지 출력
                Debug.LogWarning("Prefab not found in Resources: " + prefabName);
            }
        }
        ShuffleList(deck);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DrawCardsAndArrange();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            photonView.RPC("SpawnCards", RpcTarget.All);
            Debug.Log("엔터더 룸");
        }
    }

    //public override void OnJoinedRoom()
    //{
    //    if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
    //    {
    //        SpawnCards();
    //        Debug.Log("룸에 들어왔을때");
    //    }
    //}

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    [PunRPC]
    public void ReceiveType(int receivedType)
    {
        net.GetComponent<Network>().type2 = receivedType;
        Debug.Log("Received type value from another player: " + receivedType);
    }

    [PunRPC]
    public void SpawnCards()
    {
        photonView.RPC("ReceiveType", RpcTarget.Others, net.GetComponent<Network>().type);
        if (cardSpawned || deck.Count < 5)
        {
            return;
        }

        // 5개의 카드 프리팹을 생성하여 배열에 저장합니다.
        for (int i = 0; i < 5; i++)
        {
            HandCard[i] = SpawnCard(deck[0]); // deck[0]을 사용하여 가장 앞의 카드 생성
            deck.RemoveAt(0); // 생성한 카드 제거
        }

        ArrangeCardsInFanShape(HandCard); // 부채꼴 형태로 카드 정렬

        cardSpawned = true; // 카드가 생성되었음을 표시
    }

    private GameObject SpawnCard(GameObject cardPrefab)
    {
        if (cardPrefab == null || handposTr == null)
        {
            Debug.LogError("CardPrefab or HostPos is not assigned.");
            return null;
        }

        // 지정된 위치에 카드 프리팹 생성
        GameObject card = Instantiate(cardPrefab, handposTr.position, Quaternion.identity);

        // 생성된 카드를 hostPos의 자식으로 설정
        card.transform.SetParent(handposTr, false);

        return card;
    }

    public void ArrangeCardsInFanShape(GameObject[] cards)
    {
        int cardCount = cards.Length;
        float startAngle = -(angleRange * cardCount) / 2f;
        float angleStep = (angleRange * cardCount) / (cardCount - 1);
        if (cards.Length == 1)
        {
            cards[0].transform.localRotation = Quaternion.Euler(0, 0, -90);
            handpos.transform.position = new Vector3(5f, handpos.transform.position.y, handpos.transform.position.z);
        }
        else if (cards.Length > 1)
        {
            for (int i = 0; i < cardCount; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
                Vector3 cardPosition = handposTr.position + direction * radius;

                // 카드를 hostPos 기준으로 위치를 설정합니다.
                cards[i].transform.localPosition = direction * (radius * cardCount);

                // 카드 회전 조정
                cards[i].transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
            }

            if (cards.Length == 2)
                handpos.transform.position = new Vector3(handpos.transform.position.x, -6.6f, handpos.transform.position.z);
            if (cards.Length == 3)
                handpos.transform.position = new Vector3(handpos.transform.position.x, -7.9f, handpos.transform.position.z);
            if (cards.Length == 4)
                handpos.transform.position = new Vector3(handpos.transform.position.x, -9.2f, handpos.transform.position.z);
            if (cards.Length == 5)
                handpos.transform.position = new Vector3(handpos.transform.position.x, -10.5f, handpos.transform.position.z);
        }
    }

    // 배열에서 게임 오브젝트를 제거하는 메서드
    public void RemoveCard(GameObject card)
    {
        // 현재 배열의 크기와 내용을 기반으로 새로운 배열 생성
        int newSize = HandCard.Length - 1;
        GameObject[] newArray = new GameObject[newSize];
        int index = 0;

        foreach (var obj in HandCard)
        {
            if (obj != card)
            {
                newArray[index++] = obj;
            }
        }

        // 새 배열로 교체
        HandCard = newArray;
        ArrangeCardsInFanShape(HandCard);
    }

    // E 키를 눌렀을 때 호출되는 메서드
    public void DrawCardsAndArrange()
    {
        int currentHandCount = HandCard.Length;

        // HandCard가 5장이 될 때까지 덱에서 추가로 카드를 뽑음
        if (currentHandCount < 5)
        {
            int cardsToDraw = 5 - currentHandCount;

            // 현재 HandCard 배열을 새로운 크기로 확장
            GameObject[] newHandCard = new GameObject[5];
            for (int i = 0; i < currentHandCount; i++)
            {
                newHandCard[i] = HandCard[i];
            }

            // 부족한 카드만큼 덱에서 뽑아 HandCard에 추가
            for (int i = 0; i < cardsToDraw; i++)
            {
                newHandCard[currentHandCount + i] = SpawnCard(deck[0]);
                deck.RemoveAt(0); // 생성한 카드 제거
            }

            HandCard = newHandCard;
        }

        ArrangeCardsInFanShape(HandCard); // 부채꼴 형태로 카드 정렬
    }
}
