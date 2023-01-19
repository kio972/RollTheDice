using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : RelicFunc
{
    protected override void RelicEffect()
    {
        if(SkillManager.instance != null && SkillManager.instance.currSkill != null)
        {
            if(SkillManager.instance.currSkill.skillInfo.index == 0)
            {
                SkillManager.instance.currSkill.finalDamage += 5;
            }
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 0;
        base.Init(interactable);
    }
}
