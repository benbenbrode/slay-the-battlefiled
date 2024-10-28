using UnityEngine;

public class BackGroundSound : MonoBehaviour
{
    private static BackGroundSound instance;
    private AudioSource audioSource;

    void Awake()
    {
        // 싱글톤 패턴을 사용해 오브젝트 중복 생성을 방지
        if (instance != null)
        {
            Destroy(gameObject);  // 중복된 오브젝트는 파괴
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 바뀌어도 오브젝트가 파괴되지 않게 설정

            // AudioSource 컴포넌트를 가져옴
            audioSource = GetComponent<AudioSource>();

            // 음악 무한 반복 설정
            audioSource.loop = true;

            // 음악 재생
            audioSource.Play();
        }
    }
}
