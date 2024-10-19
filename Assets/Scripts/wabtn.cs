using UnityEngine;

public class wabtn : MonoBehaviour
{
    public GameObject wasc;
    public GameObject ssc;
    public GameObject mgr;

    public void OnButtonClick()
    {
        wasc.SetActive(true);
        ssc.SetActive(false);
        mgr.GetComponent<scrollbtn>().maincker = 0;
    }
}
