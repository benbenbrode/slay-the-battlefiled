using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class monstate : MonoBehaviourPun
{
    public int hp;
    public int shield = 0;
    public GameObject battlemgr;
    private bool cker = false;

    // 'eff' 관련 변수 선언
    private GameObject eff;
    private Text hpText;
    private Text ammorText; // 추가된 변수

    public bool animacker = false;

    void Start()
    {
        battlemgr = GameObject.Find("battlemgr");

        // 'eff' 오브젝트 찾기
        eff = transform.Find("eff")?.gameObject;

        if (eff != null)
        {
            // 'hp_txt'의 Text 컴포넌트 가져오기
            Transform hpTxtTransform = eff.transform.Find("hp/hp_txt");
            if (hpTxtTransform != null)
            {
                hpText = hpTxtTransform.GetComponent<Text>();
            }

            // 'ammor_txt'의 Text 컴포넌트 가져오기
            Transform ammorTxtTransform = eff.transform.Find("ammor/ammor_txt");
            if (ammorTxtTransform != null)
            {
                ammorText = ammorTxtTransform.GetComponent<Text>();
            }
        }

        if (gameObject.name.Contains("vfx_3") || gameObject.name.Contains("vfx_16") || gameObject.name.Contains("vfx_28") || gameObject.name.Contains("vfx_29"))
        {
            hp = 5;
        }
        if (gameObject.name.Contains("vfx_17"))
        {
            hp = 15;
        }
    }

    void Update()
    {
        if (hp < 1)
        {
            monstate manager = FindObjectOfType<monstate>();

            if (gameObject.tag == "ally_mon1")
            {
                battlemgr.GetComponent<battlemgr>().poscker[1] = true;
                manager.RequestRemoveObjectWithTag("opp_mon1");
                cker = true;
            }
            if (gameObject.tag == "ally_mon2")
            {
                battlemgr.GetComponent<battlemgr>().poscker[2] = true;
                manager.RequestRemoveObjectWithTag("opp_mon2");
                cker = true;
            }
            if (gameObject.tag == "ally_mon3")
            {
                battlemgr.GetComponent<battlemgr>().poscker[3] = true;
                manager.RequestRemoveObjectWithTag("opp_mon3");
                cker = true;
            }
            battlemgr.GetComponent<battlemgr>().monkillcount += 1;
            Destroy(gameObject);
            
        }

        // 'eff'가 활성화되어 있으면 'hp_txt'와 'ammor_txt'의 텍스트를 업데이트
        if (eff != null && eff.activeSelf)
        {
            if (hpText != null)
            {
                hpText.text = hp.ToString();
            }
            if (ammorText != null)
            {
                ammorText.text = shield.ToString();
            }
        }
    }

    [PunRPC]
    public void RemoveObjectWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
    }

    public void RequestRemoveObjectWithTag(string tag)
    {
        if (cker == true)
            return;

        // photonView가 제대로 초기화되었는지 확인
        if (photonView != null && photonView.ViewID != 0)
        {
            photonView.RPC("RemoveObjectWithTag", RpcTarget.Others, tag);
        }
        else
        {
            Debug.LogError("PhotonView가 초기화되지 않았거나 ViewID가 유효하지 않습니다.");
        }
    }
}
