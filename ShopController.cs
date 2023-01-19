using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private List<Items> itemList;
    private Transform itemSlot;

    [SerializeField]
    private int shopSize = 5;
    private Transform[] itemSlotTrans;

    private int maxRelic = 3;
    private Transform relicSlot;
    private Transform[] relicSlotTrans;


    public Transform shopUI;

    public void InstanceItem()
    {
        // 5개 아이템 생성할거임
        // 1, 2- 100퍼센트확률로 common아이템
        int random = Random.Range(0, DataManger.Item_Common.Count);
        int index = DataManger.Item_Common[random];
        Items item = Resources.Load<Items>(DataManger.ItemData[index]["Path"]);
        if (item != null)
        {
            item = Instantiate(item, itemSlot);
            item.Init();
            item.transform.position = Camera.main.WorldToScreenPoint(itemSlotTrans[0].position);
        }

        float token = Random.Range(0, 100);
        if (token <= 40)
        {
            random = Random.Range(0, DataManger.Item_Common.Count);
            index = DataManger.Item_Common[random];
        }
        else
        {
            random = Random.Range(0, DataManger.Item_Uncommon.Count);
            index = DataManger.Item_Uncommon[random];
        }

        item = Resources.Load<Items>(DataManger.ItemData[index]["Path"]);
        if (item != null)
        {
            item = Instantiate(item, itemSlot);
            item.Init();
            item.transform.position = Camera.main.WorldToScreenPoint(itemSlotTrans[1].position);
        }

        for(int i = 2; i < 5; i++)
        {
            token = Random.Range(0, 100);
            if (token < 15)
            {
                random = Random.Range(0, DataManger.Item_Epic.Count);
                index = DataManger.Item_Epic[random];
            }
            else if (token < 45)
            {
                random = Random.Range(0, DataManger.Item_Rare.Count);
                index = DataManger.Item_Rare[random];
            }
            else if (token <= 80)
            {
                random = Random.Range(0, DataManger.Item_Uncommon.Count);
                index = DataManger.Item_Uncommon[random];
            }
            else
            {
                random = Random.Range(0, DataManger.Item_Common.Count);
                index = DataManger.Item_Common[random];
            }

            item = Resources.Load<Items>(DataManger.ItemData[index]["Path"]);
            if (item != null)
            {
                item = Instantiate(item, itemSlot);
                item.Init();
                item.transform.position = Camera.main.WorldToScreenPoint(itemSlotTrans[i].position);
            }
        }
    }

    public void InstanceRelic()
    {
        List<int> relicIndex = new List<int>();
        // 생성조건 업데이트 필요
        for (int i = 0; i < maxRelic; i++)
        {
            // 안뜰확률 20퍼
            // 노말 30
            // 언커먼 25
            // 레어 15
            // 에픽 10 로 조정할예정

            int loop = 0;
            int index = -1;
            while (true)
            {
                loop++;
                if (loop >= 1000)
                {
                    print("no match!");
                    index = -1;
                    break;
                }

                index = Random.Range(0, DataManger.RelicData.Count);
                //index = int.Parse(DataManger.RelicData[index]["Index"]);

                if (relicIndex.Contains(index))
                    continue;

                if(GameData.playerRelics != null)
                {
                    if (GameData.playerRelics.Contains(index))
                        continue;
                }

                if (int.Parse(DataManger.RelicData[index]["Price"]) == -1)
                    continue;

                int value = int.Parse(DataManger.RelicData[index]["Value"]);
                float chance = 0;
                switch (value)
                {
                    // 등급별 확률
                    case 0: chance = 90; break;
                    case 1: chance = 70; break;
                    case 2: chance = 55; break;
                    case 3: chance = 40; break;
                }

                float token = Random.Range(0, 100);
                if (chance < token)
                    index = -1;

                break;
            }

            if(index != -1)
            {
                RelicFunc relic = Resources.Load<RelicFunc>("Prefab/Relics/Relic" + index.ToString());
                if (relic != null)
                {
                    relic = Instantiate(relic, relicSlot);
                    relic.Init();
                    relic.transform.position = Camera.main.WorldToScreenPoint(relicSlotTrans[i].position);
                }
                relicIndex.Add(index);
            }
        }
    }

    public void Init()
    {
        itemSlotTrans = new Transform[shopSize];
        for (int i = 0; i < shopSize; i++)
        {
            itemSlotTrans[i] = transform.Find("ItemZone/Slot" + i.ToString());
        }

        relicSlotTrans = new Transform[maxRelic];
        for (int i = 0; i < maxRelic; i++)
        {
            relicSlotTrans[i] = transform.Find("RelicZone/Slot" + i.ToString());
        }

        shopUI = TownController.instance.canvas.transform.Find("ShopUI");
        shopUI.gameObject.SetActive(false);

        itemSlot = shopUI.transform.Find("FixedItem");
        if (itemSlot != null && DataManger.ItemData != null)
            InstanceItem();

        relicSlot = shopUI.transform.Find("FixedRelic");
        if(relicSlot != null && DataManger.RelicData != null)
            InstanceRelic();
    }

}
