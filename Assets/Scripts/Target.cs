using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public string drop;
    public Vector3 objectPosition;
    public string tag;
    public bool opcker = false;

    private void Start()
    {
        if (gameObject.tag.Contains("opp_mon"))
        {
            opcker = true;
        }

    }
}


