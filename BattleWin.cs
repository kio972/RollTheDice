
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleWin : MonoBehaviour
{
    List<Transform> slots;

    RewardNumSet skillP;
    RewardNumSet permanentP;
    RewardNumSet goldP;

    [SerializeField]
    private SceneMoveBtn backToTown;

    private void CallBtn()
    {
        backToTown.gameObject.SetActive(true);
    }

    private void DayRaise()
    {
        GameData.week++;
        if (GameData.clearedQuest == null)
            GameData.clearedQuest = new List<int>();
        GameData.clearedQuest.Add((GameManager.instance.curQuest.questInfo.questId));
    }

    private void Awake()
    {
        print("Awake");
        backToTown = transform.GetComponentInChildren<SceneMoveBtn>();
        Button btn = backToTown.GetComponentInChildren<Button>();
        btn.onClick.AddListener(DayRaise);
        backToTown.sceneName = "Town";
        backToTown.gameObject.SetActive(false);
        Invoke("CallBtn", 0.5f);
    }



    public void SetReward(QuestReward reward)
    {
        int slotNum = 0;
        
        int gold = Random.Range(reward.minGold, reward.maxGold + 1);
        if (gold > 0)
            slotNum++;

        int skillPoint = Random.Range(reward.minSkillPoint, reward.maxSkillPoint + 1);
        if(skillPoint > 0)
            slotNum++;

        int permanentPoint = Random.Range(reward.minPermanetPoint, reward.maxPermanetPoint);
        if (permanentPoint > 0)
            slotNum++;

        int itemReward = -1;
        if (reward.itemPool != null)
        {
            float token = Random.Range(0, 100);
            if (token < reward.itemChance)
            {
                int index = Random.Range(0, reward.itemPool.Count);
                itemReward = reward.itemPool[index];
                slotNum++;
            }
        }

        int relic = -1;
        if (reward.relic != 0)
        {
            float token = Random.Range(0, 100);
            if (token < reward.relicChance)
            {
                relic = reward.relic;
                slotNum++;
            }
        }

        if(slotNum > 0)
        {
            SetTransform(slotNum);

            List<int> slotIndex = new List<int>();
            for (int i = 0; i < slotNum; i++)
                slotIndex.Add(i);

            if (gold > 0)
            {
                int index = Random.Range(0, slotIndex.Count);
                RewardNumSet temp = Instantiate(goldP, slots[slotIndex[index]]);
                temp.SetGoldNum(gold);
                slotIndex.Remove(slotIndex[index]);
                GameManager.instance.player.gold += gold;
            }

            if (skillPoint > 0)
            {
                int index = Random.Range(0, slotIndex.Count);
                RewardNumSet temp = Instantiate(skillP, slots[slotIndex[index]]);
                temp.SetNum(skillPoint);
                slotIndex.Remove(slotIndex[index]);
                GameManager.instance.player.skillPoint += skillPoint;
            }

            if (permanentPoint > 0)
            {
                int index = Random.Range(0, slotIndex.Count);
                RewardNumSet temp = Instantiate(permanentP, slots[slotIndex[index]]);
                temp.SetNum(permanentPoint);
                slotIndex.Remove(slotIndex[index]);
                GameManager.instance.player.permanentSkillPoint += permanentPoint;
            }

            if (itemReward != -1)
            {
                int index = Random.Range(0, slotIndex.Count);
                Items item = Resources.Load<Items>(DataManger.ItemData[itemReward]["Path"]);
                item = Instantiate(item, slots[slotIndex[index]]);
                slotIndex.Remove(slotIndex[index]);
                GameManager.instance.player.bag.itemGroup.Add(item);
            }

            if (relic != -1)
            {
                int index = Random.Range(0, slotIndex.Count);
                RelicFunc temp = Resources.Load<RelicFunc>("");
                temp = Instantiate(temp, slots[slotIndex[index]]);
                slotIndex.Remove(slotIndex[index]);
                GameManager.instance.player.playerStat.relic.GetRelic(relic);
            }
        }
        
    }

    private void SetTransform(int rewardNum)
    {
        //RectTransform rect = transform.GetComponent<RectTransform>();
        slots = new List<Transform>();

        Transform slotHead = transform.Find("Mask/" + rewardNum.ToString());
        for (int i = 0; i < rewardNum; i++)
        {
            Transform temp = slotHead.Find("Slot" + i.ToString());
            slots.Add(temp);
        }

        for(int i = 0; i < 5; i++)
        {
            Transform slots = transform.Find("Mask/" + (i + 1).ToString());
            slots.gameObject.SetActive(false);
        }

        slotHead.gameObject.SetActive(true);

        skillP = Resources.Load<RewardNumSet>("Prefab/Battle/RewardSkillPoint");
        permanentP = Resources.Load<RewardNumSet>("Prefab/Battle/RewardPermanent");
        goldP = Resources.Load<RewardNumSet>("Prefab/Battle/RewardGold");
    }
}
