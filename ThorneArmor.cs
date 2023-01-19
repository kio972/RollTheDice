using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorneArmor : RelicFunc
{
    protected override void RelicEffect()
    {
        if(GameManager.instance != null && GameManager.instance.phase == 0)
        {
            GameManager.instance.player.TakeBuff("Prefab/Buff/Thorne", 2, 5);
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 3;
        base.Init(interactable);
    }
}
