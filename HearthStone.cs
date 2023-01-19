using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearthStone : Items
{
    public override void Use()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.phase == 2 | GameManager.instance.phase == 3)
            {
                GameObject temp = new GameObject();
                SceneMoveBtn btn = temp.AddComponent<SceneMoveBtn>();
                btn.sceneName = "Town";
                btn.Move();
                Abandon();
            }
        }
        else
            print("전투 중 플레이어 턴에만 사용할 수 있습니다.");
    }

    public override void Init()
    {
        itemInfo.id = 10;

        base.Init();
    }
}
