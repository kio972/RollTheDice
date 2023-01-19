using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStackPotion : Items
{
    public override void Use()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.phase == 2 | GameManager.instance.phase == 3)
            {
                GameManager.instance.player.stack += 1;
                GameManager.instance.uiManager.stackUpdater.UpdateStack();

                Abandon();
            }
        }
        else
            print("전투 중 플레이어 턴에만 사용할 수 있습니다.");
        print("사용");
        
    }

    public override void Init()
    {
        itemInfo.id = 6;

        base.Init();
    }
}
