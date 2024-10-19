using UnityEngine;
using UnityEngine.UI;


public class scrollbtn : MonoBehaviour
{
    public ScrollRect targetScrollView; // ��� Scroll View
    public Button button; // ������ ��ư
    public Sprite activeImage; // ��ư Ȱ��ȭ �� ����� �̹���
    public Sprite inactiveImage; // ��ư ��Ȱ��ȭ �� ����� �̹���
    public Text buttonText; // ��ư�� �ִ� �ؽ�Ʈ
    public Button wa;
    public Button s;

    public GameObject deck;  // deck ������Ʈ
    public GameObject deck2; // deck2 ������Ʈ

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

                // Ư�� ������ ���� ��ũ��Ʈ�� ������ (��: StatusChecker)
                deckclick deckclick = childObject.GetComponent<deckclick>();

                // ��ũ��Ʈ�� �����ϰ�, �� Ư�� ������ 1�� ��� ������Ʈ�� ��Ȱ��ȭ
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

                // Ư�� ������ ���� ��ũ��Ʈ�� ������ (��: StatusChecker)
                deckclick deckclick = childObject.GetComponent<deckclick>();

                // ��ũ��Ʈ�� �����ϰ�, �� Ư�� ������ 1�� ��� ������Ʈ�� ��Ȱ��ȭ
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
        // maincker ���� ���� deck�� deck2 Ȱ��/��Ȱ��ȭ
        if (maincker == 0)
        {
            deck.SetActive(true);  // deck Ȱ��ȭ
            deck2.SetActive(false); // deck2 ��Ȱ��ȭ

            wa.interactable = false;
            s.interactable = true;

            targetScrollView = deck.GetComponent<ScrollRect>();
        }
        else if (maincker == 1)
        {
            deck.SetActive(false);  // deck ��Ȱ��ȭ
            deck2.SetActive(true); // deck2 Ȱ��ȭ

            wa.interactable = true;
            s.interactable = false;

            targetScrollView = deck2.GetComponent<ScrollRect>();
        }

        // Scroll View�� �������� 30�� �̻����� Ȯ��
        if (targetScrollView != null && targetScrollView.content.childCount >= 30)
        {
            // ��ư Ȱ��ȭ �� �̹��� ����
            button.interactable = true;
            button.image.sprite = activeImage;

            // ��ư �ؽ�Ʈ Ȱ��ȭ
            buttonText.enabled = true;
        }
        else
        {
            // ��ư ��Ȱ��ȭ �� �̹��� ����
            button.interactable = false;
            button.image.sprite = inactiveImage;

            // ��ư �ؽ�Ʈ ��Ȱ��ȭ
            buttonText.enabled = false;
        }
    }
}
