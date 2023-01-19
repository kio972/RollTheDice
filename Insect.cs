using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insect : EnemyController
{
    // �ൿ�� 2ȸ
    // �̵� or ����
    // ���ݽ� ������ ������
    // �⺻������ npc �켱���� �̵��Ͽ� ����
    // npc������ �÷��̾� ����
    // �ٷ� ��ĭ�� �÷��̾������� �Ź��ٻѸ���(�ൿ�� -1) �� �̵� 1ĭ
    // �̵��� ������ �ִ� ĭ�� �Ź��� �Ѹ���(������ �������� ������ �ൿ�� -1)

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

        // npc���� �Ÿ��� 1ĭ�̳��϶� : npc�켱����
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
        else if (playerDist == 1 || playerDist == 1.5f) // �׿� �÷��̾���� �Ÿ��� 1ĭ�̳��϶�
        {
            if(npcIndex != -1) // npc�� ����ִ»�Ȳ�ϰ��
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
                // �÷��̾�� �Ź��� ��ô
                GameManager.instance.player.TakeBuff("Prefab/Buff/Web", 5);
            }
            else // npc�� ������ �÷��̾� ����
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
        else  // �׿� ��Ȳ -> �̵�
        {
            // npc�� �ִٸ� npc������ �̵�
            if (npcIndex != -1)
            {
                GameManager.instance.mapController.tiles[npcIndex].canMove = true;
                GameManager.instance.mapController.FindPath(curIndex, npcIndex);
            }
            else // ������ �÷��̾������� �̵�
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

        if(secontShot) // ù�ൿ�¿� ������ �����ʾ�����
        {
            float elapsed = 0;
            while (elapsed < 0.6f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            curIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
            if (npcIndex != -1) // npc�� ����ִ� ���
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
            else // npc�� �������¸� �÷��̾� ����
            {
                playerDist = GameManager.instance.mapController.GetHCost(curIndex, playerIndex);
                if(playerDist == 1 || playerDist == 1.5f) // �÷��̾�� �Ÿ��� 1ĭ�̸� ����
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

            if (secontShot) // 2��°�Ͽ��� ���������ʾ����� �̵��ؾ���
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
        enemyName = "������";
        enemyDesc = "����������������";

        baseDamage = 10;
        maxHp = 50;
        curHp = 50;
        maxAggroRange = 8;

        base.Init();
    }

}
