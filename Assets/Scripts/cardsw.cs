using Photon.Pun;
using UnityEngine;

public class cardsw : MonoBehaviourPunCallbacks
{
    public GameObject[] cardPrefabs; // ������ card ������ �迭
    public Transform hostPos;        // ī�� ���� ��ġ
    public float radius = 300f;      // ��ä�� ������
    public float angleRange = 45f;   // ��ä�� ���� ����

    private bool cardSpawned = false;

    private void Start()
    {
        if (cardPrefabs.Length == 0)
        {
            Debug.LogError("No card prefabs assigned.");
            return;
        }

        // �迭�� �����ϴ�.
        ShuffleArray(cardPrefabs);

        // �濡 �̹� �� ���� �÷��̾ �ִٸ� ī�带 �����մϴ�.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SpawnCards();
        }
    }

    private void ShuffleArray(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void SpawnCards()
    {
        if (cardSpawned || cardPrefabs.Length < 5)
        {
            return;
        }

        GameObject[] spawnedCards = new GameObject[5];

        // 5���� ī�� �������� �����Ͽ� �迭�� �����մϴ�.
        for (int i = 0; i < 5; i++)
        {
            spawnedCards[i] = SpawnCard(cardPrefabs[i]);
        }

        ArrangeCardsInFanShape(spawnedCards); // ��ä�� ���·� ī�� ����

        cardSpawned = true; // ī�尡 �����Ǿ����� ǥ��
    }

    private GameObject SpawnCard(GameObject cardPrefab)
    {
        if (cardPrefab == null || hostPos == null)
        {
            Debug.LogError("CardPrefab or HostPos is not assigned.");
            return null;
        }

        // ������ ��ġ�� ī�� ������ ����
        GameObject card = Instantiate(cardPrefab, hostPos.position, Quaternion.identity);

        // ������ ī�带 hostPos�� �ڽ����� ����
        card.transform.SetParent(hostPos, false);

        return card;
    }

    public void ArrangeCardsInFanShape(GameObject[] cards)
    {
        int cardCount = cards.Length;
        float startAngle = -angleRange / 2f;
        float angleStep = angleRange / (cardCount - 1);

        for (int i = 0; i < cardCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Vector3 cardPosition = hostPos.position + direction * radius;

            // ī�带 hostPos �������� ��ġ�� �����մϴ�.
            cards[i].transform.localPosition = direction * radius;

            // ī�� ȸ�� ����
            cards[i].transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SpawnCards();
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            SpawnCards();
        }
    }
}
