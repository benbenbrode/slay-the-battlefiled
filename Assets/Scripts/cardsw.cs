using Photon.Pun;
using UnityEngine;

public class cardsw : MonoBehaviourPunCallbacks
{
    public GameObject[] cardPrefabs; // 생성할 card 프리팹 배열
    public Transform hostPos;        // 카드 생성 위치
    public float radius = 300f;      // 부채꼴 반지름
    public float angleRange = 45f;   // 부채꼴 각도 범위

    private bool cardSpawned = false;

    private void Start()
    {
        if (cardPrefabs.Length == 0)
        {
            Debug.LogError("No card prefabs assigned.");
            return;
        }

        // 배열을 섞습니다.
        ShuffleArray(cardPrefabs);

        // 방에 이미 두 명의 플레이어가 있다면 카드를 생성합니다.
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

        // 5개의 카드 프리팹을 생성하여 배열에 저장합니다.
        for (int i = 0; i < 5; i++)
        {
            spawnedCards[i] = SpawnCard(cardPrefabs[i]);
        }

        ArrangeCardsInFanShape(spawnedCards); // 부채꼴 형태로 카드 정렬

        cardSpawned = true; // 카드가 생성되었음을 표시
    }

    private GameObject SpawnCard(GameObject cardPrefab)
    {
        if (cardPrefab == null || hostPos == null)
        {
            Debug.LogError("CardPrefab or HostPos is not assigned.");
            return null;
        }

        // 지정된 위치에 카드 프리팹 생성
        GameObject card = Instantiate(cardPrefab, hostPos.position, Quaternion.identity);

        // 생성된 카드를 hostPos의 자식으로 설정
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

            // 카드를 hostPos 기준으로 위치를 설정합니다.
            cards[i].transform.localPosition = direction * radius;

            // 카드 회전 조정
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
