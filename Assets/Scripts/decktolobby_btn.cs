using UnityEngine;
using UnityEngine.SceneManagement;  // �� ���� ���� ���ӽ����̽�
using UnityEngine.UI;  // UI ���� ���ӽ����̽�
using Photon.Pun;  // Photon PUN ���� ���ӽ����̽�
using Photon.Realtime;  // Photon Realtime ���� ���ӽ����̽�

public class decktolobby : MonoBehaviourPunCallbacks
{
    public Button yourButton;  // ��ư�� �ν����Ϳ��� ����

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� �޼ҵ� ����
        yourButton.onClick.AddListener(OnButtonClick);
        PhotonNetwork.Disconnect();
    }

    void OnButtonClick()
    {
        // "netmgr(Clone)" ������Ʈ�� ã��
        GameObject netMgrObject = GameObject.Find("netmgr(Clone)");

        // ������Ʈ�� �����ϸ� ����
        if (netMgrObject != null)
        {
            Destroy(netMgrObject);
            Debug.Log("netmgr(Clone) object found and destroyed.");
        }
        else
        {
            Debug.Log("netmgr(Clone) object not found.");
        }

        SceneManager.LoadScene("lobby");
    }

}
