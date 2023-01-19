using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;

    public Transform enemySpace;
    public List<EnemyController> enemys;
    
    public List<int> objects;
    public UIManager uiManager;
    public SkillManager skillManager;
    public MapController2 mapController;

    public Quests curQuest;

    public int phase = -1;
    public int turn = 1;

    public bool moveInput = false;
    public bool endInput = false;

    public int attackTargetRow;
    public int attackTargetCol;
    public bool attackInput = false;

    public bool enemyAttack;
    public int enemyCount = 0;

    public Coroutine turnPopUp;
    public Coroutine moveing;
    public Coroutine turnEnding;
    
    public bool dicePhase = false;
    public bool dicePhaseEnd = false;
    public int diceNum;

    public bool battleEnd = false;
    public bool isWin;

    private bool isEnd = false;

    // 업데이트 제어용 변수
    public bool ready = false;

    private TurnPopUp popUp;
    // phase 0 = 시작단계 (선 후공 추가가능)
    // 플레이어 턴
    // phase 1 = 주사위 굴림(플레이어)
    // phase 2 = 이동선택
    // phase 3 = 행동선택
    // phase 4 = 몬스터 행동
    // phase 5 = 종료단계 -> 시작단계로

    public void UpdaetEnemys()
    {
        enemys = new List<EnemyController>();
        EnemyController[] updateEnemy = enemySpace.GetComponentsInChildren<EnemyController>();
        if (updateEnemy != null)
        {
            for (int i = 0; i < updateEnemy.Length; i++)
            {
                enemys.Add(updateEnemy[i]);
            }
        }
    }


    private void EndTurn()
    {
        foreach (EnemyController enemyController in enemys)
        {
            if (enemyController.isDead)
                Destroy(enemyController.gameObject);
        }
        Invoke("UpdaetEnemys", 0.1f);

        // 퀘스트 엔드턴 트리거 작동
        if(curQuest != null)
            curQuest.SetEndTurn();

        if(turnEnding == null)
        {
            phase = 1;
            turn++;
        }
    }

    private void EnemyTrun()
    {
        // 적 공격
        if (!endInput)
        {
            endInput = true;
            TurnPopUp pop = Instantiate(popUp, uiManager.transform);
            pop.SetPopUp("적들의 턴!", 0.5f);
        }

        if(turnPopUp == null)
        {
            if (enemyCount > enemys.Count) // 적공격 완료되면 페이즈증가
            {
                if(!enemyAttack)
                {
                    mapController.UpdateOnTargetTiles();
                    enemyCount = 0;
                    phase++;
                    endInput = false;
                }
                return;
            }

            if (!enemyAttack) // 적이 공격상태가 아니면, 공격상태로만들고 공격함수호출, 다음 호출시 다음몬스터가 공격하게 enemycount증가
            {
                if (enemyCount + 1 <= enemys.Count)
                {
                    enemyAttack = true;

                    enemys[enemyCount].Behavior();
                    mapController.UpdateOnTargetTiles();
                }
                enemyCount++;
            }
        }
    }

    private void SelectAttack()
    {
        skillManager.gameObject.SetActive(true);

        if (skillManager.currSkill != null)
        {
            mapController.gameObject.SetActive(true);
            mapController.ResetTilesColor();
            uiManager.UpdateCharDir();
            // 타겟범위가 0이 아니라면 (지점형 스킬이면) 우선 사거리 표시
            if (skillManager.currSkill.skillInfo.targetRange == 0)
            {
                attackInput = true;
                attackTargetRow = player.curRow;
                attackTargetCol = player.curCol;
            }
            else
            {
                mapController.BasicRangeDraw(skillManager.currSkill.skillInfo.targetRange, Color.blue);
            }

            if (attackInput)
            {
                mapController.DrawAttackRange(skillManager.currSkill, Color.red, attackTargetRow, attackTargetCol);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mapController.ResetTilesColor();
            skillManager.currSkill = null;
            if (MousePointer.instance != null)
                MousePointer.instance.mouseType = MouseType.Normal;
            DescManager.instance.DescOff();
            attackInput = false;
            player.ReturnToIdle();
        }

        if (endInput)
        {
            phase++;
            endInput = false;
            
            skillManager.currSkill = null;
            skillManager.gameObject.SetActive(false);
            
            mapController.ResetTilesColor();
            mapController.UpdateOnTargetTiles();
            uiManager.helpText.gameObject.SetActive(false);
            uiManager.endTurn.gameObject.SetActive(false);
            uiManager.tableGate.SetBool("Open", false);
            if (MousePointer.instance != null)
                MousePointer.instance.mouseType = MouseType.Normal;
            int stack = (int)player.stack;
            player.stack = stack;
            uiManager.stackUpdater.UpdateStack();

            if (DescManager.instance != null)
                DescManager.instance.DescOff();
            player.ReturnToIdle();
        }
    }

    private void SelectMove()
    {
        // 이동 할 위치 고를때까지 대기
        if(moveInput)
        {
            moveInput = false;
            moveing = StartCoroutine(player.MoveCharacter());

            uiManager.stackUpdater.UpdateStack();
        }
        // 고르면 확인버튼띄워서 한번 더 묻기
        
        // 확인버튼 누를때까지 무조건 반복

        // 확인버튼 누르면 다음페이즈
        if(endInput)
        {
            mapController.UpdateOnTargetTiles();
            if(mapController.curTile != null)
            {
                mapController.curTile.DestroyGuide();
                mapController.curTile = null;
            }
            phase++;
            endInput = false;
            uiManager.tableGate.SetBool("Open", true);
            
            CameraController.instance.SetCameraPos(player.transform.position);

            uiManager.helpText.text = "주사위를 선택하여 공격";
            uiManager.endTurn.text.text = "턴종료";
        }
    }

    private void DicePhaseEnd()
    {
        player.stack += diceNum;
        if (uiManager.stackUpdater != null)
            uiManager.stackUpdater.UpdateStack();
        phase++;
        dicePhase = false;
        dicePhaseEnd = false;

        DescManager.instance.DescOff();

        uiManager.helpText.gameObject.SetActive(true);
        uiManager.helpText.text = "이동할 타일을 선택하여 이동";
        uiManager.endTurn.gameObject.SetActive(true);
        uiManager.endTurn.text.text = "이동종료";
        player.tokenSpace.BroadcastMessage("Effect", SendMessageOptions.DontRequireReceiver); // 이동페이즈 진입시 효과발동 버프/디버프
        player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);
        mapController.UpdateOnTargetTiles();
    }

    private void DicePhase()
    {
        if (dicePhase)
        {
            RollDice();
            return;
        }

        if (!endInput)
        {
            endInput = true;
            TurnPopUp pop = Instantiate(popUp, uiManager.transform);
            pop.SetPopUp("플레이어의 턴!", 0.5f);
            CameraController.instance.SetCameraPos(player.transform.position);
        }

        if (turnPopUp == null)
        {
            endInput = false;
            player.tokenSpace.BroadcastMessage("Effect", SendMessageOptions.DontRequireReceiver);
            foreach (EnemyController enemy in enemys)
            {
                if(enemy.tokenSpace != null)
                    enemy.tokenSpace.BroadcastMessage("Effect", SendMessageOptions.DontRequireReceiver);
            }
            skillManager.CoolDown();
            dicePhase = true;
            DescManager.instance.DiceOn();
            uiManager.helpText.gameObject.SetActive(true);
            uiManager.helpText.text = "왼클릭을 눌러 주사위 굴리기";
        }
    }

    private void RollDice()
    {
        if (dicePhaseEnd)
        {
            DicePhaseEnd();
            return;
        }
    }

    private void Win()
    {
        BattleWin win = Resources.Load<BattleWin>("Prefab/UI/BattleWin");
        win = Instantiate(win);
        win.SetReward(curQuest.questInfo.questReward);
        print("플레이어 승리");
    }

    private void Lose()
    {
        BattleLose lose = Resources.Load<BattleLose>("Prefab/UI/BattleLose");
        lose = Instantiate(lose);
    }

    private void ReadyBattle()
    {
        //퀘스트 함수에서 몬스터 소환등 게임준비하게
        if(!isEnd)
        {
            curQuest.SetBattle();
            isEnd = true;
        }
        
        if(endInput)
        {
            player.transform.position = mapController.tiles[mapController.ReturnIndex(player.curRow, player.curCol)].transform.position;
            phase = 0;
            OpenScene();
            isEnd = false;
            endInput = false;
        }
    }

    private void BattleUpdate()
    {
        if (!battleEnd)
        {
            switch (phase)
            {
                case -1:
                    ReadyBattle();
                    break;
                case 0:
                    if (!endInput)
                    {
                        AudioClip clip = Resources.Load<AudioClip>("Sounds/BackGround/battle");
                        AudioManager.Instance.PlayBG(clip);
                        endInput = true;
                        TurnPopUp pop = Instantiate(popUp, uiManager.transform);
                        pop.SetPopUp("전투시작!", 1f);
                    }

                    if (turnPopUp == null)
                    {
                        player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);
                        endInput = false;
                        phase++;
                    }
                    break;
                case 1:
                    DicePhase();
                    break;
                case 2:
                    //print("이동페이즈");
                    SelectMove();
                    break;
                case 3:
                    //print("공격페이즈");
                    if (skillManager.currSkill == null || skillManager.currSkill != null && skillManager.effect == null)
                        SelectAttack();
                    break;
                case 4:
                    //print("몬스터행동");
                    EnemyTrun();
                    break;
                case 5:
                    EndTurn();
                    break;
            }
        }
        else if (!isEnd && battleEnd)
        {
            ScoreCounter.instance.ScoreCount();

            AudioManager.Instance.StopBG();

            if (isWin)
            {
                Invoke("Win", 2.0f);
            }
            else
            {
                Invoke("Lose", 2.0f);
            }

            isEnd = true;
        }
    }

    private void OpenScene()
    {
        FadeOut fade = FindObjectOfType<FadeOut>();
        if (fade != null)
            fade.FadeI();

    }

    private void Init()
    {
        instance = this;

        popUp = Resources.Load<TurnPopUp>("Prefab/UI/PopUp");

        uiManager = FindObjectOfType<UIManager>();
        uiManager.Init();

        skillManager = uiManager.GetComponentInChildren<SkillManager>();
        skillManager.Init();
        skillManager.gameObject.SetActive(false);

        player = FindObjectOfType<PlayerController>();
        player.Init();

        enemySpace = GameObject.Find("EnemyGroup").GetComponent<Transform>();
        UpdaetEnemys();
        
        objects = new List<int>();

        curQuest = GetComponentInChildren<Quests>();
        if(curQuest != null)
            curQuest.Init();

        mapController = FindObjectOfType<MapController2>();
        if(mapController != null && !mapController.isinit)
        {
            mapController.Init();
        }

        ready = true;

        Time.timeScale = GameSpeedController.SetSpeed(SaveManager.Instance.settingData.gameSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
            return;

        if (Pause.instance != null && Pause.instance.pause)
            return;

        BattleUpdate();
    }
}
