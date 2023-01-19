using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpace : MonoBehaviour
{
    private Controller target;

    public Controller Target
    {
        set
        {
            target = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Transform pos = target.transform.Find("TokenPivot");
            if(pos != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(pos.position);
            }
            else
                transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        }
    }
}
