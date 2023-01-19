using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSpeedController : MonoBehaviour
{
    Slider gameSpeed;
    TextMeshProUGUI gameSpeedText;

    public static float SetSpeed(float value)
    {
        switch(value)
        {
            case 0:
                return 0.8f;
            case 1:
                return 1.0f;
            case 2:
                return 1.5f;
            case 3:
                return 2.0f;
        }

        return 1.0f;
    }

    private void SetText()
    {
        gameSpeedText.text = SetSpeed(gameSpeed.value).ToString() + "x";
    }

    public void Init()
    {
        gameSpeed = UtillHelper.GetComponent<Slider>("Total",transform);
        gameSpeed.value = SaveManager.Instance.settingData.gameSpeed;

        gameSpeedText = UtillHelper.GetComponent<TextMeshProUGUI>("Value", gameSpeed.transform);
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameSpeed.value != SaveManager.Instance.settingData.gameSpeed)
        {
            SaveManager.Instance.settingData.gameSpeed = gameSpeed.value;
            SaveManager.Instance.SaveSettingData();
            SetText();
        }
    }
}
