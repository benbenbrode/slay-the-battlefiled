using UnityEngine;
using UnityEngine.SceneManagement;  // �� ���� ���� ���ӽ����̽�
using UnityEngine.UI;  // UI ���� ���ӽ����̽�

public class lobbytodeck : MonoBehaviour
{
    public Button yourButton;  // ��ư�� �ν����Ϳ��� ����

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� �޼ҵ� ����
        yourButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // "lobby"��� ������ �̵�
        SceneManager.LoadScene("deck");
    }
}