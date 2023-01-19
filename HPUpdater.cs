using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPUpdater : MonoBehaviour
{
    Slider hpSlider;
    Slider transHpSlider;
    TextMeshProUGUI hpText;

    private float curHp;
    Coroutine update;

    public void LoadHealth(Controller target)
    {
        hpSlider.value = target.curHp / target.maxHp;
        transHpSlider.value = hpSlider.value;
        curHp = target.curHp;

        if (hpText != null)
            hpText.text = target.curHp.ToString() + " / " + target.maxHp.ToString();
    }

    public void UpdateHealthBar(float curHp, float nextHp, float maxHealth)
    {
        if(update != null)
        {
            StopCoroutine(update);
            curHp = this.curHp;
        }
        update = StartCoroutine(CoUpdateHealthBar(curHp, nextHp, maxHealth));
    }

    public IEnumerator CoUpdateHealthBar(float curHp, float nextHp, float maxHealth)
    {
        float targetVal = UpdateHealth(nextHp, maxHealth);
        float curVal = curHp / maxHealth;
        transHpSlider.value = curVal;
        float elapsed = 0;
        while(elapsed <= 0.5f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if(targetVal < curVal)
        {
            while (curVal > targetVal)
            {
                curVal -= Time.deltaTime * 2;
                transHpSlider.value = curVal;
                yield return null;
            }
        }
        else if (targetVal > curVal)
        {
            while (curVal < targetVal)
            {
                curVal += Time.deltaTime * 2;
                transHpSlider.value = curVal;
                yield return null;
            }
        }

        if (nextHp <= 0)
        {
            Destroy(gameObject, 0.1f);
        }

        this.curHp = nextHp;
        update = null;
    }

    public float UpdateHealth(float nextHp, float maxHealth)
    {   
        if(nextHp > 0)
        {
            float val = nextHp / maxHealth;
            hpSlider.value = val;
            string curHp = ((int)nextHp).ToString();
            if (nextHp < 1f)
                curHp = "1";
            if (hpText != null)
                hpText.text = curHp + " / " + ((int)maxHealth).ToString();
            return val;
        }
        else
        {
            hpSlider.value = 0;
            if (hpText != null)
                hpText.text = "0 / " + ((int)maxHealth).ToString();
            return 0;
        }

    }

    public void Init(Controller target)
    {
        hpSlider = GetComponent<Slider>();
        hpText = GetComponentInChildren<TextMeshProUGUI>();
        transHpSlider = transform.Find("Images").GetComponentInChildren<Slider>();
        LoadHealth(target);
    }
}
