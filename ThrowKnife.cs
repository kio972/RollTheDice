using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : RelicFunc
{
    IEnumerator Effect(EnemyController enemy)
    {
        float elapsed = 0;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        enemy.TakeDamage(3, GameManager.instance.player);
    }

    protected override void RelicEffect()
    {
        if (SkillManager.instance != null && SkillManager.instance.currSkill != null)
        {
            if (SkillManager.instance.currSkill.skillInfo.type != AttackType.Buff)
                StartCoroutine(Effect(SkillManager.instance.targetEnemy));
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 1;
        base.Init(interactable);
    }
}
