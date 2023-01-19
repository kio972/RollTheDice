using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 11;
        buffInfo.name = "마력집중";
        buffInfo.description = "피해를 입지 않을시 획득합니다. 중첩당 공격력이 5 상승합니다. 1층의 하수인을 제거하면 사라집니다.";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
