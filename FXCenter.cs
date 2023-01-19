using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXCenter : MonoBehaviour
{
    RectTransform rect;
    bool end = false;
    float elpased = 0;
    float time = 0.5f;
    float size = 1000;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rect != null && !end)
        {
            rect.sizeDelta = new Vector2(elpased, rect.sizeDelta.y);
            elpased += Time.deltaTime / time * size;
            if (elpased > size)
            {
                elpased = size;
                rect.sizeDelta = new Vector2(elpased, rect.sizeDelta.y);
                end = true;
            }
        }
    }
}
