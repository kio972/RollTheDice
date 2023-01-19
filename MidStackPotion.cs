using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidStackPotion : Items
{
    public override void Use()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.phase == 2 | GameManager.instance.phase == 3)
            {
                GameManager.instance.player.stack += 3;
                GameManager.instance.uiManager.stackUpdater.UpdateStack();

                Abandon();
            }
        }
        else
            print("���� �� �÷��̾� �Ͽ��� ����� �� �ֽ��ϴ�.");
        print("���");
        
    }

    public override void Init()
    {
        itemInfo.id = 7;

        base.Init();
    }
}
