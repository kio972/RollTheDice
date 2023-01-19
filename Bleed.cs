using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 12;
        buffInfo.name = "출혈";
        buffInfo.description = "턴이 시작될때 중첩량만큼의 피해를 입는다.\n턴당 1중첩 감소";
    }

    protected override void Effect()
    {
        // 턴 시작시 스택계산해서 데미지
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            target.TakeDamage(stack, null);
            
            stack--;
            SetStack();

            if(stack <= 0)
                Destroy(gameObject, 0.1f);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
