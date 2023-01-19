using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardDesc : MonoBehaviour
{
    TextMeshProUGUI questName;
    TextMeshProUGUI questDesc;
    QuestRewardSet questRewardSet;

    public void SetQuest(Quests quest)
    {
        questName.text = quest.questInfo.questName;
        questDesc.text = quest.questInfo.questDesc;
        questRewardSet.SetReward(quest.questInfo.questReward);
    }


    public void Init()
    {
        questName = UtillHelper.GetComponent<TextMeshProUGUI>("QuestZone/QuestName", transform);
        questDesc = UtillHelper.GetComponent<TextMeshProUGUI>("QuestZone/QuestDesc", transform);
        questRewardSet = UtillHelper.GetComponent<QuestRewardSet>("QuestZone/QuesReward", transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
