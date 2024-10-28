using UnityEngine;

public class sound_mgr : MonoBehaviour
{
    // 오디오 소스를 참조
    public AudioSource audioSource;

    // 다양한 상황에서 사용할 오디오 클립들
    public AudioClip soundClip1;  // 버튼
    public AudioClip soundClip2;  // 몬스터 소환
    public AudioClip soundClip3;  // 칼로 베는 소리
    public AudioClip soundClip4;  // 방패소리
    public AudioClip soundClip5;  // 랭크업소리
    public AudioClip soundClip6;  // 둔기타격
    public AudioClip soundClip7;  // 천지개벽용 강한 둔기소리
    public AudioClip soundClip8;  // 별똥별베기 소리
    public AudioClip soundClip9;  // 불타는 소리
    public AudioClip soundClip10;  // 갑옷 소리
    public AudioClip soundClip11;  // 피격음
    public AudioClip soundClip12;  // 방패나 갑옷 쪼개지는 소리
    public AudioClip soundClip13;  // 바람소리
    public AudioClip soundClip14; // 전기소리
    public AudioClip soundClip15;  // 독
    public AudioClip soundClip16;  // 유령 으스스한소리
    public AudioClip soundClip17;  // 폭탄터지는 소리
    public AudioClip soundClip18; // 숨돌리기?
    public AudioClip soundClip19; // 귀를 찢는 비명

    // 상황에 따라 소리를 재생하는 메서드
    public void PlaySoundBasedOnCondition(int condition)
    {
        switch (condition)
        {
            case 1:
                
                audioSource.PlayOneShot(soundClip1);
                break;
            case 2:
                
                audioSource.PlayOneShot(soundClip2);
                break;
            case 3:
                
                audioSource.PlayOneShot(soundClip3);
                break;
            case 4:

                audioSource.PlayOneShot(soundClip4);
                break;
            case 5:

                audioSource.PlayOneShot(soundClip5);
                break;
            case 6:

                audioSource.PlayOneShot(soundClip6);
                break;
            case 7:

                audioSource.PlayOneShot(soundClip7);
                break;
            case 8:

                audioSource.PlayOneShot(soundClip8);
                break;
            case 9:

                audioSource.PlayOneShot(soundClip9);
                break;
            case 10:

                audioSource.PlayOneShot(soundClip10);
                break;
            case 11:

                audioSource.PlayOneShot(soundClip11);
                break;
            case 12:

                audioSource.PlayOneShot(soundClip12);
                break;
            case 13:

                audioSource.PlayOneShot(soundClip13);
                break;
            case 14:

                audioSource.PlayOneShot(soundClip14);
                break;
            case 15:

                audioSource.PlayOneShot(soundClip15);
                break;
            case 16:

                audioSource.PlayOneShot(soundClip16);
                break;
            case 17:

                audioSource.PlayOneShot(soundClip17);
                break;
            case 18:

                audioSource.PlayOneShot(soundClip18);
                break;
            case 19:

                audioSource.PlayOneShot(soundClip19);
                break;

            default:
                Debug.Log("해당 조건에 맞는 소리가 없습니다.");
                break;
        }
    }
}
