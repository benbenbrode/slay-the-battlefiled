using UnityEngine;
using UnityEngine.UI;

public class vol_ctrl : MonoBehaviour
{
    // �����̴� ���� ����
    public Slider volumeSlider;

    void Start()
    {
        // �����̴� �� ���� �� ȣ��� �̺�Ʈ ����
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // �����̴� �ʱⰪ�� �ý��� ������ ���� ���� (0 ~ 1 ����)
        volumeSlider.value = AudioListener.volume;
    }

    // ���� ���� �޼���
    void SetVolume(float value)
    {
        // ��ü ���� ���� ����
        AudioListener.volume = value;
    }
}
