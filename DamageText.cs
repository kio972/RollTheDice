using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public static DamageText instance;
    public List<Damage> damageTexts;

    public void Heal(Transform pos, float heal)
    {
        Vector2 target = Camera.main.WorldToScreenPoint(pos.position);
        if (damageTexts.Count > 1)
        {
            damageTexts[0].gameObject.SetActive(true);
            StartCoroutine(damageTexts[0].Effect(pos, -heal));
            damageTexts.Remove(damageTexts[0]);
        }
        else
        {
            Damage temp = Resources.Load<Damage>("Prefab/Effect/Damage");
            temp = Instantiate(temp, transform);
            StartCoroutine(temp.Effect(pos, -heal));
        }
    }

    public void Damage(Transform pos, float damage)
    {
        if (damageTexts.Count > 1)
        {
            damageTexts[0].gameObject.SetActive(true);
            StartCoroutine(damageTexts[0].Effect(pos, damage));
            damageTexts.Remove(damageTexts[0]);
        }
        else
        {
            Damage temp = Resources.Load<Damage>("Prefab/Effect/Damage");
            temp = Instantiate(temp, transform);
            StartCoroutine(temp.Effect(pos, damage));
        }
    }

    private void Start()
    {
        instance = this;
        damageTexts = new List<Damage>();
    }
}
