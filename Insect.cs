using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insect : EnemyController
{
    // 행동력 2회
    // 이동 or 공격
    // 공격시 무조건 턴종료
    // 기본적으로 npc 우선으로 이동하여 공격
    // npc없으면 플레이어 공격
    // 바로 옆칸에 플레이어있으면 거미줄뿌리기(행동력 -1) 후 이동 1칸
    // 이동후 본인이 있던 칸에 거미줄 뿌리고감(밟으면 없어지며 다음턴 행동력 -1)

    private void Attack(int index)
    {
        Controller target = GameManager.instance.mapController.FindIndexTarget(index);

        RotateChar(target.curRow, target.curCol);

        PlayerController player = target.GetComponent<PlayerController>();
        if(player != null)
        {
            target.TakeDamage(baseDamage, this);
        }
    }

    private void DropWeb(int index)
    {
        GameManager.instance.mapController.tiles[index].TileSideEffect("Prefab/Battle/Tiles/WebZone", 1);
    }

    IEnumerator InsectBehavior()
    {
        CameraController.instance.SetCameraPos(transform.position);
        float cameraWait = 0;
        while (cameraWait < 1)
        {
            cameraWait += Time.deltaTime;
            yield return null;
        }

        bool secontShot = true;
        int npcIndex = -1;
        StunedNPC npc = FindObjectOfType<StunedNPC>();
        if(npc != null)
            npcIndex = GameManager.instance.mapController.ReturnIndex(npc.curRow, npc.curCol);
        int curIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        float playerDist = GameManager.instance.mapController.GetHCost(curIndex, playerIndex);
        float npcDist = -1;
        if (npcIndex != -1)
            npcDist = GameManager.instance.mapController.GetHCost(curIndex, npcIndex);

        // npc와의 거리가 1칸이내일때 : npc우선공격
        if(npcDist == 1 || npcDist == 1.5f)
        {
            AttackAni();
            RotateChar(npc.curRow, npc.curCol);
            while (true)
            {
                if (goNext == true)
                {
                    goNext = false;
                    break;
                }
                yield return null;
            }
            npc.TakeDamage(baseDamage, this);
            secontShot = false;
        }
        else if (playerDist == 1 || playerDist == 1.5f) // 그외 플레이어와의 거리가 1칸이내일때
        {
            if(npcIndex != -1) // npc가 살아있는상황일경우
            {
                if (animator != null)
                    animator.SetTrigger("Web");
                RotateChar(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
                while (true)
                {
                    if (goNext == true)
                    {
                        goNext = false;
                        break;
                    }
                    yield return null;
                }
                // 플레이어에게 거미줄 투척
                GameManager.instance.player.TakeBuff("Prefab/Buff/Web", 5);
            }
            else // npc가 없으면 플레이어 공격
            {
                AttackAni();
                RotateChar(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
                while (true)
                {
                    if (goNext == true)
                    {
                        goNext = false;
                        break;
                    }
                    yield return null;
                }
                Attack(playerIndex);
                secontShot = false;
            }
        }
        else  // 그외 상황 -> 이동
        {
            // npc가 있다면 npc쪽으로 이동
            if (npcIndex != -1)
            {
                GameManager.instance.mapController.tiles[npcIndex].canMove = true;
                GameManager.instance.mapController.FindPath(curIndex, npcIndex);
            }
            else // 없으면 플레이어쪽으로 이동
            {
                GameManager.instance.mapController.tiles[playerIndex].canMove = true;
                GameManager.instance.mapController.FindPath(curIndex, playerIndex);
            }

            if(GameManager.instance.mapController.pathIndexs != null)
            {
                DropWeb(curIndex);
                GameManager.instance.mapController.FindPath(curIndex, GameManager.instance.mapController.pathIndexs[0]);
                GameManager.instance.moveing = StartCoroutine(MoveCharacter());
                MoveAni();
                while (GameManager.instance.moveing != null)
                {
                    yield return null;
                }
                ReturnToIdle();
            }
        }

        if(secontShot) // 첫행동력에 공격을 하지않았으면
        {
            float elapsed = 0;
            while (elapsed < 0.6f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            curIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
            if (npcIndex != -1) // npc가 살아있는 경우
            {
                npcDist = GameManager.instance.mapController.GetHCost(curIndex, npcIndex);
                if (npcDist == 1 || npcDist == 1.5f)
                {
                    AttackAni();
                    RotateChar(npc.curRow, npc.curCol);
                    while (true)
                    {
                        if (goNext == true)
                        {
                            goNext = false;
                            break;
                        }
                        yield return null;
                    }
                    npc.TakeDamage(baseDamage, this);
                    secontShot = false;
                }
                else
                {
                    GameManager.instance.mapController.tiles[npcIndex].canMove = true;
                    GameManager.instance.mapController.FindPath(curIndex, npcIndex);
                }
            }
            else // npc가 죽은상태면 플레이어 공격
            {
                playerDist = GameManager.instance.mapController.GetHCost(curIndex, playerIndex);
                if(playerDist == 1 || playerDist == 1.5f) // 플레이어와 거리가 1칸이면 공격
                {
                    AttackAni();
                    RotateChar(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
                    while (true)
                    {
                        if (goNext == true)
                        {
                            goNext = false;
                            break;
                        }
                        yield return null;
                    }
                    Attack(playerIndex);
                    secontShot = false;
                }
                else
                {
                    GameManager.instance.mapController.tiles[playerIndex].canMove = true;
                    GameManager.instance.mapController.FindPath(curIndex, playerIndex);
                }
            }

            if (secontShot) // 2번째턴에서 공격하지않았으면 이동해야함
            {
                if (GameManager.instance.mapController.pathIndexs != null)
                {
                    DropWeb(curIndex);
                    GameManager.instance.mapController.FindPath(curIndex, GameManager.instance.mapController.pathIndexs[0]);
                    GameManager.instance.moveing = StartCoroutine(MoveCharacter());
                    MoveAni();
                    while (GameManager.instance.moveing != null)
                    {
                        yield return null;
                    }
                    ReturnToIdle();
                }
            }
        }

        Invoke("EndEnemyTrun", 2.0f);
    }

    public override void Behavior()
    {
        if(!isDead)
        {
            StartCoroutine(InsectBehavior());
        }
        else
        {
            EndEnemyTrun();
        }
    }

    public override void Init()
    {
        enemyName = "에벌레";
        enemyDesc = "벌레벌레벌레벌레";

        baseDamage = 10;
        maxHp = 50;
        curHp = 50;
        maxAggroRange = 8;

        base.Init();
    }

}
