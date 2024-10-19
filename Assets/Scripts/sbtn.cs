using UnityEngine;

public class sbtn : MonoBehaviour
{
    public GameObject wasc;
    public GameObject ssc;
    public GameObject mgr;

    public void OnButtonClick()
    {
        wasc.SetActive(false);
        ssc.SetActive(true);
        mgr.GetComponent<scrollbtn>().maincker = 1;
    }
}