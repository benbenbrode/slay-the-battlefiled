using UnityEngine;

public class QUIT_BTN : MonoBehaviour
{
    // ���� ���� �޼���
    public void Quit()
    {
        // ������ �����մϴ�.
        Application.Quit();

        // ����Ƽ �����Ϳ����� ������� �����Ƿ� ����� �޽����� ����մϴ�.
        Debug.Log("������ ����Ǿ����ϴ�.");
    }
}

