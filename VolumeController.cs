using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    VolumeSlider totalVolume;
    VolumeSlider bgVolume;
    VolumeSlider fxVolume;


    public void Init()
    {
        totalVolume = UtillHelper.GetComponent<VolumeSlider>("Total",transform);
        totalVolume.Init();
        bgVolume = UtillHelper.GetComponent<VolumeSlider>("Bg", transform);
        bgVolume.Init();
        fxVolume = UtillHelper.GetComponent<VolumeSlider>("Fx", transform);
        fxVolume.Init();

        totalVolume.slider.value = SaveManager.Instance.settingData.totalVolume;
        bgVolume.slider.value = SaveManager.Instance.settingData.bgVolume;
        fxVolume.slider.value = SaveManager.Instance.settingData.fxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if(totalVolume.slider.value != SaveManager.Instance.settingData.totalVolume)
        {
            SaveManager.Instance.settingData.totalVolume = totalVolume.slider.value;
            SaveManager.Instance.SaveSettingData();
        }

        if (bgVolume.slider.value != SaveManager.Instance.settingData.bgVolume)
        {
            SaveManager.Instance.settingData.bgVolume = bgVolume.slider.value;
            SaveManager.Instance.SaveSettingData();
        }

        if (fxVolume.slider.value != SaveManager.Instance.settingData.fxVolume)
        {
            SaveManager.Instance.settingData.fxVolume = fxVolume.slider.value;
            SaveManager.Instance.SaveSettingData();
        }
    }
}
