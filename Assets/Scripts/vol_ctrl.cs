using UnityEngine;
using UnityEngine.UI;

public class vol_ctrl : MonoBehaviour
{
    // 슬라이더 참조 변수
    public Slider volumeSlider;

    void Start()
    {
        // 슬라이더 값 변경 시 호출될 이벤트 설정
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // 슬라이더 초기값을 시스템 볼륨에 맞춰 설정 (0 ~ 1 범위)
        volumeSlider.value = AudioListener.volume;
    }

    // 볼륨 설정 메서드
    void SetVolume(float value)
    {
        // 전체 게임 볼륨 설정
        AudioListener.volume = value;
    }
}
