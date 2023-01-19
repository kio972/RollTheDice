using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 10;
        buffInfo.name = "화상";
        buffInfo.description = "내 턴이 시작될때 스택에 비례하여 5의 데미지를 입는다";
    }

    protected override void Effect()
    {
        // 턴 시작시 스택계산해서 데미지
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            target.TakeDamage(5 * stack, null);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
