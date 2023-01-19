using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnClickBtn : MonoBehaviour
{
    Button use;
    Button abandon;

    private void Use()
    {
        if(ItemBag.instance.curItem != null)
            ItemBag.instance.curItem.Use();
    }

    private void Abandon()
    {
        if (ItemBag.instance.curItem != null)
            ItemBag.instance.curItem.Abandon();
    }

    public void Init()
    {
        use = transform.Find("Use").GetComponent<Button>();
        use.onClick.AddListener(Use);

        abandon = transform.Find("Abandon").GetComponent<Button>();
        abandon.onClick.AddListener(Abandon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
