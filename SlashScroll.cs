using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScroll : RelicFunc
{
    protected override void RelicEffect()
    {
        if (GameManager.instance != null && GameManager.instance.phase == 0)
        {
            Slash slash = GameManager.instance.skillManager.GetComponentInChildren<Slash>();
            if (slash != null)
            {
                slash.skillInfo.coolTime = 0;
                slash.skillInfo.damage -= 5;
            }
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 2;
        base.Init(interactable);
    }
}
