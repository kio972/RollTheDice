using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreList : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string description;
    
    public void Init()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
}
