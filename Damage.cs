using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour
{
    TextMeshProUGUI text;

    public IEnumerator Effect(Transform pos, float damage)
    {
        text = GetComponent<TextMeshProUGUI>();
        if(damage >= 0)
        {
            text.text = "- " + damage.ToString();
            text.color = Color.red;
        }
        else
        {
            text.text = "+ " + (-damage).ToString();
            text.color = Color.green;
        }

        
        Vector2 random = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        float a = 1;
        float elpasedTime = 0;
        Vector2 plus = new Vector2(0, 1);
        Vector2 target = Camera.main.WorldToScreenPoint(pos.position);
        while (true)
        {
            
            transform.position = target + random + plus;
            plus = plus + new Vector2(0, 1);
            elpasedTime += Time.deltaTime;
            //transform.position = transform.position + new Vector3(0, 1);

            if(elpasedTime > 0.5f)
            {
                a -= Time.deltaTime * 5;
                Color color = new Color(text.color.r, text.color.g, text.color.b, a);
                text.color = color;
            }

            if (a <= 0)
                break;

            yield return null;
        }

        DamageText.instance.damageTexts.Add(this);
        gameObject.SetActive(false);
        yield return null;
    }
}
