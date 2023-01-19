using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : EnemyController
{
    // �⺻����
    // �÷��̾���� �Ÿ�, ���ʿ��� �Ÿ��� Ȯ��, ����� ��󿡰� 1ĭ�̵�(����,�밢 �������)
    // 1ĭ �̵��� �����Ҽ� �ִٸ� ���� �ƴϸ� ������
    // ����� ���ʸ� ���� ����ı�, ���ʹ�������
    // �⺻������ 7, ��ȭ�� 15
    // �⺻ü�� 10, ��ȭ�� 30
    // ��ȭ���¿��� ��� �� ���ʹ����Ұ� �÷��̾�� ���ʹ��� 1���� �ο�

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

        // �����Ȼ��°� �ƴ϶�� ��� Ž��
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
                // ��� Ÿ�ٰ���
                Attack(targetIndexs[nearestTarget]);
            }
            else
            {
                // Ÿ�ٹ������� ��ĭ �̵� �� Ÿ�ٰ��ݰ����ϸ� ����
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
        enemyName = "������";
        enemyDesc = "���ʸ� �����ϴ� ������";
        maxHp = 10;
        curHp = 10;

        base.Init();
    }
}
