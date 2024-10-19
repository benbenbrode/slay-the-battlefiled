using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class icon_poi : MonoBehaviour
{
    private Text txt;
    private GameObject game;

    void Start()
    {
        // �ڽ� ������Ʈ �� �̸��� "text"�� ������Ʈ���� Text ������Ʈ�� �����ɴϴ�.
        Transform textChild = transform.Find("text");
        if (textChild != null)
        {
            txt = textChild.GetComponent<Text>();
            if (txt == null)
            {
                Debug.LogError("�ڽ� ������Ʈ 'text'�� Text ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("�̸��� 'text'�� �ڽ� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // �� ������Ʈ�� �±׸� Ȯ���Ͽ� 'game' ������ ������ GameObject�� �Ҵ��մϴ�.
        if (gameObject.tag == "myicon")
        {
            game = GameObject.Find("me_drop");
            if (game == null)
            {
                Debug.LogError("me_drop�̶�� �̸��� GameObject�� ã�� �� �����ϴ�.");
            }
        }
        else if (gameObject.tag == "oppicon")
        {
            game = GameObject.Find("opp_drop");
            if (game == null)
            {
                Debug.LogError("opp_drop�̶�� �̸��� GameObject�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("�� ��ũ��Ʈ�� ������ GameObject�� �±װ� 'myicon' �Ǵ� 'oppicon'�� �ƴմϴ�.");
        }
    }

    void Update()
    {
        if (txt != null && game != null)
        {
            PlayerState playerState = game.GetComponent<PlayerState>();
            if (playerState != null)
            {
                txt.text = playerState.poison.ToString();
            }
            else
            {
                Debug.LogError("GameObject�� PlayerState ������Ʈ�� �����ϴ�.");
            }
        }
    }
}
