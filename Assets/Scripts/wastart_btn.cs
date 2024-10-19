using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class wastart_btn : MonoBehaviour
{

    public GameObject net;
    public GameObject load;
    // ���� ��ġ�� �����մϴ�.
    public Transform spawnPoint;
    private bool cker = false;
    public Canvas parentCanvas;
    public RectTransform referenceObject;
    public int maincker = 0;

    public void Spawn()
    {
        if (cker == true)
            return;

        GameObject network = Instantiate(net, spawnPoint.position, spawnPoint.rotation);
        Network network1 = network.GetComponent<Network>();
        network1.type = 1;
        string savedNames = PlayerPrefs.GetString("SavedNames", "card1,card1,card1,card2,card2,card2,card3,card3,card4,card4,card4,card5,card5,card5,card6,card6,card6,card7,card7,card7,card8,card8,card8,card9,card9,card9,card10,card10,card10");
        // �ҷ��� ���ڿ��� ��ǥ�� �����Ͽ� ����Ʈ�� ��ȯ
        network1.deck = new List<string>(savedNames.Split(','));

        GameObject newObject = Instantiate(load);
        newObject.transform.SetParent(parentCanvas.transform, false);
        // ���� ������Ʈ�� ��ġ�� �����ϰ� �����մϴ�.
        RectTransform newRectTransform = newObject.GetComponent<RectTransform>();
        if (newRectTransform != null && referenceObject != null)
        {
            newRectTransform.anchoredPosition = referenceObject.anchoredPosition;
            newRectTransform.rotation = referenceObject.rotation;
        }
        cker = true;
    }
}
