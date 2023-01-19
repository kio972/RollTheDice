using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : SkillFunc
{
    public override void CallAni()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillReady");
        AudioManager.Instance.PlayEffect(clip);

        GameManager.instance.player.ReturnToIdle();
        GameManager.instance.player.Stab();
    }

    public override void Init()
    {
        skillInfo.index = 6;
        base.Init();

        skillInfo.skillDesc =
            "적에게 조금의 데미지와 함께 출혈을 4부여합니다." +
            "공격타입 : 지점\n" +
            "공격범위 : 1\n" +
            "재사용 대기시간 : " + skillInfo.coolTime.ToString() + "\n" +
            "데미지 : " + skillInfo.damage.ToString() + "\n";

    }

    public override void Skill()
    {
        SkillBtn btn = GetComponentInParent<SkillBtn>();
        btn.CoolDown = true;
        btn.coolTime = skillInfo.coolTime;

        GameManager.instance.player.stack -= skillInfo.cost;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();

        List<int> indexs = new List<int>();
        foreach (Tile tile in GameManager.instance.mapController.tiles)
        {
            if (tile.inRange)
            {
                indexs.Add(GameManager.instance.mapController.ReturnIndex(tile.rowIndex, tile.colIndex));
            }
        }

        foreach (int index in indexs)
        {
            foreach (EnemyController enemy in GameManager.instance.enemys)
            {
                int enemyIndex = GameManager.instance.mapController.ReturnIndex(enemy.curRow, enemy.curCol);
                if (enemyIndex == index)
                {
                    finalDamage = skillInfo.damage;
                    SkillManager.instance.targetEnemy = enemy;
                    GameManager.instance.player.playerStat.relic.BroadcastMessage("RelicEffect", SendMessageOptions.DontRequireReceiver);
                    if (!enemy.isDead)
                    {
                        enemy.TakeDamage(finalDamage, GameManager.instance.player);
                        enemy.TakeBuff("Prefab/Buff/Bleed", 12, 4);
                        FxEffectManager.Instance.PlayEffect("Prefab/Battle/Effect/Scratch", enemy.transform.position + new Vector3(0, 0, 1));
                    }
                }
            }
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;

        GameManager.instance.player.Stab();
        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
