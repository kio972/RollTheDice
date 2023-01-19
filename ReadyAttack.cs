using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAttack : Buff
{

    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 4;
        buffInfo.name = "차지";
        buffInfo.description = "강력한 공격을 준비중입니다.";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
