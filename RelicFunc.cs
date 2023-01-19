using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct RelicInfo
{
    public int index;
    public string name;
    public float price;
    public ItemValue value;
    public string description;
}

public class RelicFunc : MonoBehaviour
{
    public RelicInfo relicInfo;
    public bool haveItem = false;
    EventTrigger eventTrigger;

    private bool mouseOver = false;

    protected virtual void RelicEffect()
    {

    }

    private void BuyItem(PointerEventData data)
    {
        if(!haveItem)
        {
            // 상점에서 클릭했을때, 돈있으면 바로 사지게
            if (TownController.instance.player.gold >= relicInfo.price)
            {
                TownController.instance.player.gold -= (int)relicInfo.price;
                TownController.instance.player.statBar.UpdateStat();

                TownController.instance.player.playerStat.relic.GetRelic(relicInfo.index);

                AudioClip clip = Resources.Load<AudioClip>("Sounds/buy");
                AudioManager.Instance.PlayEffect(clip);

                string ment = "좋은거래였네.";
                TownController.instance.npc.SetConv(ment);

                OnPointerExit(data);
                Destroy(gameObject);
            }
            else
            {
                // 돈이 부족합니다
                string ment = "돈이 부족합니다.";
                TownController.instance.npc.SetConv(ment);
            }
        }
    }

    private void OnPointerExit(PointerEventData data)
    {
        if (PlayerStat.instance != null)
        {
            PlayerStat.instance.popUp.gameObject.SetActive(false);
            mouseOver = false;
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;
    }

    private void OnPointerEnter(PointerEventData data)
    {
        if(PlayerStat.instance != null)
        {
            PlayerStat.instance.popUp.gameObject.SetActive(true);
            PlayerStat.instance.popUp.UpdateInfo(this);
            mouseOver = true;
        }

        if (MousePointer.instance != null)
        {
            if (haveItem)
                MousePointer.instance.mouseType = MouseType.Interact;
            else
                MousePointer.instance.mouseType = MouseType.Buy;
        }
    }


    private void SetMouseOver()
    {
        if(eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();

            UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerEnter, OnPointerEnter);
            UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerExit, OnPointerExit);

            if(!haveItem)
                UtillHelper.SetEvent(eventTrigger, EventTriggerType.PointerDown, BuyItem);
        }
    }

    private void LoadRelic(int index, bool interactable)
    {
        if(DataManger.RelicData != null)
        {
            relicInfo.name = DataManger.RelicData[index]["Name"];
            relicInfo.description = DataManger.RelicData[index]["Description"];
            relicInfo.price = float.Parse(DataManger.RelicData[index]["Price"]);
            int value = int.Parse(DataManger.RelicData[index]["Value"]);
            switch (value)
            {
                case 0: relicInfo.value = ItemValue.Common; break;
                case 1: relicInfo.value = ItemValue.Uncommon; break;
                case 2: relicInfo.value = ItemValue.Rare; break;
                case 3: relicInfo.value = ItemValue.Epic; break;
            }

            if(interactable)
                SetMouseOver();
        }
    }

    public virtual void Init(bool interactable = true)
    {
        LoadRelic(relicInfo.index, interactable);
    }

    private void Update()
    {
        if (mouseOver && PlayerStat.instance != null)
        {
            Vector2 pos = Input.mousePosition;
            pos.x += 230;
            pos.y += 80;
            PlayerStat.instance.popUp.transform.position = pos;
        }
    }
}
