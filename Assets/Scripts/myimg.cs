using UnityEngine;
using UnityEngine.UI;

public class myimg : MonoBehaviour
{
    public Sprite w;
    public Sprite s;
    private Image image;            // UI Image ������Ʈ ����
    public GameObject net;

    void Start()
    {
        net = GameObject.Find("netmgr(Clone)");
        // ���� ������Ʈ�� Image ������Ʈ ��������
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("Image component not found!");
        }
    }

    void Update()
    {
        // Ư�� ������ 1�� �Ǿ��� ��
        if (net.GetComponent<Network>().type == 1)
        {
            // ���İ��� 255�� ���� (1�� ����: Color�� ���Ĵ� 0~1 �����̹Ƿ� 255�� 1�� �ش�)
            Color color = image.color;
            color.a = 1f; // ���İ��� 255�� �ش��ϴ� 1�� ����
            image.color = color;
            image.sprite = w;
        }
        else if (net.GetComponent<Network>().type == 2)
        {
            // ���İ��� 255�� ���� (1�� ����: Color�� ���Ĵ� 0~1 �����̹Ƿ� 255�� 1�� �ش�)
            Color color = image.color;
            color.a = 1f; // ���İ��� 255�� �ش��ϴ� 1�� ����
            image.color = color;
            image.sprite = s;
        }
    }
}
