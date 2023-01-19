using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighStackPotion : Items
{
    public override void Use()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.phase == 2 | GameManager.instance.phase == 3)
            {
                GameManager.instance.player.stack += 6;
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
        itemInfo.id = 8;

        base.Init();
    }
}
