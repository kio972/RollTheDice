using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBomb : Items
{
    EnemyController nearEnemy;

    public override void Use()
    {
        if(GameManager.instance != null)
        {
            if (GameManager.instance.phase == 2 | GameManager.instance.phase == 3)
            {
                if (GameManager.instance.enemys != null)
                {
                    float nearDist = 10000;
                    foreach (EnemyController enemy in GameManager.instance.enemys)
                    {
                        float dist = MapController.instance.GetHCost(MapController.instance.ReturnIndex(enemy.curRow, enemy.curCol),
                            MapController.instance.ReturnIndex(GameManager.instance.player.curRow, GameManager.instance.player.curCol));
                        if (dist < nearDist && !enemy.isDead)
                        {
                            nearDist = dist;
                            nearEnemy = enemy;
                        }
                    }

                    if (nearEnemy != null)
                    {
                        nearEnemy.TakeDamage(20, GameManager.instance.player);
                        Abandon();
                    }
                }
            }
            else
                print("전투 중 플레이어 턴에만 사용할 수 있습니다.");
        }
    }

    public override void Init()
    {
        itemInfo.id = 5;

        base.Init();
    }

    private void Start()
    {
        Init();
    }
}
