using UnityEngine;
using UnityEngine.UI;

public class save_btn : MonoBehaviour
{
    public ScrollRect scrollView;
    public ScrollRect scrollView2;
    public string playerPrefKey = "SavedNames1";  // ������ PlayerPrefs Ű
    public string playerPrefKey2 = "SavedNames2";  // ������ PlayerPrefs Ű
    public Button saveButton;  // �̸��� �����ϴ� ��ư
    public GameObject mgr;

    void Start()
    {
        // ��ư Ŭ�� �� SaveNames �޼ҵ带 ȣ��
        saveButton.onClick.AddListener(SaveNames);
    }

    void SaveNames()
    {
        mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(1);
        if (mgr.GetComponent<scrollbtn>().maincker == 0)
        {
            // ������ ����� �̸� ����
            PlayerPrefs.DeleteKey(playerPrefKey);

            // ��ũ�Ѻ信�� ��� ���� ������Ʈ�� �ڽĵ��� Ȯ��
            Transform content = scrollView.content;
            int childCount = content.childCount;

            // ��� �ڽ� ������Ʈ���� �̸��� �迭�� ����
            string[] names = new string[childCount];
            for (int i = 0; i < childCount; i++)
            {
                // �� �ڽ� ������Ʈ�� �̸����� 'deck'�� 'card'�� ����
                string objectName = content.GetChild(i).gameObject.name;
                names[i] = objectName.Replace("deck", "card");
            }

            // �̸����� ���ڿ��� ���ļ� PlayerPrefs�� ���� (��ǥ�� ����)
            string namesToSave = string.Join(",", names);
            PlayerPrefs.SetString(playerPrefKey, namesToSave);

            // ������ ��� �ݿ�
            PlayerPrefs.Save();

            mgr.GetComponent<textmanger>().ShowTextWithDelay(3);
        }

        if (mgr.GetComponent<scrollbtn>().maincker == 1)
        {
            PlayerPrefs.DeleteKey(playerPrefKey2);

            // ��ũ�Ѻ信�� ��� ���� ������Ʈ�� �ڽĵ��� Ȯ��
            Transform content = scrollView2.content;
            int childCount = content.childCount;

            // ��� �ڽ� ������Ʈ���� �̸��� �迭�� ����
            string[] names = new string[childCount];
            for (int i = 0; i < childCount; i++)
            {
                // �� �ڽ� ������Ʈ�� �̸����� 'deck'�� 'card'�� ����
                string objectName = content.GetChild(i).gameObject.name;
                names[i] = objectName.Replace("deck", "card");
            }

            // �̸����� ���ڿ��� ���ļ� PlayerPrefs�� ���� (��ǥ�� ����)
            string namesToSave = string.Join(",", names);
            PlayerPrefs.SetString(playerPrefKey2, namesToSave);

            // ������ ��� �ݿ�
            PlayerPrefs.Save();

            Debug.Log("Names saved (with deck replaced by card): " + namesToSave);
        }
    }
}
