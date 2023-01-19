using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BravePendant : RelicFunc
{
    protected override void RelicEffect()
    {
        if(GameManager.instance != null && GameManager.instance.phase == 0)
        {
            GameManager.instance.player.stack += 3;
            GameManager.instance.uiManager.stackUpdater.UpdateStack();
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 5;
        base.Init(interactable);
    }
}
