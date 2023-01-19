using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleLose : MonoBehaviour
{
    Image bg;
    Transform lose;
    Button loseBtn;

    IEnumerator Effect()
    {
        bg.color = Color.clear;
        bg.gameObject.SetActive(true);
        float elapsed = 0;
        while (elapsed < 0.8)
        {
            elapsed += Time.deltaTime;
            bg.color = new Color(0, 0, 0, elapsed);
            yield return null;
        }
        elapsed = 0;

        lose.gameObject.SetActive(true);
        loseBtn.gameObject.SetActive(false);
        TextMeshProUGUI text = UtillHelper.GetComponent<TextMeshProUGUI>("LoseText", lose);
        while (elapsed < 1)
        {
            text.color = new Color(1, 1, 1, elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.color = Color.white;
        loseBtn.gameObject.SetActive(true);
        yield return null;
    }

    private void GoTitle()
    {
        SceneManager.LoadSceneAsync("EndScene");
    }

    private void GoBack()
    {
        FadeOut fade = FindObjectOfType<FadeOut>(true);
        if (fade != null)
            fade.FadeO();
        Invoke("GoTitle", 1.0f);
    }

    private void Awake()
    {
        bg = UtillHelper.GetComponent<Image>("BgImg", transform);
        lose = transform.Find("Lose");
        loseBtn = GetComponentInChildren<Button>();
        loseBtn.onClick.AddListener(GoBack);
        lose.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(Effect());
    }
}
