using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemValue
{
    Common,
    Uncommon,
    Rare,
    Epic,
}

public struct ItemInfo
{
    public int id;
    public string name;
    public ItemValue value;
    public string description;
    public float itemPrice;
}

public class Items : MonoBehaviour
{
    public Image icon;
    public Button itemBtn;
    public bool haveItem = false;

    public bool interact = false;

    protected bool isInit;

    public ItemInfo itemInfo;

    EventTrigger eventTrigger;

    private void OnPointerExit(PointerEventData data)
    {
        if (!ItemBag.instance.interact)
        {
            ItemBag.instance.PopUpDescExit();
            ItemBag.instance.ispointerOn = false;
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;
    }

    private void OnPointerEnter(PointerEventData data)
    {
        FadeOut fade = FindObjectOfType<FadeOut>();
        if(fade != null && !fade.interactable)
            return;

        if (!ItemBag.instance.interact)
        {
            ItemBag.instance.PopUpDesc(this);
            ItemBag.instance.ispointerOn = true;
        }

        if(MousePointer.instance != null)
        {
            if (haveItem)
                MousePointer.instance.mouseType = MouseType.Interact;
            else
                MousePointer.instance.mouseType = MouseType.Buy;
        }
    }

    public virtual void Use()
    {
        

    }

    public void Abandon()
    {
        ItemBag.instance.itemGroup.Remove(this);
        ItemBag.instance.curItem = null;
        ItemBag.instance.interact = false;
        ItemBag.instance.PopUpDescExit();
        ItemBag.instance.UpdateBag();
        Destroy(gameObject);
    }


    private void OnClick()
    {
        if(haveItem)
        {
            if (!interact)
            {
                // 사용 / 버리기 선택창 뜨게
                interact = true;
                ItemBag.instance.curItem = this;
                ItemBag.instance.PopUpDesc(this);
                ItemBag.instance.UseItemChoice();
            }
        }
        else
        {
            BuyItem();
        }
    }

    public void LoadItem(int index)
    {
        if(DataManger.ItemData != null)
        {
            itemInfo.name = DataManger.ItemData[index]["Name"];
            itemInfo.itemPrice = float.Parse(DataManger.ItemData[index]["Price"]);

            int value = int.Parse(DataManger.ItemData[index]["Value"]);
            switch (value)
            {
                case 0: itemInfo.value = ItemValue.Common; break;
                case 1: itemInfo.value = ItemValue.Uncommon; break;
                case 2: itemInfo.value = ItemValue.Rare; break;
                case 3: itemInfo.value = ItemValue.Epic; break;
            }
            
            itemInfo.description = DataManger.ItemData[index]["Description"];
        }
    }

    private void BuyItem()
    {
        // 상점에서 클릭했을때, 돈있으면 바로 사지게
        if (TownController.instance.player.gold >= itemInfo.itemPrice)
        {
            int itemCount = ItemBag.instance.itemGroup.Count;
            int bagSize = ItemBag.instance.itemGroupTrans.Count;
            if (itemCount < bagSize)
            {
                TownController.instance.player.gold -= (int)itemInfo.itemPrice;
                TownController.instance.player.statBar.UpdateStat();
                transform.SetParent(ItemBag.instance.itemGroupTrans[itemCount]);
                transform.position = ItemBag.instance.itemGroupTrans[itemCount].position;
                ItemBag.instance.itemGroup.Add(this);
                haveItem = true;

                AudioClip clip = Resources.Load<AudioClip>("Sounds/buy");
                AudioManager.Instance.PlayEffect(clip);

                string ment = "좋은거래였네.";
                TownController.instance.npc.SetConv(ment);
            }
            else
            {
                string ment = "가방이 꽉 찼군.";
                TownController.instance.npc.SetConv(ment);
            }
        }
        else
        {
            // 돈이 부족합니다
            string ment = "돈이 부족합니다.";
            TownController.instance.npc.SetConv(ment);
        }
    }

    public virtual void Init()
    {
        if(!isInit)
        {
            if (eventTrigger == null)
                eventTrigger = gameObject.AddComponent<EventTrigger>();

            UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerEnter, OnPointerEnter);
            UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerExit, OnPointerExit);

            itemBtn = GetComponent<Button>();
            itemBtn.onClick.AddListener(OnClick);

            LoadItem(itemInfo.id);
        }
        
        isInit = true;
    }

    private void Update()
    {
        if(interact)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
            {
                List<RaycastResult> results = new List<RaycastResult>();
                GraphicRaycaster raycaster = FindObjectOfType<GraphicRaycaster>();
                PointerEventData data = new PointerEventData(null);
                data.position = Input.mousePosition;
                raycaster.Raycast(data, results);
                if (results.Count == 0)
                {
                    //interact = false;
                    //ItemBag.instance.PopUpDescExit();
                }
                else
                {
                    Items item = results[0].gameObject.GetComponent<Items>();
                    if(this != item)
                    {
                        interact = false;
                    }
                }
            }
        }
    }
}
