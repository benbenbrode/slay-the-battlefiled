using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card4vfx : MonoBehaviour
{
    public GameObject battle;

    private void Start()
    {
        battle = GameObject.Find("battlemgr");
        Destroy(gameObject, 1f);
    }
    // Start is called before the first frame update
    void OnDestroy()
    {
        battle.GetComponent<battlemgr>().applycker = false;
    }
}
