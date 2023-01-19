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
            print("���� �� �÷��̾� �Ͽ��� ����� �� �ֽ��ϴ�.");
    }

    public override void Init()
    {
        itemInfo.id = 10;

        base.Init();
    }
}
