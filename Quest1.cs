using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1 : Quests
{
    public bool isQuestEnd = false;
    public List<int> slimeSpawnIndex;
    MissionUpdate updater;
    HerbCollect herb;
    int herbStack = 0;
    public override void SetEndTurn()
    {
        Slime[] slimes = GameManager.instance.enemySpace.GetComponentsInChildren<Slime>();
        if(slimes.Length < 3)
        {
            Herb herb = GameManager.instance.enemySpace.GetComponentInChildren<Herb>();
            if(herb != null)
            {
                SpawnSlime();
            }
        }
    }

    public void SpawnSlime()
    {
        int availIndex = 0;
        while (true)
        {
            availIndex = Random.Range(0, slimeSpawnIndex.Count);
            int index = slimeSpawnIndex[availIndex];
            if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None)
            {
                string slime = "Prefab/Monster/Slime";
                GameManager.instance.mapController.SpawnEnemy(slime, index);
                break;
            }
        }
        GameManager.instance.UpdaetEnemys();
    }

    public override void SetBattle()
    {
        GameManager.instance.mapController.UpdateOnTargetTiles();

        for(int i = 0; i < 3; i++)
        {
            int index = 0;
            while (true)
            {
                index = Random.Range(0, GameManager.instance.mapController.tiles.Count);
                if (GameManager.instance.mapController.tiles[index].onTarget == OnTile.None)
                    break;
            }
            string herb = "Prefab/Monster/Herb";
            GameManager.instance.mapController.SpawnEnemy(herb, index);
        }

        slimeSpawnIndex = new List<int>();
        // 0�� 0 ~ colSize - 1
        // 1�� 0, colsize -1
        // ...rowsize - 1�� 0 ~ colSize -1
        for(int i = 0; i < GameManager.instance.mapController.mapRowsize; i++)
        {
            int size = GameManager.instance.mapController.mapRowsize;
            if (i == 0 || i == GameManager.instance.mapController.mapRowsize - 1)
            {
                for(int j = 0; j < GameManager.instance.mapController.mapColsize; j++)
                {
                    slimeSpawnIndex.Add(GameManager.instance.mapController.tiles[(i * size) + j].tileIndex);
                }
            }
            else
            {
                
                // rowsize ���, rowsize ���,-1 �߰��ؾ���
                // i = 1
                slimeSpawnIndex.Add(GameManager.instance.mapController.tiles[i * size].tileIndex);
                slimeSpawnIndex.Add(GameManager.instance.mapController.tiles[i * size + size - 1].tileIndex);
            }
        }

        SpawnSlime();

        base.SetBattle();
    }

    public override void Init()
    {
        questInfo.questName = "���� ����";
        questInfo.questType = QuestType.Normal;
        questInfo.questId = 1;
        questInfo.questDesc = "��ǥ : ���� 3�� ����\n\n����\n���ʼ������� �ʿ��� �ڻ��ϴ� ������ ������ ��Ź�ߴ�. �����ӵ��� ���� �� ������ ��������.";
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
        if (GameManager.instance != null)
        {
            if(herb == null)
                herb = GameManager.instance.player.tokenSpace.GetComponentInChildren<HerbCollect>();

            if (herb != null)
                herbStack = herb.stack;

            if (herbStack >= 3)
            {
                GameManager.instance.isWin = true;
                GameManager.instance.battleEnd = true;
            }
            else if(GameManager.instance.player.curHp <= 0)
            {
                GameManager.instance.isWin = false;
                GameManager.instance.battleEnd = true;
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

            updater.SetMission("���� ���� " + herbStack.ToString() + " / 3");
        }

    }
}
