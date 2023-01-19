using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest2 : Quests
{
    public bool isQuestEnd = false;
    private bool isQuestStart = false;
    MissionUpdate updater;
    public override void SetEndTurn()
    {
        
    }


    public override void SetBattle()
    {
        for(int i = 0; i < 3; i++)
        {
            GameManager.instance.mapController.UpdateOnTargetTiles();
            int index = 0;
            while(true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None)
                    break;
            }

            GameManager.instance.mapController.SpawnEnemy("Prefab/Monster/Bandit", index);
        }

        GameManager.instance.UpdaetEnemys();
        isQuestStart = true;

        base.SetBattle();
    }

    public override void Init()
    {
        questInfo.questName = "고블린 퇴치";
        questInfo.questType = QuestType.Normal;
        questInfo.questId = 2;
        questInfo.questDesc = "목표 : 모든 적 토벌\n\n내용\n마을사람들을 괴롭히는 고블린들을 퇴치하자.";
        questInfo.questReward.minGold = 50;
        questInfo.questReward.maxGold = 80;
        questInfo.questReward.minSkillPoint = 1;
        questInfo.questReward.maxSkillPoint = 1;

        if (GameManager.instance != null)
            missionUpdater = GameManager.instance.uiManager.transform.Find("MissonUpdater");
    }

    // Update is called once per frame
    public override void Update()
    {
        
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

            updater.SetMission("모든 적 처치");
        }

        if (isQuestStart)
        {
            if (GameManager.instance != null && !isQuestEnd)
            {
                if (GameManager.instance.player.curHp <= 0)
                {
                    GameManager.instance.isWin = false;
                    GameManager.instance.battleEnd = true;
                    isQuestEnd = true;
                    return;
                }

                foreach (EnemyController enemy in GameManager.instance.enemys)
                {
                    if (!enemy.isDead)
                        return;
                }

                GameManager.instance.isWin = true;
                GameManager.instance.battleEnd = true;
                isQuestEnd = true;
            }
        }
    }
}
