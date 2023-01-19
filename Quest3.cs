using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest3 : Quests
{
    public bool isQuestEnd = false;
    private bool isQuestStart = false;
    MissionUpdate updater1;
    MissionUpdate updater2;

    private int merchantIndex = 99;

    private List<int> goldIndexs = new List<int>();

    private int collect = 0;
    public override void SetEndTurn()
    {
        if(GameManager.instance.turn % 3 == 0)
        {
            GameManager.instance.mapController.UpdateOnTargetTiles();
            int index = 0;
            while (true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None)
                    break;
            }

            GameManager.instance.mapController.SpawnEnemy("Prefab/Monster/Bandit", index);
        }
    }


    public override void SetBattle()
    {
        GameManager.instance.objects.Add(merchantIndex);

        for(int i = 0; i < 3; i++)
        {
            GameManager.instance.mapController.UpdateOnTargetTiles();
            int index = 0;
            while (true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None)
                    break;
            }

            GameObject gold = Resources.Load<GameObject>("Prefab/Object/Gold");
            gold = Instantiate(gold, GameManager.instance.mapController.tiles[index].transform);
            gold.name = "Gold";
            GameManager.instance.objects.Add(index);
            goldIndexs.Add(index);
        }

        for(int i = 0; i < 2; i++)
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
        questInfo.questName = "조난된 상인 구출";
        questInfo.questType = QuestType.Normal;
        questInfo.questId = 3;
        questInfo.questDesc = "목표 : 잃어버린 상인의 짐을 되찾고 상인에게 도달\n\n내용\n마을의 보부상이 도적들에게 습격을 받았다고한다. 상인의 짐을 되찾아 구출하자.";
        questInfo.questReward.minGold = 30;
        questInfo.questReward.maxGold = 31;
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
            if (updater1 == null)
            {
                updater1 = Resources.Load<MissionUpdate>("Prefab/Quest/Mission");
                updater1 = Instantiate(updater1, missionUpdater);
                updater1.Init();
            }
            else
                updater1.SetMission("상인에게 도달하여 구출하기");

            if (updater2 == null)
            {
                updater2 = Resources.Load<MissionUpdate>("Prefab/Quest/Mission");
                updater2 = Instantiate(updater1, missionUpdater);
                updater2.Init();
            }
            else
                updater2.SetMission("(선택) 상인의 짐 되찾기. \n현재 찾은 짐 : " + collect.ToString());

            if (collect >= 1)
                updater2.isClear = true;

            if (GameManager.instance.isWin)
                updater1.isClear = true;
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

                int removeIndex = -1;
                int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
                foreach(int goldIndex in goldIndexs)
                {
                    if(playerIndex == goldIndex)
                    {
                        removeIndex = goldIndex;
                        collect++;
                        AudioManager.Instance.PlayEffect("buy");
                        break;
                    }

                    for(int i = 0; i < GameManager.instance.enemys.Count; i++)
                    {
                        int enemyIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.enemys[i].curRow, GameManager.instance.enemys[i].curCol);
                        if (enemyIndex == goldIndex)
                        {
                            removeIndex = goldIndex;
                            AudioManager.Instance.PlayEffect("buy");
                            break;
                        }
                    }
                }

                if (removeIndex != -1)
                {
                    Transform gold = GameManager.instance.mapController.tiles[removeIndex].transform.Find("Gold");
                    Destroy(gold.gameObject);
                    goldIndexs.Remove(removeIndex);
                    GameManager.instance.mapController.tiles[removeIndex].canMove = true;
                }

                if(playerIndex == merchantIndex)
                {
                    questInfo.questReward.minGold += collect * 20;
                    questInfo.questReward.maxGold += collect * 25;
                    GameManager.instance.isWin = true;
                    GameManager.instance.battleEnd = true;
                    isQuestEnd = true;
                }
            }
        }
    }
}
