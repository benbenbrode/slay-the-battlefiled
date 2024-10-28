using UnityEngine;
using UnityEngine.UI;

public class save_btn : MonoBehaviour
{
    public ScrollRect scrollView;
    public ScrollRect scrollView2;
    public string playerPrefKey = "SavedNames1";  // 저장할 PlayerPrefs 키
    public string playerPrefKey2 = "SavedNames2";  // 저장할 PlayerPrefs 키
    public Button saveButton;  // 이름을 저장하는 버튼
    public GameObject mgr;

    void Start()
    {
        // 버튼 클릭 시 SaveNames 메소드를 호출
        saveButton.onClick.AddListener(SaveNames);
    }

    void SaveNames()
    {
        mgr.GetComponent<sound_mgr>().PlaySoundBasedOnCondition(1);
        if (mgr.GetComponent<scrollbtn>().maincker == 0)
        {
            // 기존에 저장된 이름 삭제
            PlayerPrefs.DeleteKey(playerPrefKey);

            // 스크롤뷰에서 모든 게임 오브젝트의 자식들을 확인
            Transform content = scrollView.content;
            int childCount = content.childCount;

            // 모든 자식 오브젝트들의 이름을 배열로 저장
            string[] names = new string[childCount];
            for (int i = 0; i < childCount; i++)
            {
                // 각 자식 오브젝트의 이름에서 'deck'을 'card'로 변경
                string objectName = content.GetChild(i).gameObject.name;
                names[i] = objectName.Replace("deck", "card");
            }

            // 이름들을 문자열로 합쳐서 PlayerPrefs에 저장 (쉼표로 구분)
            string namesToSave = string.Join(",", names);
            PlayerPrefs.SetString(playerPrefKey, namesToSave);

            // 저장을 즉시 반영
            PlayerPrefs.Save();

            mgr.GetComponent<textmanger>().ShowTextWithDelay(3);
        }

        if (mgr.GetComponent<scrollbtn>().maincker == 1)
        {
            PlayerPrefs.DeleteKey(playerPrefKey2);

            // 스크롤뷰에서 모든 게임 오브젝트의 자식들을 확인
            Transform content = scrollView2.content;
            int childCount = content.childCount;

            // 모든 자식 오브젝트들의 이름을 배열로 저장
            string[] names = new string[childCount];
            for (int i = 0; i < childCount; i++)
            {
                // 각 자식 오브젝트의 이름에서 'deck'을 'card'로 변경
                string objectName = content.GetChild(i).gameObject.name;
                names[i] = objectName.Replace("deck", "card");
            }

            // 이름들을 문자열로 합쳐서 PlayerPrefs에 저장 (쉼표로 구분)
            string namesToSave = string.Join(",", names);
            PlayerPrefs.SetString(playerPrefKey2, namesToSave);

            // 저장을 즉시 반영
            PlayerPrefs.Save();

            Debug.Log("Names saved (with deck replaced by card): " + namesToSave);
        }
    }
}
