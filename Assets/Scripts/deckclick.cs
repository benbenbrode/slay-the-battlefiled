using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class deckclick : MonoBehaviour
{
    public ScrollRect targetScrollView;
    public ScrollRect targetScrollView2;
    public GameObject mgr;
    public GameObject prefab;
    public int job = 99;


    public void OnClick()
    {
        ScrollRect parentScrollView = GetComponentInParent<ScrollRect>();

        if (parentScrollView.name == "bag")
        {
            if (mgr.GetComponent<scrollbtn>().maincker == 0)
            {
                // ���� Scroll View�� �������� 30�� �̻����� Ȯ��
                if (targetScrollView.content.childCount >= 30)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(1);
                    return;
                }

                int count = 0;
                foreach (Transform child in targetScrollView.content)
                {
                    if (child.name == prefab.name)
                    {
                        count++;
                    }
                }

                if (count >= 3)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(2);
                  
                    return;
                }

                // ���ο� �������� Ư�� Scroll View�� Content�� �߰��մϴ�.
                GameObject newItem = Instantiate(prefab, targetScrollView.content);
                newItem.transform.localScale = Vector3.one;
                newItem.name = prefab.name;
            }
            else if(mgr.GetComponent<scrollbtn>().maincker == 1)
            {
                // ���� Scroll View�� �������� 30�� �̻����� Ȯ��
                if (targetScrollView2.content.childCount >= 30)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(1);
                    return;
                }

                int count = 0;
                foreach (Transform child in targetScrollView2.content)
                {
                    if (child.name == prefab.name)
                    {
                        count++;
                    }
                }

                if (count >= 3)
                {
                    mgr.GetComponent<textmanger>().ShowTextWithDelay(2);
                    return;
                }

                // ���ο� �������� Ư�� Scroll View�� Content�� �߰��մϴ�.
                GameObject newItem = Instantiate(prefab, targetScrollView2.content);
                newItem.transform.localScale = Vector3.one;
                newItem.name = prefab.name;
            }
        }
        else if (parentScrollView.name == "deck" || parentScrollView.name == "deck2")
        {
            Destroy(gameObject);
        }
    }
}
