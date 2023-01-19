using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireWing : RelicFunc
{
    private float damage = 3;

    protected override void RelicEffect()
    {
        if(GameManager.instance != null && GameManager.instance.phase == 0)
        {
            float toalDamage = 0;
            foreach(EnemyController enemy in GameManager.instance.enemys)
            {
                float damaged = enemy.curHp;
                enemy.TakeDamage(damage, null);
                damaged = damaged - enemy.curHp;
                toalDamage += damaged;
            }

            DamageText.instance.Heal(GameManager.instance.player.transform, toalDamage);
            float targetHp = GameManager.instance.player.curHp += toalDamage;
            if (targetHp >= GameManager.instance.player.maxHp)
                targetHp = GameManager.instance.player.maxHp;
            PlayerStat.instance.hpBar.UpdateHealthBar(GameManager.instance.player.curHp, targetHp, GameManager.instance.player.maxHp);
            GameManager.instance.player.curHp = targetHp;
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 6;
        base.Init(interactable);
    }
}
