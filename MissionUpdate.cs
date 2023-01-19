using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionUpdate : MonoBehaviour
{
    public bool isClear;
    Transform checkBox;
    TextMeshProUGUI text;

    public void SetMissionFail()
    {
        checkBox.gameObject.SetActive(false);
    }

    public void SetMission(string str)
    {
        text.text = str;

        if (isClear)
            checkBox.gameObject.SetActive(true);
    }

    public void Init()
    {
        text = GetComponent<TextMeshProUGUI>();
        checkBox = transform.Find("CheckBox/Check");
        checkBox.gameObject.SetActive(false);
    }
}
