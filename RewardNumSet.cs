using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardNumSet : MonoBehaviour
{
    TextMeshProUGUI number;

    public void SetGoldNum(int num)
    {
        if (num > 1)
        {
            number.gameObject.SetActive(true);
            number.text = num.ToString();
        }
    }

    public void SetNum(int num)
    {
        if(num > 1)
        {
            number.gameObject.SetActive(true);
            number.text = "X "+ num.ToString();
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        number = GetComponentInChildren<TextMeshProUGUI>();
        number.gameObject.SetActive(false);
    }

}
