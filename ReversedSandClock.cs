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
            print("���� �� �÷��̾� �Ͽ��� ����� �� �ֽ��ϴ�.");
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
