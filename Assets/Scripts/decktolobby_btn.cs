using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리 관련 네임스페이스
using UnityEngine.UI;  // UI 관련 네임스페이스
using Photon.Pun;  // Photon PUN 관련 네임스페이스
using Photon.Realtime;  // Photon Realtime 관련 네임스페이스

public class decktolobby : MonoBehaviourPunCallbacks
{
    public Button yourButton;  // 버튼을 인스펙터에서 연결

    void Start()
    {
        // 버튼 클릭 이벤트에 메소드 연결
        yourButton.onClick.AddListener(OnButtonClick);
        PhotonNetwork.Disconnect();
    }

    void OnButtonClick()
    {
        // "netmgr(Clone)" 오브젝트를 찾음
        GameObject netMgrObject = GameObject.Find("netmgr(Clone)");

        // 오브젝트가 존재하면 삭제
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
