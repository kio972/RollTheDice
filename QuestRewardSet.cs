using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestRewardSet : MonoBehaviour
{
    Transform gold;
    Transform skill;
    Transform permanent;
    Transform item;
    Transform relic;

    public void SetReward(QuestReward reward)
    {
        if(reward.minGold > 0)
        {
            gold.gameObject.SetActive(true);
            TextMeshProUGUI goldText = gold.GetComponentInChildren<TextMeshProUGUI>();
            if (reward.minGold == reward.maxGold)
                goldText.text = reward.minGold.ToString();
            else
                goldText.text = "?";
        }
        else
            gold.gameObject.SetActive(false);

        if(reward.minSkillPoint > 0)
        {
            skill.gameObject.SetActive(true);
            TextMeshProUGUI skillText = skill.GetComponentInChildren<TextMeshProUGUI>();
            if (reward.minSkillPoint == reward.maxSkillPoint)
                skillText.text = reward.minSkillPoint.ToString();
            else
                skillText.text = "?";
        }
        else
            skill.gameObject.SetActive(false);

        if(reward.minPermanetPoint > 0)
        {
            permanent.gameObject.SetActive(true);
            TextMeshProUGUI permanentText = permanent.GetComponentInChildren<TextMeshProUGUI>();
            if (reward.minPermanetPoint == reward.maxPermanetPoint)
                permanentText.text = reward.minPermanetPoint.ToString();
            else
                permanentText.text = "?";
        }
        else
            permanent.gameObject.SetActive(false);

        if(reward.itemPool != null && reward.itemPool.Count > 0)
        {
            item.gameObject.SetActive(true);
            TextMeshProUGUI itemText = item.GetComponentInChildren<TextMeshProUGUI>();
            if (reward.itemChance == 100)
                itemText.text = "";
            else
                itemText.text = "?";
        }
        else
            item.gameObject.SetActive(false);

        if(reward.relic != 0)
        {
            relic.gameObject.SetActive(true);
            TextMeshProUGUI relicText = relic.GetComponentInChildren<TextMeshProUGUI>();
            if (reward.relicChance == 100)
                relicText.text = "";
            else
                relicText.text = "?";
        }
        else
            relic.gameObject.SetActive(false);
    }

    void Awake()
    {
        gold = transform.Find("Gold");
        

        skill = transform.Find("SkillPoint");
        skill.gameObject.SetActive(false);

        permanent = transform.Find("PermanentPoint");
        permanent.gameObject.SetActive(false);

        item = transform.Find("Items");
        item.gameObject.SetActive(false);

        relic = transform.Find("Relics");
        relic.gameObject.SetActive(false);
    }
}
