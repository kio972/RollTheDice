using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorne : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 3;
        buffInfo.name = "가시";
        buffInfo.description = "공격받을 경우 공격자에게 5의 피해를 입힙니다.";
    }

    protected override void Effect()
    {
        // takeDamage에 효과있음
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
