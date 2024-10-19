using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager2 : MonoBehaviour
{
    // 아이콘의 원본 프리팹 (아이콘 이미지를 담은 UI 오브젝트)
    public GameObject atk;       // 기본 프리팹
    public GameObject ag; // 값이 2일 경우 생성될 프리팹
    public GameObject fire; // 값이 3일떄
    public GameObject poison;
    // 아이콘들이 배치될 패널 (Horizontal Layout Group이 적용된 Panel)
    public Transform panel;

    // 상태를 추적할 변수 리스트 (예: 아이템 상태)
    public List<int> itemStatus = new List<int>();

    // 패널에 표시될 아이콘 리스트
    private List<GameObject> activeIcons = new List<GameObject>();

    private bool atkProcessed = false;
    private bool agProcessed = false;
    private bool fireProcessed = false;
    private bool poiProcessed = false;
    private int lastatkIndex = -1;
    private int lastagIndex = -1;
    private int lastfireIndex = -1;
    private int lastpoiIndex = -1;

    public GameObject opp;

    void Start()
    {
        itemStatus = new List<int> { 0, 0, 0, 0, 0 }; // 초기 상태 예시
        UpdateIcons();
    }

    // 특정 변수가 변경될 때 이 메소드를 호출하여 아이콘을 갱신
    public void UpdateIcons()
    {
        // 기존 아이콘 모두 삭제
        foreach (GameObject icon in activeIcons)
        {
            Destroy(icon);
        }
        activeIcons.Clear();

        // itemStatus에 따라 아이콘을 다시 추가
        for (int i = 0; i < itemStatus.Count; i++)
        {
            if (itemStatus[i] == 1) // 값이 1인 경우 기본 아이콘 추가
            {
                GameObject newIcon = Instantiate(atk, panel);
                newIcon.tag = "oppicon"; // 태그를 설정합니다.
                activeIcons.Add(newIcon);
            }
            else if (itemStatus[i] == 2) // 값이 2인 경우 다른 프리팹 추가
            {
                GameObject newIcon = Instantiate(ag, panel);
                newIcon.tag = "oppicon"; // 태그를 설정합니다.
                activeIcons.Add(newIcon);
            }
            else if (itemStatus[i] == 3) // 값이 3인 경우 다른 프리팹 추가
            {
                GameObject newIcon = Instantiate(fire, panel);
                newIcon.tag = "oppicon"; // 태그를 설정합니다.
                activeIcons.Add(newIcon);
            }
            else if (itemStatus[i] == 4)// 값이 3인 경우 다른 프리팹 추가
            {
                GameObject newIcon = Instantiate(poison, panel);
                newIcon.tag = "oppicon"; // 태그를 설정합니다.
                activeIcons.Add(newIcon);
            }
        }

        // Horizontal Layout Group이 자동으로 아이콘을 정렬해줌
    }
    public void Update()
    {
        // atk가 0이 아니고 아직 처리되지 않은 경우 한 번만 실행
        if (opp.GetComponent<PlayerState>().atk != 0 && !atkProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0인 요소가 있으면
            {
                ChangeItemStatus(firstZeroIndex, 1); // 해당 요소를 1로 변경
                lastatkIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
            }
            atkProcessed = true; // 실행 후 다시 실행되지 않도록 설정
        }

        // atk가 0으로 돌아오면 마지막으로 변경된 요소를 다시 0으로 되돌림
        if (opp.GetComponent<PlayerState>().atk == 0 && atkProcessed)
        {
            if (lastatkIndex != -1) // 유효한 인덱스가 있는 경우
            {
                ChangeItemStatus(lastatkIndex, 0); // 해당 요소를 다시 0으로 변경
                lastatkIndex = -1; // 다시 초기화
            }
            atkProcessed = false; // 다시 처리할 수 있도록 설정
        }

        if (opp.GetComponent<PlayerState>().agility != 0 && !agProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0인 요소가 있으면
            {
                ChangeItemStatus(firstZeroIndex, 2); // 해당 요소를 1로 변경
                lastagIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
            }
            agProcessed = true; // 실행 후 다시 실행되지 않도록 설정
        }

        if (opp.GetComponent<PlayerState>().agility == 0 && agProcessed)
        {
            if (lastagIndex != -1) // 유효한 인덱스가 있는 경우
            {
                ChangeItemStatus(lastagIndex, 0); // 해당 요소를 다시 0으로 변경
                lastagIndex = -1; // 다시 초기화
            }
            agProcessed = false; // 다시 처리할 수 있도록 설정
        }

        // fire
        if (opp.GetComponent<PlayerState>().fire != 0 && !fireProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0인 요소가 있으면
            {
                ChangeItemStatus(firstZeroIndex, 3); // 해당 요소를 1로 변경
                lastfireIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
            }
            fireProcessed = true; // 실행 후 다시 실행되지 않도록 설정
        }


        if (opp.GetComponent<PlayerState>().fire == 0)
        {
            if (lastfireIndex != -1) // 유효한 인덱스가 있는 경우
            {
                ChangeItemStatus(lastfireIndex, 0); // 해당 요소를 다시 0으로 변경
                lastfireIndex = -1; // 다시 초기화
            }
            fireProcessed = false; // 다시 처리할 수 있도록 설정
        }

        // poison
        if (opp.GetComponent<PlayerState>().poison != 0 && !poiProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0인 요소가 있으면
            {
                ChangeItemStatus(firstZeroIndex, 4); 
                lastpoiIndex = firstZeroIndex; // 마지막에 변경된 인덱스 기록
            }
            poiProcessed = true; // 실행 후 다시 실행되지 않도록 설정
        }


        if (opp.GetComponent<PlayerState>().poison == 0 && poiProcessed)
        {
            if (lastpoiIndex != -1) // 유효한 인덱스가 있는 경우
            {
                ChangeItemStatus(lastpoiIndex, 0); // 해당 요소를 다시 0으로 변경
                lastpoiIndex = -1; // 다시 초기화
            }
            poiProcessed = false; // 다시 처리할 수 있도록 설정
        }
    }

    // 아이콘 상태를 업데이트하는 메서드
    public void ChangeItemStatus(int index, int newStatus)
    {
        if (index < 0 || index >= itemStatus.Count) return;
        itemStatus[index] = newStatus;
        UpdateIcons();
    }

    int FindFirstZeroIndex()
    {
        for (int i = 0; i < itemStatus.Count; i++)
        {
            if (itemStatus[i] == 0) // 값이 0인 요소를 찾음
            {
                return i; // 해당 인덱스를 반환
            }
        }
        return -1; // 0인 요소가 없으면 -1 반환
    }
}
