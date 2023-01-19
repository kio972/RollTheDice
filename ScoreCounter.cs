using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;
    private bool untouched = true;
    private int monsterKill = 0;

    private int monsterCount = 0;
    private float playerHp = 0;

    private int SpeedRunPoint()
    {
        int tapDancePoint = 5;
        if (GameManager.instance.turn > 3)
            tapDancePoint -= GameManager.instance.turn - 3;
        if (tapDancePoint < 0)
            tapDancePoint = 0;
        return tapDancePoint;
    }

    private void QuestClearCheck()
    {
        if (!GameManager.instance.isWin)
            return;

        switch (GameManager.instance.curQuest.questInfo.questType)
        {
            case QuestType.Normal:
                GameData.clearNormalCount++;
                break;
            case QuestType.Elite:
                GameData.clearEliteCount++;
                break;
            case QuestType.Boss:
                GameData.clearBossCount++;
                break;
        }
    }

    public void ScoreCount()
    {
        if(untouched && GameManager.instance.isWin)
        {
            if(GameManager.instance.curQuest.questInfo.questType == QuestType.Boss)
                GameData.untouchableCount++;
            else
                GameData.tapDanceCount++;
        }

        QuestClearCheck();

        GameData.monsterKillCount += monsterKill;

        if (GameManager.instance.isWin)
            GameData.speedGamer += SpeedRunPoint();
    }

    private void UnTouchedCheck()
    {
        if(untouched)
        {
            float curHp = GameManager.instance.player.curHp;
            if(playerHp > curHp)
            {
                untouched = false;
            }
            playerHp = curHp;
        }
    }

    private void MonsterKillCheck()
    {
        int curCount = GameManager.instance.enemys.Count;
        if (monsterCount > curCount)
        {
            monsterKill += monsterCount - curCount;
        }
        monsterCount = curCount;
    }

    private void CountCheck()
    {
        MonsterKillCheck();
        UnTouchedCheck();
    }

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance != null && GameManager.instance.player != null)
        {
            CountCheck();
        }
    }
}
