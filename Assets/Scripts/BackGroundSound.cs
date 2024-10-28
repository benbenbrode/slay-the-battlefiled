using UnityEngine;

public class BackGroundSound : MonoBehaviour
{
    private static BackGroundSound instance;
    private AudioSource audioSource;

    void Awake()
    {
        // �̱��� ������ ����� ������Ʈ �ߺ� ������ ����
        if (instance != null)
        {
            Destroy(gameObject);  // �ߺ��� ������Ʈ�� �ı�
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // ���� �ٲ� ������Ʈ�� �ı����� �ʰ� ����

            // AudioSource ������Ʈ�� ������
            audioSource = GetComponent<AudioSource>();

            // ���� ���� �ݺ� ����
            audioSource.loop = true;

            // ���� ���
            audioSource.Play();
        }
    }
}
