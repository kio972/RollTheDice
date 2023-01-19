using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balog : EnemyController
{
    [SerializeField]
    private float prevHp;
    private int stack = 0;

    private float FinalDamage()
    {
        float damage = baseDamage;
        if(tokenSpace != null)
        {
            Focus focus = tokenSpace.GetComponentInChildren<Focus>();
            if (focus != null)
                damage += 5 * focus.stack;
        }

        return damage;
    }

    private IEnumerator AttackType1()
    {
        AttackAni();
        while (true)
        {
            if (goNext == true)
            {
                goNext = false;
                break;
            }
            yield return null;
        }

        // ����Ÿ�� �ı�
        int index = Random.Range(0, GameManager.instance.mapController.tiles.Count - 1);
        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        int monsterIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
        while (index == playerIndex || index == monsterIndex)
        {
            index = Random.Range(0, GameManager.instance.mapController.tiles.Count - 1);
            Quest4 quest = GameManager.instance.curQuest.GetComponent<Quest4>();
            if (quest != null && index == quest.PortalIndex)
            {
                index = playerIndex;
                continue;
            }
        }

        GameManager.instance.mapController.tiles[index].ChangeTile(Tile_Type.None);
        yield return null;
        move = null;
    }
    private IEnumerator AttackType2()
    {
        if (animator != null)
            animator.SetTrigger("Ice");
        while (true)
        {
            if (goNext == true)
            {
                goNext = false;
                break;
            }
            yield return null;
        }

        // +�� nĭ ����, �����������(���Խ� 1���̵��Ұ�)
        List<int> attackRange = new List<int>();
        int range = Random.Range(2, 4);
        //for (int i = 1; i < range + 1; i++)
        //{
        //    if (GameManager.instance.mapController.TileAvailCheck(curRow + i, curCol))
        //        attackRange.Add(GameManager.instance.mapController.ReturnIndex(curRow + i, curCol));
        //    if (GameManager.instance.mapController.TileAvailCheck(curRow - i, curCol))
        //        attackRange.Add(GameManager.instance.mapController.ReturnIndex(curRow - i, curCol));
        //}
        attackRange = DiagCrossRange(curRow, curCol, range);
        RangeAttack(attackRange, FinalDamage());
        foreach (int i in attackRange)
        {
            FireZone fireZone = GameManager.instance.mapController.tiles[i].GetComponentInChildren<FireZone>();
            if (fireZone != null)
            {
                Destroy(fireZone.gameObject, 0.1f);
                continue;
            }
            IceZone iceZone = GameManager.instance.mapController.tiles[i].GetComponentInChildren<IceZone>();
            if (iceZone != null)
            {
                iceZone.startTurn = GameManager.instance.turn;
                continue;
            }

            if (GameManager.instance.mapController.tiles[i].onTarget == OnTile.Empty)
                continue;

            IceZone effect = Resources.Load<IceZone>("Prefab/Battle/Tiles/IceZone");
            effect = Instantiate(effect, GameManager.instance.mapController.tiles[i].transform);
            effect.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
        }

        yield return null;
        move = null;
    }

    private IEnumerator AttackType3()
    {
        if(animator != null)
            animator.SetTrigger("Fire");
        while (true)
        {
            if (goNext == true)
            {
                goNext = false;
                break;
            }
            yield return null;
        }

        // x�� nĭ ����, ȭ���������(���Խõ�����)
        List<int> attackRange = new List<int>();
        int range = Random.Range(2, 4);
        //for (int i = 1; i < range + 1; i++)
        //{
        //    if (GameManager.instance.mapController.TileAvailCheck(curRow, curCol + i))
        //        attackRange.Add(GameManager.instance.mapController.ReturnIndex(curRow, curCol + i));
        //    if (GameManager.instance.mapController.TileAvailCheck(curRow, curCol - i))
        //        attackRange.Add(GameManager.instance.mapController.ReturnIndex(curRow, curCol - i));
        //}

        attackRange = CrossRange(curRow, curCol, range);
        RangeAttack(attackRange, FinalDamage());
        foreach (int i in attackRange)
        {
            IceZone iceZone = GameManager.instance.mapController.tiles[i].GetComponentInChildren<IceZone>();
            if (iceZone != null)
            {
                Destroy(iceZone.gameObject, 0.1f);
                continue;
            }
            FireZone fireZone = GameManager.instance.mapController.tiles[i].GetComponentInChildren<FireZone>();
            if (fireZone != null)
            {
                fireZone.startTurn = GameManager.instance.turn;
                continue;
            }

            if (GameManager.instance.mapController.tiles[i].onTarget == OnTile.Empty)
                continue;

            FireZone effect = Resources.Load<FireZone>("Prefab/Battle/Tiles/FireZone");
            effect = Instantiate(effect, GameManager.instance.mapController.tiles[i].transform);
            effect.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
        }

        yield return null;
        move = null;
    }

    private IEnumerator AttackType4()
    {
        // ������ġ �ڷ���Ʈ
        int index = Random.Range(0, GameManager.instance.mapController.tiles.Count - 1);
        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        while (index == playerIndex)
        {
            index = Random.Range(0, GameManager.instance.mapController.tiles.Count - 1);
            if (GameManager.instance.mapController.GetHCost(index, playerIndex) <= 3 ||
                GameManager.instance.mapController.tiles[index].onTarget == OnTile.Empty)
                index = playerIndex;
        }
        Vector3 nextPos = GameManager.instance.mapController.tiles[index].transform.position;
        move = StartCoroutine(Move(nextPos, 2, true));
        while (move != null)
            yield return null;
        curRow = GameManager.instance.mapController.tiles[index].rowIndex;
        curCol = GameManager.instance.mapController.tiles[index].colIndex;

        yield return null;
        move = null;
    }

    IEnumerator CoBehavior()
    {
        CameraController.instance.SetCameraPos(transform.position);

        if(curHp == prevHp)
        {
            if(animator != null)
            {
                animator.SetTrigger("Focus");
                float elapsed = 0;
                while (elapsed < 1f)
                {
                    elapsed+= Time.deltaTime;
                    yield return null;
                }
            }
            TakeBuff("Prefab/Buff/Focus", 11, 1);
        }
        prevHp = curHp;
        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        int monsterIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);

        bool ismoved = false;

        //�÷��̾������� 1ĭ�̵�
        GameManager.instance.mapController.tiles[playerIndex].canMove = true;
        GameManager.instance.mapController.FindPath(monsterIndex, playerIndex);
        move = null;
        if(GameManager.instance.mapController.pathIndexs != null && GameManager.instance.mapController.pathIndexs[0] != playerIndex)
        {
            GameManager.instance.mapController.FindPath(monsterIndex, GameManager.instance.mapController.pathIndexs[0]);
            GameManager.instance.moveing = StartCoroutine(MoveCharacter());
            MoveAni();
            ismoved = true;
        }
        
        while(GameManager.instance.moveing != null)
            yield return null;
        ReturnToIdle();

        int attackType = Random.Range(1, 4); // �������� �ʾҴٸ� Ÿ���ı��� ������ ���ϻ��
        if (ismoved)
            attackType = Random.Range(0, 3); // �������ٸ� ������ ������ ���ϻ��
        print(attackType);
        switch (attackType)
        {
            case 0:
                move = StartCoroutine(AttackType1()); // Ÿ���ı�
                break;
            case 1:
                move = StartCoroutine(AttackType2()); // ��������
                break;
            case 2:
                move = StartCoroutine(AttackType3()); // ȭ������
                break;
            case 3:
                move = StartCoroutine(AttackType4()); // ����
                break;
        }

        while (move != null)
            yield return null;

        Invoke("EndEnemyTrun", 1.5f);
    }

    public override void Behavior()
    {
        if (!isDead)
        {
            StartCoroutine(CoBehavior());
        }
        else
        {
            EndEnemyTrun();
        }
    }

    public override void Init()
    {
        enemyName = "�߷�";
        enemyDesc = "ȭ���� �������� �����Ѵ�.\n���ݹ��� ������ ���Ӿ��� ��ȭ�Ǹ� 1���� �ϼ����� �����Ͽ� ��ȭ�� ������ų���ִ�.";
        maxHp = 200;
        curHp = 200;

        prevHp = curHp;
        baseDamage = 15;

        maxAggroRange = 3;

        base.Init();
    }
}
