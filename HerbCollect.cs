using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbCollect : Buff
{

    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 1;
        buffInfo.name = "약초수집";
        buffInfo.description = "약초 향기가 납니다.";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
