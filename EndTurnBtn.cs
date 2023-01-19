using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnBtn : MonoBehaviour
{
    private Button btn;
    public TextMeshProUGUI text;

    private void EndTurnOnClick()
    {
        if(GameManager.instance.phase == 2 || GameManager.instance.phase == 3)
        {
            if(GameManager.instance.moveing == null)
                GameManager.instance.endInput = true;
        }
    }

    public void Init()
    {
        btn = GetComponent<Button>();
        if(btn != null)
            btn.onClick.AddListener(EndTurnOnClick);
        text = GetComponentInChildren<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }
}
