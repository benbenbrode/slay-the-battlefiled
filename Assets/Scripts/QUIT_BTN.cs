using UnityEngine;

public class QUIT_BTN : MonoBehaviour
{
    // 게임 종료 메서드
    public void Quit()
    {
        // 게임을 종료합니다.
        Application.Quit();

        // 유니티 에디터에서는 종료되지 않으므로 디버그 메시지를 출력합니다.
        Debug.Log("게임이 종료되었습니다.");
    }
}

