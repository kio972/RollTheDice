using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatDice : RelicFunc
{
    private void AddStack()
    {
        GameManager.instance.player.stack += 1;
        GameManager.instance.uiManager.stackUpdater.UpdateStack();
    }

    protected override void RelicEffect()
    {
        if (GameManager.instance != null && GameManager.instance.phase == 2)
        {
            Invoke("AddStack", 0.5f);
        }
    }

    public override void Init(bool interactable = true)
    {
        relicInfo.index = 7;
        base.Init(interactable);
    }
}
