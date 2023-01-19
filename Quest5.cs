using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest5 : Quests
{
    public bool isQuestEnd = false;
    private bool isQuestStart = false;
    MissionUpdate updater1;
    MissionUpdate updater2;
    [SerializeField]
    private EnemyController merchant;

    public override void SetEndTurn()
    {
        
    }


    public override void SetBattle()
    {
        List<int> occupiedIndex = new List<int>();
        int index = 0;
        for(int i = 0; i < 2; i++)
        {
            GameManager.instance.mapController.UpdateOnTargetTiles();
            while(true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None && !occupiedIndex.Contains(index))
                {
                    occupiedIndex.Add(index);
                    break;
                }
            }

            GameManager.instance.mapController.SpawnEnemy("Prefab/Monster/Bandit", index);
        }

        for (int i = 0; i < 2; i++)
        {
            GameManager.instance.mapController.UpdateOnTargetTiles();
            while (true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None && !occupiedIndex.Contains(index))
                {
                    occupiedIndex.Add(index);
                    break;
                }
            }
            GameManager.instance.mapController.SpawnEnemy("Prefab/Monster/Insect", index);
        }

        if(merchant != null)
        {
            merchant.Init();
            GameManager.instance.mapController.UpdateOnTargetTiles();
            while (true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None && !occupiedIndex.Contains(index))
                {
                    occupiedIndex.Add(index);
                    break;
                }
            }
            merchant.transform.position = GameManager.instance.mapController.tiles[index].transform.position;
            merchant.curRow = GameManager.instance.mapController.tiles[index].rowIndex;
            merchant.curCol = GameManager.instance.mapController.tiles[index].colIndex;
        }
        

        GameManager.instance.UpdaetEnemys();
        isQuestStart = true;

        base.SetBattle();
    }

    public override void Init()
    {
        questInfo.questName = "마을 주변 몬스터 토벌";
        questInfo.questType = QuestType.Elite;
        questInfo.questId = 5;
        questInfo.questDesc = "목표 : 모든 적 토벌\n\n내용\n모든 몬스터들을 토벌하자.";
        questInfo.questReward.minGold = 80;
        questInfo.questReward.maxGold = 150;
        questInfo.questReward.minSkillPoint = 1;
        questInfo.questReward.maxSkillPoint = 3;
        questInfo.questReward.minPermanetPoint = 1;
        questInfo.questReward.maxPermanetPoint = 1;

        if (GameManager.instance != null)
            missionUpdater = GameManager.instance.uiManager.transform.Find("MissonUpdater");
    }

    private bool SecondMissionCheck()
    {
        StunedNPC npc = GameManager.instance.enemySpace.GetComponentInChildren<StunedNPC>();
        if (npc == null)
            return false;

        return true;
    }

    // Update is called once per frame
    public override void Update()
    {
        
        if (missionUpdater != null)
        {
            if (updater1 == null)
            {
                updater1 = Resources.Load<MissionUpdate>("Prefab/Quest/Mission");
                updater1 = Instantiate(updater1, missionUpdater);
                updater1.Init();
            }

            if (GameManager.instance.isWin)
                updater1.isClear = true;

            updater1.SetMission("모든 적 처치");

            if (updater2 == null)
            {
                updater2 = Resources.Load<MissionUpdate>("Prefab/Quest/Mission");
                updater2 = Instantiate(updater1, missionUpdater);
                updater2.Init();
            }
            else
                updater2.SetMission("(선택) 쓰러진 행인 생존");

            if (SecondMissionCheck())
                updater2.isClear = true;
            else
                updater2.SetMissionFail();
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
                    {
                        StunedNPC npc = enemy.GetComponent<StunedNPC>();
                        if (npc != null)
                            continue;
                        return;
                    }
                }

                GameManager.instance.isWin = true;
                GameManager.instance.battleEnd = true;
                isQuestEnd = true;
            }
        }
    }
}
