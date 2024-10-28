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
            // 오브젝트 활성화
            settings.SetActive(false);
        }
    }
}