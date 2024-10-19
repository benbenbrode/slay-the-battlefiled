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
        // 자식 오브젝트 중 이름이 "text"인 오브젝트에서 Text 컴포넌트를 가져옵니다.
        Transform textChild = transform.Find("text");
        if (textChild != null)
        {
            txt = textChild.GetComponent<Text>();
            if (txt == null)
            {
                Debug.LogError("자식 오브젝트 'text'에 Text 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("이름이 'text'인 자식 오브젝트를 찾을 수 없습니다.");
        }

        // 이 오브젝트의 태그를 확인하여 'game' 변수에 적절한 GameObject를 할당합니다.
        if (gameObject.tag == "myicon")
        {
            game = GameObject.Find("me_drop");
            if (game == null)
            {
                Debug.LogError("me_drop이라는 이름의 GameObject를 찾을 수 없습니다.");
            }
        }
        else if (gameObject.tag == "oppicon")
        {
            game = GameObject.Find("opp_drop");
            if (game == null)
            {
                Debug.LogError("opp_drop이라는 이름의 GameObject를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("이 스크립트가 부착된 GameObject의 태그가 'myicon' 또는 'oppicon'이 아닙니다.");
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
                Debug.LogError("GameObject에 PlayerState 컴포넌트가 없습니다.");
            }
        }
    }
}
