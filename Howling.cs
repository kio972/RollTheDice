using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howling : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 8;
        buffInfo.name = "공명";
        buffInfo.description = "강화된 기운에의해 다음번 공격에는 1의 데미지만 입는다.";
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
