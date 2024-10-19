using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class turnblock : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // �濡 �����ϸ� ���� �÷��̾� ���� Ȯ��
        CheckPlayerCount();
    }

    // ���ο� �÷��̾ �濡 ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckPlayerCount();
    }

    // �濡 �ִ� �÷��̾� ���� üũ�ϴ� �޼���
    private void CheckPlayerCount()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Destroy(gameObject); // 2�� �̻��� �÷��̾ ������ �ڽ��� �ı�
        }
    }
}
