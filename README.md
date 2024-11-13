# slay-the-battlefiled


게임 장르 : 2D 턴제 카드게임
---

게임 소개 : 
원하는 카드로 자신만의 덱을 구축하고 상대의 HP를 0으로 만들면 승리하는 게임입니다.
---

게임 규칙 : 
카드를 올바른 대상에 드래그앤 드롭하면 적용되며 턴종료시 효과가 스킬카드 -> 공격카드 -> 스폰카드 순으로 처리됩니다.
턴 시작시 손패가 5장이 되도록 카드를 뽑습니다.
---

개발 목적 : 즐겨 했던 턴제 카드 게임의 기능 직접 구현하고 내가 느낀 장점들로 나만의 카드게임 구현
---

개발 기간 : 2024.06.10 - 2024.10.30
---

프로젝트 타입 : 개인프로젝트
---

사용 엔진 : UNITY 2022.3.15f1
---

시연 영상 : https://www.youtube.com/watch?v=fT_4HDiQwJ8
---

주요 활용 기술
---
#01) 포톤 네트워크를 이용한 랜덤매칭 시스템 구현
<details>
<summary>예시 코드</summary>
  
```csharp
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon; // 커스텀 룸 속성 사용을 위한 네임스페이스
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network : MonoBehaviourPunCallbacks
{
    public static Network Instance;
    public int type = 0; // 내 직업
    public int type2 = 0; // 상대 직업
    public List<string> deck; // 내 덱

    private void Awake()
    {
        Debug.Log("NetworkManager Awake");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Debug.Log("NetworkManager Start");
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings(); // 설정된 Photon 설정으로 서버에 연결
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(); // 마스터 서버에 연결되면 로비에 참가
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        TryJoinOrCreateRoom();
    }

    private void TryJoinOrCreateRoom()
    {
        Debug.Log("Trying to join a random room...");

        // 커스텀 룸 속성을 설정해서 인원이 2명이 아닌 방을 찾도록 지정
        Hashtable expectedCustomRoomProperties = new Hashtable() { { "IsFull", false } };

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No existing room found. Creating a new room.");

        // 무작위로 방 이름을 생성
        string roomName = GenerateRandomRoomName();

        // 방이 존재하지 않으면 새로 생성, 커스텀 속성 추가
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2,
            CustomRoomProperties = new Hashtable() { { "IsFull", false } }, // 방이 가득 차지 않았음을 나타내는 커스텀 속성
            CustomRoomPropertiesForLobby = new string[] { "IsFull" } // 이 속성을 로비에서 검색할 수 있도록 등록
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        Debug.Log("Created Room with Name: " + roomName);
    }

    // 무작위로 방 이름을 생성하는 함수
    private string GenerateRandomRoomName()
    {
        return "Room_" + System.Guid.NewGuid().ToString("N"); // 고유한 문자열을 생성
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("GameScene"); // 방에 입장하면 게임 씬으로 이동
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        if (scene.name == "GameScene")
        {
            CheckPlayerCountAndSpawn();
        }
    }

    private void CheckPlayerCountAndSpawn()
    {
        Debug.Log("Checking player count...");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Not the master client, flipping camera view.");
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has entered the room: " + newPlayer.NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            // 커스텀 속성 업데이트: 방이 가득 찼음을 표시
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "IsFull", true } });
        }

        CheckPlayerCountAndSpawn();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A player has left the room: " + otherPlayer.NickName);

        GameObject end = GameObject.Find("battlemgr");
        end.GetComponent<battlemgr>().win();
    }

    private void OnDestroy()
    {
        Debug.Log("NetworkManager OnDestroy");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

```

</details>

#02) 카드의 개수에 따른 시작 각도와 회전 각도 계산하여 부채꼴로 카드 정렬
<details>
<summary>예시 코드</summary>
  
```csharp

    public void ArrangeCardsInFanShape(GameObject[] cards)
    {
        int cardCount = cards.Length;
        float startAngle = -(angleRange * cardCount) / 2f;
        float angleStep = (angleRange * cardCount) / (cardCount - 1);
        if (cards.Length == 1) // 패가 한장일때
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

            if (cards.Length == 2) // 패가 2장일때
                handpos.transform.position = new Vector3(handpos.transform.position.x, -6.6f, handpos.transform.position.z);
            if (cards.Length == 3) // 패가 3장일때
                handpos.transform.position = new Vector3(handpos.transform.position.x, -7.9f, handpos.transform.position.z);
            if (cards.Length == 4) // 패가 4장일때
                handpos.transform.position = new Vector3(handpos.transform.position.x, -9.2f, handpos.transform.position.z);
            if (cards.Length == 5) // 패가 5장일때
                handpos.transform.position = new Vector3(handpos.transform.position.x, -10.5f, handpos.transform.position.z);
        }
    }

```

</details>

#03) 현재 코스트에 따라서 카드 드래그(사용 가능한 카드는 테두리를 초록색으로 표시)
<details>
<summary>예시</summary>
  
![TEST_1 2024-10-22 16-12-17](https://github.com/user-attachments/assets/00178606-f064-4e67-97fc-2d66a355ba97)
</details>

#04) 태그로 적합한 대상인지 확인후 카드를 효과를 적용할 배열로 이동
<details>
<summary>예시 코드</summary>
  
```csharp

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
                me.GetComponent<PlayerState>().cost -= 1; // 코스트 감소
                transform.Translate(1000f, 0f, 0f); // 화면밖으로 이동
                activeck = false; // 올바른 타켓에 드롭했는지 알기 위한 변수
                mgr.GetComponent<CardMgr>().RemoveCard(gameObject); // 손패에서 삭제
                battlemgr battleManager = FindObjectOfType<battlemgr>();
                if (dropTarget.GetComponent<PlayerState>() != null) // 대상이 플레이어 였을 경우
                {
                    // PlayerState 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.name;
                }
                else if (dropTarget.GetComponent<monstate>() != null)// 대상이 몬스터 였을 경우
                {
                    // monstate 스크립트를 가지고 있을 때
                    gameObject.GetComponent<Target>().drop = dropTarget.tag;
                }      
                battleManager.AddToAttack(this.gameObject); // 효과 적용 리스트에 추가
            }
        }
}
```

</details>

#05) OnPointerEnter와 OnPointerExit를 사용하여 마우스 커서가 따른 기능 추가
<details>
<summary>예시 코드</summary>
  
```csharp

     // 마우스 커서를 올렸을 때 실행되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 이미지 크기를 확대
        rectTransform.localScale = originalScale * scaleFactor;

        // 원래의 계층 순서를 저장하고, 캔버스에서 가장 앞으로 이동
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
    }

    // 마우스 커서가 이미지에서 벗어났을 때 실행되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // 이미지 크기를 원래대로 되돌림
        rectTransform.localScale = originalScale;

        // 원래의 계층 순서로 되돌림
        rectTransform.SetSiblingIndex(originalSiblingIndex);
    }
```
![TEST_1 2024-10-22 16-28-31](https://github.com/user-attachments/assets/f65e2abf-a3ea-4d98-b5b1-c27af22ba027)
</details>

#06) OnDestroy 메서드로 삭제시 효과 적용
<details>
<summary>예시 코드</summary>
  
```csharp

  void OnDestroy()
 {
     if (gameObject.GetComponent<Target>().drop == "opp_drop" || gameObject.GetComponent<Target>().drop == "me_drop") // 대상이 플레이어 일 경우
     {
         drop = GameObject.Find(gameObject.GetComponent<Target>().drop);
     }
     else // 대상이 몬스터일 경우
     {
         string targetTag = gameObject.GetComponent<Target>().drop; // drop 필드에 있는 값이 태그라고 가정
         drop = GameObject.FindWithTag(Swap(targetTag)); // 해당 태그를 가진 오브젝트를 찾음
     }

     // drop에 찾은 오브젝트가 있으면 ActivateEffect 호출
     if (drop != null)
     {
         ActivateEffect(drop);
     }
     else
     {
         Debug.LogError("Drop object not found!");
         battle.GetComponent<battlemgr>().applycker = false;
     }
 }


 public void ActivateEffect(GameObject target)
 {
     if (target.GetComponent<Target>().opcker == true) //내가 사용한 경우
     {
         a = me.GetComponent<PlayerState>().atk + 5;
     }
     else //상대가 사용한 경우
     {
         a = opp.GetComponent<PlayerState>().atk + 5;
     }

     // PlayerState 컴포넌트가 있는지 확인
     PlayerState playerState = target.GetComponent<PlayerState>();
     if (playerState != null)
     {
         // PlayerState가 있을 경우 실행
         if (playerState.shield > 0)
         {
             playerState.shield -= a;
         }
         else
         {
             playerState.hp -= a;
         }
     }
     else
     {
         // PlayerState가 없으면 monstate를 확인
         monstate monsterState = target.GetComponent<monstate>();
         if (monsterState != null)
         {
             // monstate가 있을 경우 실행
             if (monsterState.shield > 0)
             {
                 monsterState.shield -= a;
             }
             else
             {
                 monsterState.hp -= a;
             }
         }
         else
         {
             Debug.LogError("Target does not have PlayerState or monstate.");
         }
     }

     // Canvas 찾기
     GameObject canvasObject = GameObject.Find("Canvas");

     // 이펙트 로드
     GameObject CardEffectVFX = Resources.Load<GameObject>("vfx/vfx_1");

     // 타겟의 위치에 VFX 생성
     Vector3 spawnPosition = target.transform.position;
     GameObject effectInstance = Instantiate(CardEffectVFX, spawnPosition, Quaternion.identity, canvasObject.transform);
 }
```

</details>

#07) 전투 예시(효과를 적용하기 전 내가 사용한 카드는 오른쪽에 상대가 사용한 카드는 왼쪽에 크게 띄워줌)
<details>
<summary>예시</summary>
  
![TEST_1 2024-10-22 16-46-08](https://github.com/user-attachments/assets/42739d23-c1d8-495a-b950-b1eaa0d97814)

</details>

#08) 스폰 카드로 몬스터 객체를 생성하여 전투 보조
<details>
<summary>예시</summary>
  
![TEST_1 2024-10-22 17-34-43](https://github.com/user-attachments/assets/5f7eb1c7-f9f6-4635-acf4-b94a7e9d6792)

3개의 몬스터 존이 존재, 몬스터 카드 드래그시 몬스터존 활성화, 소환된 몬스터가 파괴될때까지 그 몬스터존 비활성화, 엔드페이즈에 소환된 몬스터들의 효과 적용 

![TEST_1 2024-10-22 17-51-38](https://github.com/user-attachments/assets/bd958188-8d79-4e41-9857-0a64dd7e4c3c)

몬스터는 채력과 방어력을 가지고 있으며 마우스 커서를 올릴시 효과창 활성화
</details>

#9) 스크롤뷰로 덱구축 구현
<details>
<summary>예시코드</summary>
  
```csharp
    public void OnClick()
    {
        ScrollRect parentScrollView = GetComponentInParent<ScrollRect>();

        if (parentScrollView.name == "bag")
        {
            if (mgr.GetComponent<scrollbtn>().maincker == 0)
            {
                // 현재 Scroll View의 콘텐츠가 30개 이상인지 확인
                if (targetScrollView.content.childCount >= 30)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(1);
                    return;
                }

                int count = 0;
                foreach (Transform child in targetScrollView.content)
                {
                    if (child.name == prefab.name)
                    {
                        count++;
                    }
                }

                if (count >= 3)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(2);
                  
                    return;
                }

                // 새로운 프리팹을 특정 Scroll View의 Content에 추가합니다.
                GameObject newItem = Instantiate(prefab, targetScrollView.content);
                newItem.transform.localScale = Vector3.one;
                newItem.name = prefab.name;
            }
            else if(mgr.GetComponent<scrollbtn>().maincker == 1)
            {
                // 현재 Scroll View의 콘텐츠가 30개 이상인지 확인
                if (targetScrollView2.content.childCount >= 30)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(1);
                    return;
                }

                int count = 0;
                foreach (Transform child in targetScrollView2.content)
                {
                    if (child.name == prefab.name)
                    {
                        count++;
                    }
                }

                if (count >= 3)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(2);
                    return;
                }

                // 새로운 프리팹을 특정 Scroll View의 Content에 추가합니다.
                GameObject newItem = Instantiate(prefab, targetScrollView2.content);
                newItem.transform.localScale = Vector3.one;
                newItem.name = prefab.name;
            }
        }
        else if (parentScrollView.name == "deck" || parentScrollView.name == "deck2")
        {
            Destroy(gameObject);
        }
    }
```

![TEST_1 2024-10-22 18-06-18 (1)](https://github.com/user-attachments/assets/3bbfaaa8-48b5-465d-8a51-da2bd6b04912)

</details>

#10) 턴종료시 손패가 5장이 되도록 뽑는 기능
<details>
<summary>예시코드</summary>
  
```csharp
    // 턴 종료시 드로우 하고 정렬
    public void DrawCardsAndArrange()
    {
        int currentHandCount = HandCard.Length;

        // 덱에 카드가 없는 경우 패배처리
        if (deck.Count == 0)
        {
            End(gameObject);
            return;
        }

        // HandCard가 5장이 될 때까지 덱에서 추가로 카드를 뽑음
        if (currentHandCount < 5)
        {
            int cardsToDraw = 5 - currentHandCount;

            // 덱에 남아있는 카드 수 확인하여 실제로 뽑을 카드 수 결정
            int actualCardsToDraw = (cardsToDraw < deck.Count) ? cardsToDraw : deck.Count;

            // 현재 HandCard 배열을 새로운 크기로 확장
            GameObject[] newHandCard = new GameObject[currentHandCount + actualCardsToDraw];
            for (int i = 0; i < currentHandCount; i++)
            {
                newHandCard[i] = HandCard[i];
            }

            // 부족한 카드만큼 덱에서 뽑아 HandCard에 추가
            for (int i = 0; i < actualCardsToDraw; i++)
            {
                newHandCard[currentHandCount + i] = SpawnCard(deck[0]);
                deck.RemoveAt(0); // 생성한 카드 제거
            }

            HandCard = newHandCard;
        }

        // 부채꼴 형태로 카드 정렬
        ArrangeCardsInFanShape(HandCard);
    }
```

</details>

#11) 아이콘 처리
<details>
<summary>예시코드</summary>
  
```csharp
 void Start()
 {
 
     itemStatus = new List<int> { 0, 0, 0, 0, 0 }; // 초기 상태 예시
     UpdateIcons();
 }

 public void UpdateIcons()
 {
     // 기존 아이콘 모두 삭제
     foreach (GameObject icon in activeIcons)
     {
         Destroy(icon);
     }
     activeIcons.Clear();

     // itemStatus에 따라 아이콘을 다시 추가
     for (int i = 0; i < itemStatus.Count; i++)
     {
         if (itemStatus[i] == 1) // 값이 1인 경우 공격증가 아이콘
         {
             GameObject newIcon = Instantiate(atk, panel);
             newIcon.tag = "myicon"; // 태그를 설정합니다.
             activeIcons.Add(newIcon);
         }
         else if (itemStatus[i] == 2) // 값이 2인 경우 민첩성증가 아이콘
         {
             GameObject newIcon = Instantiate(ag, panel);
             newIcon.tag = "myicon"; // 태그를 설정합니다.
             activeIcons.Add(newIcon);
         }
         else if (itemStatus[i] == 3) // 값이 3인 경우 화염 아이콘
         {
             GameObject newIcon = Instantiate(fire, panel);
             newIcon.tag = "myicon"; // 태그를 설정합니다.
             activeIcons.Add(newIcon);
         }
         else if (itemStatus[i] == 4)// 값이 3인 경우 독 아이콘
         {
             GameObject newIcon = Instantiate(poison, panel);
             newIcon.tag = "myicon"; // 태그를 설정합니다.
             activeIcons.Add(newIcon);
         }
     }

     // Horizontal Layout Group이 자동으로 아이콘을 정렬해줌
 }
 public void Update()
 {
     // atk가 0이 아니고 아직 처리되지 않은 경우 한 번만 실행
     if (me.GetComponent<PlayerState>().atk != 0 && !atkProcessed)
     {
         int firstZeroIndex = FindFirstZeroIndex();
         if (firstZeroIndex != -1) // 0인 요소가 있으면
         {
             ChangeItemStatus(firstZeroIndex, 1); // 해당 요소를 1로 변경
             lastatkIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
         }
         atkProcessed = true; // 실행 후 다시 실행되지 않도록 설정
     }

     // atk가 0으로 돌아오면 마지막으로 변경된 요소를 다시 0으로 되돌림
     if (me.GetComponent<PlayerState>().atk == 0 && atkProcessed)
     {
         if (lastatkIndex != -1) // 유효한 인덱스가 있는 경우
         {
             ChangeItemStatus(lastatkIndex, 0); // 해당 요소를 다시 0으로 변경
             lastatkIndex = -1; // 다시 초기화
         }
         atkProcessed = false; // 다시 처리할 수 있도록 설정
     }

     if (me.GetComponent<PlayerState>().agility != 0 && !agProcessed)
     {
         int firstZeroIndex = FindFirstZeroIndex();
         if (firstZeroIndex != -1) // 0인 요소가 있으면
         {
             ChangeItemStatus(firstZeroIndex, 2); // 해당 요소를 1로 변경
             lastagIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
         }
         agProcessed = true; // 실행 후 다시 실행되지 않도록 설정
     }

     if (me.GetComponent<PlayerState>().agility == 0 && agProcessed)
     {
         if (lastagIndex != -1) // 유효한 인덱스가 있는 경우
         {
             ChangeItemStatus(lastagIndex, 0); // 해당 요소를 다시 0으로 변경
             lastagIndex = -1; // 다시 초기화
         }
         agProcessed = false; // 다시 처리할 수 있도록 설정
     }

     // fire
     if (me.GetComponent<PlayerState>().fire != 0 && !fireProcessed)
     {
         int firstZeroIndex = FindFirstZeroIndex();
         if (firstZeroIndex != -1) // 0인 요소가 있으면
         {
             ChangeItemStatus(firstZeroIndex, 3); // 해당 요소를 1로 변경
             lastfireIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
         }
         fireProcessed = true; // 실행 후 다시 실행되지 않도록 설정
     }


     if (me.GetComponent<PlayerState>().fire == 0 && fireProcessed)
     {
         if (lastfireIndex != -1) // 유효한 인덱스가 있는 경우
         {
             ChangeItemStatus(lastfireIndex, 0); // 해당 요소를 다시 0으로 변경
             lastfireIndex = -1; // 다시 초기화
         }
         fireProcessed = false; // 다시 처리할 수 있도록 설정
     }

     // poison
     if (me.GetComponent<PlayerState>().poison != 0 && !poiProcessed)
     {
         int firstZeroIndex = FindFirstZeroIndex();
         if (firstZeroIndex != -1) // 0인 요소가 있으면
         {
             ChangeItemStatus(firstZeroIndex, 4); // 해당 요소를 1로 변경
             lastpoiIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
         }
         poiProcessed = true; // 실행 후 다시 실행되지 않도록 설정
     }


     if (me.GetComponent<PlayerState>().poison == 0 && poiProcessed)
     {
         if (lastpoiIndex != -1) // 유효한 인덱스가 있는 경우
         {
             ChangeItemStatus(lastpoiIndex, 0); // 해당 요소를 다시 0으로 변경
             lastpoiIndex = -1; // 다시 초기화
         }
         poiProcessed = false; // 다시 처리할 수 있도록 설정
     }
 }

 // 아이콘 상태를 업데이트하는 메서드
 public void ChangeItemStatus(int index, int newStatus)
 {
     if (index < 0 || index >= itemStatus.Count) return;
     itemStatus[index] = newStatus;
     UpdateIcons();
 }

 int FindFirstZeroIndex()
 {
     for (int i = 0; i < itemStatus.Count; i++)
     {
         if (itemStatus[i] == 0) // 값이 0인 요소를 찾음
         {
             return i; // 해당 인덱스를 반환
         }
     }
     return -1; // 0인 요소가 없으면 -1 반환
 }
```
![TEST_1 2024-10-25 17-19-53](https://github.com/user-attachments/assets/60cb5e2f-27f1-48cb-b44b-89ff11bd7c77)
</details>

#12) 승리, 패배처리
<details>
<summary>예시</summary>
  
![TEST_1 2024-10-25 17-31-31](https://github.com/user-attachments/assets/03a1da41-1f7c-46ad-b7ed-5bcccb22e57e)
승리

![image](https://github.com/user-attachments/assets/44a056eb-9b3b-46b4-9336-6ec82a378830)
패배

![TEST_1 2024-10-25 17-39-38](https://github.com/user-attachments/assets/c2fceb1d-74bd-4fa2-8da8-669f2d78d146)
무승부
</details>

#13) 종료, 설정 버튼(슬라이더를 사용한 볼륨조절)
<details>
<summary>예시</summary>
  
![TEST_1 2024-10-25 18-08-44](https://github.com/user-attachments/assets/921e9b38-f17f-499e-aa91-357d858a31fa)
</details>
