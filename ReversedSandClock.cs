using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversedSandClock : Items
{
    public override void Use()
    {
        if(GameManager.instance != null)
        {
            if(GameManager.instance.phase == 2| GameManager.instance.phase == 3)
            {
                GameManager.instance.phase = 1;
                Abandon();
            }
        }
        else
            print("전투 중 플레이어 턴에만 사용할 수 있습니다.");
    }

    public override void Init()
    {
        itemInfo.id = 4;

        base.Init();
    }

    private void Start()
    {
        Init();
    }
}
