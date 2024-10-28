using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon; // Ŀ���� �� �Ӽ� ����� ���� ���ӽ����̽�
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network : MonoBehaviourPunCallbacks
{
    public static Network Instance;
    public int type = 0; // �� ����
    public int type2 = 0; // ��� ����
    public List<string> deck; // �� ��

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
        PhotonNetwork.ConnectUsingSettings(); // ������ Photon �������� ������ ����
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(); // ������ ������ ����Ǹ� �κ� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        TryJoinOrCreateRoom();
    }

    private void TryJoinOrCreateRoom()
    {
        Debug.Log("Trying to join a random room...");

        // Ŀ���� �� �Ӽ��� �����ؼ� �ο��� 2���� �ƴ� ���� ã���� ����
        Hashtable expectedCustomRoomProperties = new Hashtable() { { "IsFull", false } };

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No existing room found. Creating a new room.");

        // �������� �� �̸��� ����
        string roomName = GenerateRandomRoomName();

        // ���� �������� ������ ���� ����, Ŀ���� �Ӽ� �߰�
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2,
            CustomRoomProperties = new Hashtable() { { "IsFull", false } }, // ���� ���� ���� �ʾ����� ��Ÿ���� Ŀ���� �Ӽ�
            CustomRoomPropertiesForLobby = new string[] { "IsFull" } // �� �Ӽ��� �κ񿡼� �˻��� �� �ֵ��� ���
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        Debug.Log("Created Room with Name: " + roomName);
    }

    // �������� �� �̸��� �����ϴ� �Լ�
    private string GenerateRandomRoomName()
    {
        return "Room_" + System.Guid.NewGuid().ToString("N"); // ������ ���ڿ��� ����
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("GameScene"); // �濡 �����ϸ� ���� ������ �̵�
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
            // Ŀ���� �Ӽ� ������Ʈ: ���� ���� á���� ǥ��
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
