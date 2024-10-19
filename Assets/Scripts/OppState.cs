using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class OppState : MonoBehaviour
{
    public int hp = 30;
    public int shield = 0;
    public Text hp_text;
    public Text shild_text;
    public Sprite opp_img;
    public GameObject opp_obj;

    private void Start()
    {

    }
    private void Update()
    {
        if (shield < 0)
        {
            hp += shield;
            shield = 0;
        }
        if (hp < 1)
        {
            Debug.Log("³¡");
        }
        hp_text.text = hp.ToString();
        shild_text.text = shield.ToString();
    }
}
