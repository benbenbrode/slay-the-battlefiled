using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network : MonoBehaviourPunCallbacks
{
    public static Network Instance;
    public int type = 0;
    public int type2 = 0;
    public List<string> deck;
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
        TryJoinOrCreateRoom("game");
    }

    private void TryJoinOrCreateRoom(string roomName)
    {
        Debug.Log("Trying to join or create room: " + roomName);

        // ���� ã�� ���� �����ϸ� ����, �ƴϸ� ���ο� ���� ����
        PhotonNetwork.JoinRandomRoom(null, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No existing room found. Creating a new room.");

        // ���� �������� ������ ���� ����
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };

        PhotonNetwork.CreateRoom("game", roomOptions, TypedLobby.Default);
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
            Debug.Log("�濡 ������");
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Not the master client, flipping camera view.");
                //FlipCameraView();
            }
        }
    }

    //private void FlipCameraView()
    //{
    //    Camera camera = Camera.main;
    //    if (camera != null)
    //    {
    //        Matrix4x4 matrix = camera.projectionMatrix;
    //        matrix.m01 = -matrix.m01; // Y�� ����
    //        matrix.m11 = -matrix.m11; // Y�� ����
    //        camera.projectionMatrix = matrix;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Main camera not found.");
    //    }
    //}

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has entered the room: " + newPlayer.NickName);
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {

            CheckPlayerCountAndSpawn();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A player has left the room: " + otherPlayer.NickName);
    }

    private void OnDestroy()
    {
        Debug.Log("NetworkManager OnDestroy");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
