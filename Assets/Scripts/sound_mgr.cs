using UnityEngine;

public class sound_mgr : MonoBehaviour
{
    // ����� �ҽ��� ����
    public AudioSource audioSource;

    // �پ��� ��Ȳ���� ����� ����� Ŭ����
    public AudioClip soundClip1;  // ��ư
    public AudioClip soundClip2;  // ���� ��ȯ
    public AudioClip soundClip3;  // Į�� ���� �Ҹ�
    public AudioClip soundClip4;  // ���мҸ�
    public AudioClip soundClip5;  // ��ũ���Ҹ�
    public AudioClip soundClip6;  // �б�Ÿ��
    public AudioClip soundClip7;  // õ�������� ���� �б�Ҹ�
    public AudioClip soundClip8;  // ���˺����� �Ҹ�
    public AudioClip soundClip9;  // ��Ÿ�� �Ҹ�
    public AudioClip soundClip10;  // ���� �Ҹ�
    public AudioClip soundClip11;  // �ǰ���
    public AudioClip soundClip12;  // ���г� ���� �ɰ����� �Ҹ�
    public AudioClip soundClip13;  // �ٶ��Ҹ�
    public AudioClip soundClip14; // ����Ҹ�
    public AudioClip soundClip15;  // ��
    public AudioClip soundClip16;  // ���� �������ѼҸ�
    public AudioClip soundClip17;  // ��ź������ �Ҹ�
    public AudioClip soundClip18; // ��������?
    public AudioClip soundClip19; // �͸� ���� ���

    // ��Ȳ�� ���� �Ҹ��� ����ϴ� �޼���
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
                Debug.Log("�ش� ���ǿ� �´� �Ҹ��� �����ϴ�.");
                break;
        }
    }
}
