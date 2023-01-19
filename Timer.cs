using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void SetText()
    {
        text.text = "Day " + GameData.week.ToString();
    }

    public void Init()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        if(text != null)
            SetText();
    }

}
