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
    public Transform handposTr;        // ī�� ���� ��ġ
    public float radius = 300f;      // ��ä�� ������
    public float angleRange = 45f;   // ��ä�� ���� ����
    public GameObject[] HandCard; // �ڵ�
    public GameObject handpos;
    public List<GameObject> deck; // ���� List�� �����Ͽ� ���� ũ�� ���� ����
    private bool cardSpawned = false; // ó�� ī�� ����Ȯ��
    public turnstate mycurturn = turnstate.isActionable;
    public turnstate encurturn = turnstate.isActionable;

    public Button targetButton; // ������ ��ư
    public Sprite originalSprite; // ���� ��������Ʈ
    public Sprite pressedSprite; // ������ ���� ��������Ʈ

    public GameObject net;
    void Awake()
    {
        // netmgr ������Ʈ�� ã�� �α� ���
        net = GameObject.Find("netmgr(Clone)");

        // net�� null���� Ȯ��
        if (net == null)
        {
            Debug.LogError("netmgr object not found!");
            return;
        }

        // Network ������Ʈ�� �ִ��� Ȯ��
        Network networkComponent = net.GetComponent<Network>();
        if (networkComponent == null)
        {
            Debug.LogError("Network component not found on netmgr object!");
            return;
        }

        // deck ����Ʈ�� null�̰ų� ����ִ��� Ȯ��
        if (networkComponent.deck == null || networkComponent.deck.Count == 0)
        {
            Debug.LogError("Prefab names list (deck) is null or empty.");
            return;
        }

        // ���ڿ� ����Ʈ�� �ִ� �̸��� �������� �������� �ε��ϰ� ����Ʈ�� ������ �߰�
        foreach (string prefabName in networkComponent.deck)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogWarning("Prefab name is null or empty.");
                continue;  // �̸��� null�̰ų� ��������� �ǳʶ�
            }

            // Resources �������� ������ �ε�
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            // ������ �ε� ���� ���� Ȯ��
            if (prefab != null)
            {
                // �������� �����ϸ� ����Ʈ�� ������ �߰�
                deck.Add(prefab);
                Debug.Log("Prefab added to list: " + prefabName);
            }
            else
            {
                // �������� �������� ������ ��� �޽��� ���
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
            Debug.Log("���ʹ� ��");
        }
    }

    //public override void OnJoinedRoom()
    //{
    //    if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
    //    {
    //        SpawnCards();
    //        Debug.Log("�뿡 ��������");
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

        // 5���� ī�� �������� �����Ͽ� �迭�� �����մϴ�.
        for (int i = 0; i < 5; i++)
        {
            HandCard[i] = SpawnCard(deck[0]); // deck[0]�� ����Ͽ� ���� ���� ī�� ����
            deck.RemoveAt(0); // ������ ī�� ����
        }

        ArrangeCardsInFanShape(HandCard); // ��ä�� ���·� ī�� ����

        cardSpawned = true; // ī�尡 �����Ǿ����� ǥ��
    }

    private GameObject SpawnCard(GameObject cardPrefab)
    {
        if (cardPrefab == null || handposTr == null)
        {
            Debug.LogError("CardPrefab or HostPos is not assigned.");
            return null;
        }

        // ������ ��ġ�� ī�� ������ ����
        GameObject card = Instantiate(cardPrefab, handposTr.position, Quaternion.identity);

        // ������ ī�带 hostPos�� �ڽ����� ����
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

                // ī�带 hostPos �������� ��ġ�� �����մϴ�.
                cards[i].transform.localPosition = direction * (radius * cardCount);

                // ī�� ȸ�� ����
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

    // �迭���� ���� ������Ʈ�� �����ϴ� �޼���
    public void RemoveCard(GameObject card)
    {
        // ���� �迭�� ũ��� ������ ������� ���ο� �迭 ����
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

        // �� �迭�� ��ü
        HandCard = newArray;
        ArrangeCardsInFanShape(HandCard);
    }

    // E Ű�� ������ �� ȣ��Ǵ� �޼���
    public void DrawCardsAndArrange()
    {
        int currentHandCount = HandCard.Length;

        // HandCard�� 5���� �� ������ ������ �߰��� ī�带 ����
        if (currentHandCount < 5)
        {
            int cardsToDraw = 5 - currentHandCount;

            // ���� HandCard �迭�� ���ο� ũ��� Ȯ��
            GameObject[] newHandCard = new GameObject[5];
            for (int i = 0; i < currentHandCount; i++)
            {
                newHandCard[i] = HandCard[i];
            }

            // ������ ī�常ŭ ������ �̾� HandCard�� �߰�
            for (int i = 0; i < cardsToDraw; i++)
            {
                newHandCard[currentHandCount + i] = SpawnCard(deck[0]);
                deck.RemoveAt(0); // ������ ī�� ����
            }

            HandCard = newHandCard;
        }

        ArrangeCardsInFanShape(HandCard); // ��ä�� ���·� ī�� ����
    }
}
