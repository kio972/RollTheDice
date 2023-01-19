using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bandit : EnemyController
{
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

    IEnumerator BanditBehavior()
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

        GameManager.instance.mapController.tiles[playerIndex].canMove = true;
        GameManager.instance.mapController.FindPath(curIndex, playerIndex);
        if (maxAggroRange >= GameManager.instance.mapController.pathCost)
        {
            if (GameManager.instance.mapController.pathCost == 1 || GameManager.instance.mapController.pathCost == 1.5f)
            {
                AttackAni();
                Attack(playerIndex);
            }
            else
            {
                if (GameManager.instance.mapController.pathIndexs != null && GameManager.instance.mapController.pathIndexs.Count != 0)
                {
                    int moveIndex = GameManager.instance.mapController.pathIndexs[0];
                    GameManager.instance.mapController.FindPath(curIndex, moveIndex);
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
            StartCoroutine(BanditBehavior());
        }
        else
        {
            EndEnemyTrun();
        }
    }

    public override void Init()
    {
        enemyName = "고블린";
        enemyDesc = "주변 마을을 약탈하며 살아가는 종족이다";

        baseDamage = 8;
        maxHp = 30;
        curHp = 30;
        maxAggroRange = 8;

        base.Init();
    }

}
