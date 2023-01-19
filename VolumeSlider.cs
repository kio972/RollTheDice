using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    TextMeshProUGUI valueText;
    public Slider slider;

    // Start is called before the first frame update
    public void Init()
    {
        valueText = UtillHelper.GetComponent<TextMeshProUGUI>("Value", transform);
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        valueText.text = ((int)(slider.value * 100)).ToString() + "%";
    }
}
