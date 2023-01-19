using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public bool interactable = false;
    public Coroutine fading;
    private void Awake()
    {
        FadeOut[] fade = FindObjectsOfType<FadeOut>(true);
        if(fade.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public IEnumerator CoFadeOut(float time = 1f, float waitTime = 0.5f, bool open = false)
    {
        Image fade = GetComponentInChildren<Image>(true);
        fade.gameObject.SetActive(true);
        float a = fade.color.a;
        float elapsed = 0;
        while (true)
        {
            a += Time.deltaTime/time;
            elapsed += Time.deltaTime;
            Color color = new Color(0, 0, 0, a);
            fade.color = color;
            if (elapsed >= time)
            {
                elapsed = 0;
                break;
            }
            yield return null;
        }

        if(waitTime > 0)
        {
            while(elapsed < waitTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        if (open)
            FadeI();
    }

    public IEnumerator CoFadeIn(bool sceneChange = false)
    {
        Image fade = GetComponentInChildren<Image>(true);
        float a = fade.color.a;
        while(true)
        {
            a -= Time.deltaTime;
            Color color = new Color(0, 0, 0, a);
            fade.color = color;
            if (a <= 0)
                break;
            yield return null;
        }

        fade.gameObject.SetActive(false);
        fading = null;
    }

    public void FadeO(float time = 1f, float waitTime = 0, bool open = false)
    {
        interactable = false;
        if (fading != null)
            StopCoroutine(fading);
        fading = StartCoroutine(CoFadeOut(time, waitTime, open));
    }

    public void FadeI()
    {
        interactable = true;
        if (fading != null)
            StopCoroutine(fading);
        fading = StartCoroutine(CoFadeIn());
    }


}
