using UnityEngine;

public class setting : MonoBehaviour
{
 
    public GameObject settings;

    public void ActivateObject()
    {

            settings.SetActive(true);
        
    }

    public void del()
    {
        if (settings != null)
        {
            // ������Ʈ Ȱ��ȭ
            settings.SetActive(false);
        }
    }
}