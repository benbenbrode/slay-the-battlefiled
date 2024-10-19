using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class turnblock : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 방에 접속하면 현재 플레이어 수를 확인
        CheckPlayerCount();
    }

    // 새로운 플레이어가 방에 들어왔을 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckPlayerCount();
    }

    // 방에 있는 플레이어 수를 체크하는 메서드
    private void CheckPlayerCount()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Destroy(gameObject); // 2명 이상의 플레이어가 있으면 자신을 파괴
        }
    }
}
