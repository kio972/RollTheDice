using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execution : SkillFunc
{
    public override void CallAni()
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/skillReady");
        AudioManager.Instance.PlayEffect(clip);

        GameManager.instance.player.ReturnToIdle();
        GameManager.instance.player.Slash();
    }

    public override void Init()
    {
        skillInfo.index = 7;
        base.Init();

        skillInfo.skillDesc =
            "ũ�� ������ ���ظ� �ش�.\n����� ������ �����ϰ� 5��ø�� �������� 100%�����Ѵ�.\n" +
            "����Ÿ�� : ����\n" +
            "���ݹ��� : 1\n" +
            "���� ���ð� : " + skillInfo.coolTime.ToString() + "\n" +
            "������ : " + skillInfo.damage.ToString();

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
                        if (enemy.tokenSpace != null)
                        {
                            Bleed bleed = enemy.tokenSpace.GetComponentInChildren<Bleed>();
                            if (bleed != null && bleed.stack >= 5)
                            {
                                int damageMag = bleed.stack / 5;
                                print(damageMag);
                                finalDamage = finalDamage * (damageMag + 1);
                                enemy.RemoveBuff(12, 100);
                            }
                        }

                        enemy.TakeDamage(finalDamage, GameManager.instance.player);
                    }
                }
            }
        }

        if (MousePointer.instance != null)
            MousePointer.instance.mouseType = MouseType.Normal;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/shout");
        GameManager.instance.player.SetTrigger("Slash2");
        SkillManager.instance.effect = StartCoroutine(Effect());
    }
}
