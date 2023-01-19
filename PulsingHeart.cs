using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingHeart : RelicFunc
{
    protected override void RelicEffect()
    {
        if(SkillManager.instance != null && SkillManager.instance.currSkill != null)
        {
            if(SkillManager.instance.currSkill.skillInfo.type == AttackType.Buff)
            {
                DamageText.instance.Heal(GameManager.instance.player.transform, 1);
                float targetHp = GameManager.instance.player.curHp += 1;
                if (targetHp >= GameManager.instance.player.maxHp)
                    targetHp = GameManager.instance.player.maxHp;
                PlayerStat.instance.hpBar.UpdateHealthBar(GameManager.instance.player.curHp, targetHp, GameManager.instance.player.maxHp);
                GameManager.instance.player.curHp = targetHp;
            }
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 4;
        base.Init(interactable);
    }
}
