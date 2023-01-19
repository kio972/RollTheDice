using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest0 : Quests
{
    private bool isquestEnd = false;
    MissionUpdate updater;

    public override void SetBattle()
    {
        GameManager.instance.player.curRow = 0;
        GameManager.instance.player.curCol = 0;

        StoneMan stoneMan = GameManager.instance.enemySpace.GetComponentInChildren<StoneMan>();
        if(stoneMan == null)
        {
            stoneMan = Resources.Load<StoneMan>("Prefab/Monster/StoneMan");
            stoneMan = Instantiate(stoneMan, GameManager.instance.enemySpace);

            int index = MapController.instance.tileTransform.Count - 1;
            stoneMan.transform.position = MapController.instance.tileTransform[index].transform.position;
            stoneMan.curRow = MapController.instance.tiles[index].rowIndex;
            stoneMan.curCol = MapController.instance.tiles[index].colIndex;
        }
        stoneMan.Init();

        base.SetBattle();
    }

    public override void Init()
    {
        questInfo.questName = "마을의 위협 제거";
        questInfo.questType = QuestType.Elite;
        questInfo.questId = 0;
        questInfo.questDesc = "목표 : 스톤맨 처치\n\n내용\n돌을 던지는 미치광이가 있다고한다. 마을의 치안을 위해 처치하자.";
        questInfo.questReward.minGold = 80;
        questInfo.questReward.maxGold = 150;
        questInfo.questReward.minSkillPoint = 1;
        questInfo.questReward.maxSkillPoint = 3;
        questInfo.questReward.minPermanetPoint = 1;
        questInfo.questReward.maxPermanetPoint = 1;

        if(GameManager.instance != null)
            missionUpdater = GameManager.instance.uiManager.transform.Find("MissonUpdater");
    }

    public override void Update()
    {
        if(GameManager.instance != null && !isquestEnd)
        {
            if (GameManager.instance.enemys[0].curHp <= 0)
            {
                GameManager.instance.isWin = true;
                GameManager.instance.battleEnd = true;
                isquestEnd = true;
            }

            if (GameManager.instance.player.curHp <= 0)
            {
                GameManager.instance.isWin = false;
                GameManager.instance.battleEnd = true;
                isquestEnd = true;
            }
        }

        if (missionUpdater != null)
        {
            if (updater == null)
            {
                updater = Resources.Load<MissionUpdate>("Prefab/Quest/Mission");
                updater = Instantiate(updater, missionUpdater);
                updater.Init();
            }

            if (GameManager.instance.isWin)
                updater.isClear = true;
            updater.SetMission("스톤맨 처치");
        }
    }
}
