using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : EnemyController
{
    private float prevHp;
    private int prevMonsterNum;
    private int stack = 0;

    private int patternCount = 0;

    public override void TakeDamage(float damage, Controller attacker)
    {
        if (isDead)
            return;

        Howling howling = tokenSpace.GetComponentInChildren<Howling>();
        if(howling != null)
        {
            RemoveBuff(8, 1);
            damage = 1;
        }
        base.TakeDamage(damage, attacker);

        if (isDead)
        {
            Quest4 quest = GameManager.instance.curQuest.GetComponent<Quest4>();
            if (quest != null)
            {
                if(!quest.portalOpen)
                    quest.portalOpen = true;
                quest.firstFMonsters.Remove(this);
                quest.secondFMonster.RemoveBuff(11, 100);
            }
            //죽으면 플레이어에게 화염버프
            GameManager.instance.player.TakeBuff("Prefab/Buff/FireImmune", 7, 5);
            RemoveBuff(7);
        }
    }

    public void LostHowlingBuff()
    {
        RemoveBuff(8, 100);
        stack = 0;
    }

    private bool HowlingBuffCheck()
    {
        if (prevMonsterNum == GameManager.instance.enemys.Count)
        {
            if (prevHp == curHp)
            {
                stack++;
                if (stack == 2)
                {
                    // 강화이펙트
                    stack = 0;
                    return true;
                }
            }
            return false;
        }
        else
        {
            // 제거이펙트
            LostHowlingBuff();
            return false;
        }
    }



    private IEnumerator AttackType1()
    {

        // 대각공격
        RangeAttack(DiagCrossRange(curRow, curCol, 2), baseDamage, "Prefab/Battle/Effect/HitRed");
        yield return null;
        move = null;
    }
    private IEnumerator AttackType2()
    {
        // 직선공격
        RangeAttack(CrossRange(curRow, curCol, 2), baseDamage, "Prefab/Battle/Effect/HitRed");
        yield return null;
        move = null;
    }

    private IEnumerator AttackType3()
    {
        // 안쪽 주변 1칸 공격
        RangeAttack(BasicRange(curRow, curCol, 1), baseDamage, "Prefab/Battle/Effect/HitRed");
        yield return null;
        move = null;
    }

    IEnumerator CoBehavior()
    {
        float invokeTime = 0.1f;
        // 몬스터숫자가 달라졌는지 확인하고, 달라졌다면 공명 버프 제거, 아니면 2턴동안안맞으면 공명버프 1획득
        if (HowlingBuffCheck())
        {
            CameraController.instance.SetCameraPos(transform.position);
            float elapsed = 0;
            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            animator.SetTrigger("Charge");
            TakeBuff("Prefab/Buff/Howling", 8);
            invokeTime = 1f;
        }

        int playerIndex = GameManager.instance.mapController.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol);
        int monsterIndex = GameManager.instance.mapController.ReturnIndex(curRow, curCol);
        
        // 어그로범위 들어오면 행동
        if(GameManager.instance.mapController.GetHCost(monsterIndex, playerIndex) <= maxAggroRange)
        {
            CameraController.instance.SetCameraPos(transform.position);

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

            switch (patternCount)
            {
                case 0:
                    move = StartCoroutine(AttackType1());
                    break;
                case 1:
                    move = StartCoroutine(AttackType2());
                    break;
                case 2:
                    move = StartCoroutine(AttackType3());
                    break;
            }

            invokeTime = 0.5f;

            patternCount++;
            if (patternCount > 2)
                patternCount = 0;
        }

        while (move != null)
            yield return null;

        Invoke("EndEnemyTrun", invokeTime);
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
        enemyName = "화염의 하수인";
        enemyDesc = "공격받지 않으면 끊임없이 강화된다.\n결속을 끊으면 강화를 해제시킬수있다.";
        maxHp = 60;
        curHp = 60;

        baseDamage = 10;

        prevHp = curHp;
        prevMonsterNum = 4;

        maxAggroRange = 3;

        base.Init();
        TakeBuff("Prefab/Buff/FireImmune", 7);
    }
}
