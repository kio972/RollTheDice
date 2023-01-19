using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 9;
        buffInfo.name = "동상";
        buffInfo.description = "한 턴동안 이동이 불가해진다.";
    }

    protected override void Effect()
    {
        // 턴 시작시 스택계산해서 데미지
        if (GameManager.instance != null && GameManager.instance.phase == 2)
        {
            GameManager.instance.endInput = true;
            stack--;
            SetStack();
            if(stack <= 0)
                Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
