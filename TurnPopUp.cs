using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnPopUp : MonoBehaviour
{
    private Image popUp;
    private TextMeshProUGUI popUpText;

    IEnumerator TextSet(string text, float time)
    {
        popUpText.text = text;
        float a = 0;
        while(a < 1)
        {
            a += Time.deltaTime;
            Color popUpcolor = new Color(popUp.color.r, popUp.color.g, popUp.color.b, a);
            popUp.color = popUpcolor;
            Color popUpTextColor = new Color(popUpText.color.r, popUpText.color.g, popUpText.color.b, a);
            popUpText.color = popUpTextColor;
            yield return null;
        }

        float elapsed = 0;
        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        while(a > 0)
        {
            a -= Time.deltaTime;
            Color popUpcolor = new Color(popUp.color.r, popUp.color.g, popUp.color.b, a);
            popUp.color = popUpcolor;
            Color popUpTextColor = new Color(popUpText.color.r, popUpText.color.g, popUpText.color.b, a);
            popUpText.color = popUpTextColor;
            yield return null;
        }

        if (GameManager.instance != null)
            GameManager.instance.turnPopUp = null;

        Destroy(gameObject, 0.1f);
    }

    public void SetPopUp(string text, float time)
    {
        popUp = GetComponentInChildren<Image>();
        popUpText = GetComponentInChildren<TextMeshProUGUI>();
        if(GameManager.instance != null)
            GameManager.instance.turnPopUp = StartCoroutine(TextSet(text, time));
        else
            StartCoroutine(TextSet(text, time));
    }
}
