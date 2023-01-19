using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCController : MonoBehaviour
{
    private Image npcIllur;
    private TextMeshProUGUI npcName;
    private TextMeshProUGUI npcType;

    private Transform npcConvZone;
    private Image npcConvBg;
    private TextMeshProUGUI npcConv;
    
    private Coroutine textPrinting;
    
    public void SetNPCName(string type, string name)
    {
        npcName.text = name;
        npcType.text = type;
    }

    IEnumerator PrintText(string text, float time = 2)
    {
        npcConvZone.gameObject.SetActive(true);
        npcConvBg.color = Color.white;
        npcConv.color = Color.black;
        npcConv.text = text;
        float elapsed = 0;
        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        float alpha = 1f;
        while(true)
        {
            alpha -= 0.05f;
            Color bgColor = new Color(npcConvBg.color.r, npcConvBg.color.g, npcConvBg.color.b, alpha);
            Color textColor = new Color(npcConv.color.r, npcConv.color.g, npcConv.color.b, alpha);
            npcConvBg.color = bgColor;
            npcConv.color = textColor;
            if (alpha <= 0)
                break;
            yield return new WaitForSeconds(0.01f);
        }

        textPrinting = null;
    }

    public void SetConv(string text)
    {
        if(textPrinting != null)
            StopCoroutine(textPrinting);
        TownController.instance.npc.gameObject.SetActive(true);
        textPrinting = StartCoroutine(PrintText(text));
    }

    public void Init()
    {
        npcIllur = transform.Find("NPCIllur").GetComponent<Image>();
        npcConvZone = transform.Find("NPCConv");
        npcConvBg = npcConvZone.transform.Find("Bg").GetComponent<Image>();
        npcConv = npcConvZone.GetComponentInChildren<TextMeshProUGUI>();

        npcName = transform.Find("NPCName").GetComponent<TextMeshProUGUI>();
        npcType = transform.Find("NPCType").GetComponent<TextMeshProUGUI>();
    }

}
