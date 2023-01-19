using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPopUp : MonoBehaviour
{
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemDescription;
    private TextMeshProUGUI itemPrice;

    public void UpdateInfo(Items item)
    {
        itemName.text = item.itemInfo.name;
        itemDescription.text = item.itemInfo.description;
        if(item.haveItem)
        {
            itemPrice.gameObject.SetActive(false);
        }
        else
        {
            itemPrice.gameObject.SetActive(true);
            itemPrice.text = item.itemInfo.itemPrice.ToString() + "<sprite=0>";
        }
    }

    public void UpdateInfo(RelicFunc relic)
    {
        itemName.text = relic.relicInfo.name;
        itemDescription.text = relic.relicInfo.description;
        if (relic.haveItem)
        {
            itemPrice.gameObject.SetActive(false);
        }
        else
        {
            itemPrice.gameObject.SetActive(true);
            itemPrice.text = relic.relicInfo.price.ToString() + "<sprite=0>";
        }
    }

    public void UpdateInfo(Buff buff)
    {
        itemName.text = buff.buffInfo.name;
        itemDescription.text = buff.buffInfo.description;
        itemPrice.gameObject.SetActive(false);
    }

    public void Init()
    {
        itemName = transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        itemDescription = transform.Find("ItemDesc").GetComponent<TextMeshProUGUI>();
        itemPrice = UtillHelper.GetComponent<TextMeshProUGUI>("ItemPrice", transform);
    }
}
