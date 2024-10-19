using UnityEngine;
using UnityEngine.UI;

public class myimg : MonoBehaviour
{
    public Sprite w;
    public Sprite s;
    private Image image;            // UI Image 컴포넌트 참조
    public GameObject net;

    void Start()
    {
        net = GameObject.Find("netmgr(Clone)");
        // 현재 오브젝트의 Image 컴포넌트 가져오기
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("Image component not found!");
        }
    }

    void Update()
    {
        // 특정 변수가 1이 되었을 때
        if (net.GetComponent<Network>().type == 1)
        {
            // 알파값을 255로 설정 (1로 설정: Color의 알파는 0~1 범위이므로 255는 1에 해당)
            Color color = image.color;
            color.a = 1f; // 알파값을 255에 해당하는 1로 설정
            image.color = color;
            image.sprite = w;
        }
        else if (net.GetComponent<Network>().type == 2)
        {
            // 알파값을 255로 설정 (1로 설정: Color의 알파는 0~1 범위이므로 255는 1에 해당)
            Color color = image.color;
            color.a = 1f; // 알파값을 255에 해당하는 1로 설정
            image.color = color;
            image.sprite = s;
        }
    }
}
