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

사용 엔진 : UNITY 2022.3.15f1
---

주요 활용 기술
---
* #01) 포톤 네트워크를 이용한 랜덤매칭 시스템 구현
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

        // 플레이어가 방을 나가면 다시 방을 "가득 차지 않음"으로 표시
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "IsFull", false } });
    }

    private void OnDestroy()
    {
        Debug.Log("NetworkManager OnDestroy");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

```

</details>
