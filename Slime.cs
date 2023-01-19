using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : EnemyController
{
    // 기본패턴
    // 플레이어와의 거리, 약초와의 거리를 확인, 가까운 대상에게 1칸이동(십자,대각 상관없이)
    // 1칸 이동후 공격할수 있다면 공격 아니면 턴종료
    // 대상이 약초면 약초 즉시파괴, 약초버프얻음
    // 기본데미지 7, 강화시 15
    // 기본체력 10, 강화시 30
    // 강화상태에서 사망 시 약초버프잃고 플레이어에게 약초버프 1스택 부여

    private bool isBuffed = false;

    public override void RotateChar(int rowIndex, int colIndex)
    {
        int dirRow = rowIndex - curRow;
        int dirCol = colIndex - curCol;
        SetDir(dirRow, dirCol);

        if (curDir == CharDir.W || curDir == CharDir.D)
            charImg.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            charImg.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public override void TakeDamage(float damage, Controller attacker)
    {
        base.TakeDamage(damage, attacker);
        
        if(isBuffed && isDead)
        {
            GameManager.instance.player.TakeBuff("Prefab/Buff/HerbCollect", 1);
            RemoveBuff(1);
        }
    }

    private void GetBuff()
    {
        isBuffed = true;
        maxHp += 20;
        curHp += 20;
        SpriteRenderer image = GetComponentInChildren<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>("Img/Monster/slime_red");
        image.sprite = sprite;
    }

    private void Attack(int index)
    {
        Controller target = GameManager.instance.mapController.FindIndexTarget(index);

        RotateChar(target.curRow, target.curCol);
        AttackAni();

        PlayerController player = target.GetComponent<PlayerController>();
        if(player != null)
        {
            if(isBuffed)
            {
                target.TakeDamage(15, this);
            }
            else
            {
                target.TakeDamage(7, this);
            }
            return;
        }

        Herb herb = target.GetComponent<Herb>();
        if(herb != null)
        {
            herb.DestroyHerb();
            TakeBuff("Prefab/Buff/HerbCollect", 1);
            GetBuff();
        }
    }

    IEnumerator SlimeBehavior()
    {
        CameraController.instance.SetCameraPos(transform.position);
        float cameraWait = 0;
        while (cameraWait < 1)
        {
            cameraWait += Time.deltaTime;
            yield return null;
        }

        int curIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        List<int> targetIndexs = new List<int>();
        targetIndexs.Add(playerIndex);

        // 버프된상태가 아니라면 허브 탐색
        if(!isBuffed)
        {
            Herb[] herbs = GameManager.instance.enemySpace.GetComponentsInChildren<Herb>();
            foreach (Herb herb in herbs)
            {
                targetIndexs.Add(GameManager.instance.mapController.ReturnIndex(herb.curRow, herb.curCol));
            }
        }

        foreach(int index in targetIndexs)
        {
            GameManager.instance.mapController.tiles[index].canMove = true;
        }

        int nearestTarget = -1;
        float nearestCost = 1000000;
        for(int i = 0; i < targetIndexs.Count; i++)
        {
            GameManager.instance.mapController.FindPath(curIndex, targetIndexs[i]);
            if(GameManager.instance.mapController.pathCost < nearestCost)
            {
                nearestCost = GameManager.instance.mapController.pathCost;
                nearestTarget = i;
            }
        }

        if (nearestCost <= maxAggroRange)
        {
            if (nearestCost == 1 || nearestCost == 1.5f)
            {
                // 즉시 타겟공격
                Attack(targetIndexs[nearestTarget]);
            }
            else
            {
                // 타겟방향으로 한칸 이동 후 타겟공격가능하면 공격
                GameManager.instance.mapController.FindPath(curIndex, targetIndexs[nearestTarget]);

                if (GameManager.instance.mapController.pathIndexs.Count > 0)
                {
                    int moveIndex = 0;
                    moveIndex = GameManager.instance.mapController.pathIndexs[0];
                    GameManager.instance.mapController.FindPath(curIndex, moveIndex);
                    GameManager.instance.moveing = StartCoroutine(MoveCharacter());
                    MoveAni();
                    while (GameManager.instance.moveing != null)
                    {
                        yield return null;
                    }
                    ReturnToIdle();

                    foreach (int index in targetIndexs)
                    {
                        GameManager.instance.mapController.tiles[index].canMove = true;
                    }

                    curIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
                    GameManager.instance.mapController.FindPath(curIndex, targetIndexs[nearestTarget]);
                    if (GameManager.instance.mapController.pathCost == 1 || GameManager.instance.mapController.pathCost == 1.5f)
                    {
                        Attack(targetIndexs[nearestTarget]);
                    }
                }
            }

            //foreach (int index in targetIndexs)
            //{
            //    GameManager.instance.mapController.tiles[index].canMove = false;
            //}
        }
        
        Invoke("EndEnemyTrun", 2.0f);
    }

    public override void Behavior()
    {
        if(!isDead)
        {
            StartCoroutine(SlimeBehavior());
        }
        else
        {
            EndEnemyTrun();
        }
    }

    public override void Init()
    {
        enemyName = "슬라임";
        enemyDesc = "약초를 좋아하는 슬라임";
        maxHp = 10;
        curHp = 10;

        base.Init();
    }
}
