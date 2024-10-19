using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager2 : MonoBehaviour
{
    // �������� ���� ������ (������ �̹����� ���� UI ������Ʈ)
    public GameObject atk;       // �⺻ ������
    public GameObject ag; // ���� 2�� ��� ������ ������
    public GameObject fire; // ���� 3�ϋ�
    public GameObject poison;
    // �����ܵ��� ��ġ�� �г� (Horizontal Layout Group�� ����� Panel)
    public Transform panel;

    // ���¸� ������ ���� ����Ʈ (��: ������ ����)
    public List<int> itemStatus = new List<int>();

    // �гο� ǥ�õ� ������ ����Ʈ
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
        itemStatus = new List<int> { 0, 0, 0, 0, 0 }; // �ʱ� ���� ����
        UpdateIcons();
    }

    // Ư�� ������ ����� �� �� �޼ҵ带 ȣ���Ͽ� �������� ����
    public void UpdateIcons()
    {
        // ���� ������ ��� ����
        foreach (GameObject icon in activeIcons)
        {
            Destroy(icon);
        }
        activeIcons.Clear();

        // itemStatus�� ���� �������� �ٽ� �߰�
        for (int i = 0; i < itemStatus.Count; i++)
        {
            if (itemStatus[i] == 1) // ���� 1�� ��� �⺻ ������ �߰�
            {
                GameObject newIcon = Instantiate(atk, panel);
                newIcon.tag = "oppicon"; // �±׸� �����մϴ�.
                activeIcons.Add(newIcon);
            }
            else if (itemStatus[i] == 2) // ���� 2�� ��� �ٸ� ������ �߰�
            {
                GameObject newIcon = Instantiate(ag, panel);
                newIcon.tag = "oppicon"; // �±׸� �����մϴ�.
                activeIcons.Add(newIcon);
            }
            else if (itemStatus[i] == 3) // ���� 3�� ��� �ٸ� ������ �߰�
            {
                GameObject newIcon = Instantiate(fire, panel);
                newIcon.tag = "oppicon"; // �±׸� �����մϴ�.
                activeIcons.Add(newIcon);
            }
            else if (itemStatus[i] == 4)// ���� 3�� ��� �ٸ� ������ �߰�
            {
                GameObject newIcon = Instantiate(poison, panel);
                newIcon.tag = "oppicon"; // �±׸� �����մϴ�.
                activeIcons.Add(newIcon);
            }
        }

        // Horizontal Layout Group�� �ڵ����� �������� ��������
    }
    public void Update()
    {
        // atk�� 0�� �ƴϰ� ���� ó������ ���� ��� �� ���� ����
        if (opp.GetComponent<PlayerState>().atk != 0 && !atkProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0�� ��Ұ� ������
            {
                ChangeItemStatus(firstZeroIndex, 1); // �ش� ��Ҹ� 1�� ����
                lastatkIndex = firstZeroIndex; // �������� ����� �ε��� ���
            }
            atkProcessed = true; // ���� �� �ٽ� ������� �ʵ��� ����
        }

        // atk�� 0���� ���ƿ��� ���������� ����� ��Ҹ� �ٽ� 0���� �ǵ���
        if (opp.GetComponent<PlayerState>().atk == 0 && atkProcessed)
        {
            if (lastatkIndex != -1) // ��ȿ�� �ε����� �ִ� ���
            {
                ChangeItemStatus(lastatkIndex, 0); // �ش� ��Ҹ� �ٽ� 0���� ����
                lastatkIndex = -1; // �ٽ� �ʱ�ȭ
            }
            atkProcessed = false; // �ٽ� ó���� �� �ֵ��� ����
        }

        if (opp.GetComponent<PlayerState>().agility != 0 && !agProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0�� ��Ұ� ������
            {
                ChangeItemStatus(firstZeroIndex, 2); // �ش� ��Ҹ� 1�� ����
                lastagIndex = firstZeroIndex; // �������� ����� �ε��� ���
            }
            agProcessed = true; // ���� �� �ٽ� ������� �ʵ��� ����
        }

        if (opp.GetComponent<PlayerState>().agility == 0 && agProcessed)
        {
            if (lastagIndex != -1) // ��ȿ�� �ε����� �ִ� ���
            {
                ChangeItemStatus(lastagIndex, 0); // �ش� ��Ҹ� �ٽ� 0���� ����
                lastagIndex = -1; // �ٽ� �ʱ�ȭ
            }
            agProcessed = false; // �ٽ� ó���� �� �ֵ��� ����
        }

        // fire
        if (opp.GetComponent<PlayerState>().fire != 0 && !fireProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0�� ��Ұ� ������
            {
                ChangeItemStatus(firstZeroIndex, 3); // �ش� ��Ҹ� 1�� ����
                lastfireIndex = firstZeroIndex; // �������� ����� �ε��� ���
            }
            fireProcessed = true; // ���� �� �ٽ� ������� �ʵ��� ����
        }


        if (opp.GetComponent<PlayerState>().fire == 0)
        {
            if (lastfireIndex != -1) // ��ȿ�� �ε����� �ִ� ���
            {
                ChangeItemStatus(lastfireIndex, 0); // �ش� ��Ҹ� �ٽ� 0���� ����
                lastfireIndex = -1; // �ٽ� �ʱ�ȭ
            }
            fireProcessed = false; // �ٽ� ó���� �� �ֵ��� ����
        }

        // poison
        if (opp.GetComponent<PlayerState>().poison != 0 && !poiProcessed)
        {
            int firstZeroIndex = FindFirstZeroIndex();
            if (firstZeroIndex != -1) // 0�� ��Ұ� ������
            {
                ChangeItemStatus(firstZeroIndex, 4); 
                lastpoiIndex = firstZeroIndex; // �������� ����� �ε��� ���
            }
            poiProcessed = true; // ���� �� �ٽ� ������� �ʵ��� ����
        }


        if (opp.GetComponent<PlayerState>().poison == 0 && poiProcessed)
        {
            if (lastpoiIndex != -1) // ��ȿ�� �ε����� �ִ� ���
            {
                ChangeItemStatus(lastpoiIndex, 0); // �ش� ��Ҹ� �ٽ� 0���� ����
                lastpoiIndex = -1; // �ٽ� �ʱ�ȭ
            }
            poiProcessed = false; // �ٽ� ó���� �� �ֵ��� ����
        }
    }

    // ������ ���¸� ������Ʈ�ϴ� �޼���
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
            if (itemStatus[i] == 0) // ���� 0�� ��Ҹ� ã��
            {
                return i; // �ش� �ε����� ��ȯ
            }
        }
        return -1; // 0�� ��Ұ� ������ -1 ��ȯ
    }
}
