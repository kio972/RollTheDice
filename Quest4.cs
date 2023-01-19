using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest4 : Quests
{
    private bool isquestEnd = false;
    MissionUpdate updater;

    [SerializeField]
    MapController2 firstF;
    [SerializeField]
    MapController2 secondF;
    
    private bool isUpstair = false;

    public bool portalOpen = false;
    private int portalRow = 3;
    private int portalCol = 3;

    [SerializeField]
    private ParticleSystem portalEffect;
    private bool portalEffectStart = false;

    public int PortalIndex
    {
        get
        {
            return GameManager.instance.mapController.ReturnIndex(portalRow, portalCol);
        }
    }

    [SerializeField]
    private Button portalBtn;

    public List<EnemyController> firstFMonsters = new List<EnemyController>();
    public EnemyController secondFMonster;

    private Coroutine portalMove;

    private int movedTurn = 0;

    private void SetTo1F()
    {
        isUpstair = false;
        GameManager.instance.mapController = firstF;
        //2층몬스터 disable하고 update
        foreach(EnemyController enemy in firstFMonsters)
        {
            enemy.gameObject.SetActive(true);
        }
        secondFMonster.gameObject.SetActive(false);
    }

    private void SetTo2F()
    {
        isUpstair = true;
        GameManager.instance.mapController = secondF;

        if (!GameManager.instance.mapController.isinit)
            GameManager.instance.mapController.Init();

        //1층몬스터 disable하고 update
        if(movedTurn != GameManager.instance.turn)
        {
            int random = -1;
            do
                random = Random.Range(0, GameManager.instance.mapController.tiles.Count - 1);
            while (random == GameManager.instance.mapController.ReturnIndex(portalRow, portalCol));
            secondFMonster.transform.position = GameManager.instance.mapController.tiles[random].transform.position;
            secondFMonster.curRow = GameManager.instance.mapController.tiles[random].rowIndex;
            secondFMonster.curCol = GameManager.instance.mapController.tiles[random].colIndex;
            movedTurn = GameManager.instance.turn;
        }
        
        secondFMonster.gameObject.SetActive(true);
        foreach (EnemyController enemy in firstFMonsters)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void PortalUse()
    {
        if (GameManager.instance.player.move != null)
            return;

        firstF.gameObject.SetActive(isUpstair);
        secondF.gameObject.SetActive(!isUpstair);
        if (isUpstair)
            SetTo1F();
        else
            SetTo2F();

        portalEffect.transform.position = GameManager.instance.mapController.tiles[PortalIndex].transform.position;
        GameManager.instance.UpdaetEnemys();
        // 플레이어 이동
        int index = GameManager.instance.mapController.ReturnIndex(portalRow, portalCol);
        Vector3 nextPos = GameManager.instance.mapController.tiles[index].transform.position;

        GameManager.instance.player.move = StartCoroutine(GameManager.instance.player.Move(nextPos, 2, true));
        CameraController.instance.SetMaxSize();
        CameraController.instance.SetCameraPos(nextPos, 3);
    }

    public override void SetBattle()
    {
        GameManager.instance.player.curRow = 3;
        GameManager.instance.player.curCol = 3;

        portalBtn.onClick.AddListener(PortalUse);

        // 연출필요(발록 -> 골렘 소환 -> 포탈 비활성화)

        foreach(EnemyController enemy in firstFMonsters)
        {
            enemy.Init();
        }

        secondFMonster.Init();
        secondFMonster.gameObject.SetActive(false);

        GameManager.instance.UpdaetEnemys();
        base.SetBattle();
    }

    public override void Init()
    {
        questInfo.questName = "마을의 위협 제거";
        questInfo.questType = QuestType.Boss;
        questInfo.questId = 4;
        questInfo.questDesc = "목표 : 발록 처치\n\n내용\n몰루";
        questInfo.questReward.minGold = 100;
        questInfo.questReward.maxGold = 200;
        questInfo.questReward.minSkillPoint = 2;
        questInfo.questReward.maxSkillPoint = 3;
        questInfo.questReward.minPermanetPoint = 1;
        questInfo.questReward.minPermanetPoint = 2;



        if(GameManager.instance != null)
            missionUpdater = GameManager.instance.uiManager.transform.Find("MissonUpdater");
    }

    private void EffectCheck()
    {
        if(!portalOpen)
        {
            portalBtn.gameObject.SetActive(false);
            portalEffect.Stop();
        }
        else if(!portalEffectStart)
        {
            portalEffect.Play();
            portalEffectStart = true;
        }

        //포탈 활성화 / 비활성화 여부 체크, 플레이어가 활성화된 포탈밟으면 맵전환해주기
        if (portalOpen && GameManager.instance.phase == 2 &&
            GameManager.instance.player.curRow == portalRow && GameManager.instance.player.curCol == portalCol)
            portalBtn.gameObject.SetActive(true);
        else
            portalBtn.gameObject.SetActive(false);
    }

    private void QuestClearCheck()
    {
        if (GameManager.instance != null && !isquestEnd)
        {
            // 발록죽으면 끝나게
            if (secondFMonster.isDead)
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
            updater.SetMission("발록 처치");
        }
    }

    public override void Update()
    {
        if(GameManager.instance != null)
        {
            EffectCheck();
            QuestClearCheck();
        }
    }
}
