using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리 관련 네임스페이스
using UnityEngine.UI;  // UI 관련 네임스페이스

public class lobbytodeck : MonoBehaviour
{
    public Button yourButton;  // 버튼을 인스펙터에서 연결

    void Start()
    {
        // 버튼 클릭 이벤트에 메소드 연결
        yourButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // "lobby"라는 씬으로 이동
        SceneManager.LoadScene("deck");
    }
}