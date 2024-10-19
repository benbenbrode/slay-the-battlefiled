using UnityEngine;
using UnityEngine.UI;


public class scrollbtn : MonoBehaviour
{
    public ScrollRect targetScrollView; // 대상 Scroll View
    public Button button; // 제어할 버튼
    public Sprite activeImage; // 버튼 활성화 시 사용할 이미지
    public Sprite inactiveImage; // 버튼 비활성화 시 사용할 이미지
    public Text buttonText; // 버튼에 있는 텍스트
    public Button wa;
    public Button s;

    public GameObject deck;  // deck 오브젝트
    public GameObject deck2; // deck2 오브젝트

    public int maincker = 0;

    public ScrollRect bag;
    void Start()
    {
        UpdateScrollView();
    }

    void Update()
    {
        UpdateScrollView();
        if(maincker == 0)
        {
            Transform content = bag.content;
            int childCount = content.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject childObject = content.GetChild(i).gameObject;

                // 특정 변수를 가진 스크립트를 가져옴 (예: StatusChecker)
                deckclick deckclick = childObject.GetComponent<deckclick>();

                // 스크립트가 존재하고, 그 특정 변수가 1일 경우 오브젝트를 비활성화
                if (deckclick != null && deckclick.job == 1)
                {
                    childObject.SetActive(true);  
                }
                if (deckclick != null && deckclick.job == 2)
                {
                    childObject.SetActive(false);
                }
            }
        }

        if (maincker == 1)
        {
            Transform content = bag.content;
            int childCount = content.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject childObject = content.GetChild(i).gameObject;

                // 특정 변수를 가진 스크립트를 가져옴 (예: StatusChecker)
                deckclick deckclick = childObject.GetComponent<deckclick>();

                // 스크립트가 존재하고, 그 특정 변수가 1일 경우 오브젝트를 비활성화
                if (deckclick != null && deckclick.job == 2)
                {
                    childObject.SetActive(true);
                }
                if (deckclick != null && deckclick.job == 1)
                {
                    childObject.SetActive(false);
                }
            }
        }

    }

    void UpdateScrollView()
    {
        // maincker 값에 따라 deck과 deck2 활성/비활성화
        if (maincker == 0)
        {
            deck.SetActive(true);  // deck 활성화
            deck2.SetActive(false); // deck2 비활성화

            wa.interactable = false;
            s.interactable = true;

            targetScrollView = deck.GetComponent<ScrollRect>();
        }
        else if (maincker == 1)
        {
            deck.SetActive(false);  // deck 비활성화
            deck2.SetActive(true); // deck2 활성화

            wa.interactable = true;
            s.interactable = false;

            targetScrollView = deck2.GetComponent<ScrollRect>();
        }

        // Scroll View의 콘텐츠가 30개 이상인지 확인
        if (targetScrollView != null && targetScrollView.content.childCount >= 30)
        {
            // 버튼 활성화 및 이미지 변경
            button.interactable = true;
            button.image.sprite = activeImage;

            // 버튼 텍스트 활성화
            buttonText.enabled = true;
        }
        else
        {
            // 버튼 비활성화 및 이미지 변경
            button.interactable = false;
            button.image.sprite = inactiveImage;

            // 버튼 텍스트 비활성화
            buttonText.enabled = false;
        }
    }
}
