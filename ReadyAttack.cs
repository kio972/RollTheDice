using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAttack : Buff
{

    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 4;
        buffInfo.name = "����";
        buffInfo.description = "������ ������ �غ����Դϴ�.";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
