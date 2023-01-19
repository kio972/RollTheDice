using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuestReward
{
    public int minSkillPoint;
    public int maxSkillPoint;
    public int minPermanetPoint;
    public int maxPermanetPoint;
    public int minGold;
    public int maxGold;

    public List<int> itemPool;
    public float itemChance;
    
    public int relic;
    public float relicChance;
}

public enum QuestType
{
    Normal,
    Elite,
    Boss,
}

public struct QuestInfo
{
    public int questId;
    public QuestType questType;
    public string questName;
    public string questDesc;
    public QuestReward questReward;
}

public class Quests : MonoBehaviour
{
    public QuestInfo questInfo;
    protected Transform missionUpdater;
    public virtual void SetEndTurn()
    {

    }

    public virtual void SetBattle()
    {
        GameManager.instance.endInput = true;
    }

    public virtual void Init()
    {

    }

    public virtual void Update()
    {

    }
}
