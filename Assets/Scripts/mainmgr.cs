using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class mainmgr : MonoBehaviour
{
    public Text cost;
    public GameObject me;
    public GameObject myhero;
    public GameObject opphero;
    public Sprite w;
    public Sprite s;
    public GameObject net;
    public Image spriteRenderer;
    public Image spriteRenderer2;
    // Start is called before the first frame update
    void Start()
    {
        net = GameObject.Find("netmgr(Clone)");
        spriteRenderer = myhero.GetComponent<Image>();
        spriteRenderer2 = myhero.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        cost.text = me.GetComponent<PlayerState>().cost + "/" + me.GetComponent<PlayerState>().maxcost;
        if(net.GetComponent<Network>().type == 1)
        {
            spriteRenderer.sprite = w;
        }
        else if(net.GetComponent<Network>().type == 2)
        {
            spriteRenderer.sprite = s;
        }

        if (net.GetComponent<Network>().type2 == 1)
        {
            spriteRenderer2.sprite = w;
        }
        else if (net.GetComponent<Network>().type2 == 2)
        {
            spriteRenderer2.sprite = s;
        }


    }
}
