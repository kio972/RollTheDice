using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireImmune : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 7;
        buffInfo.name = "화염보호막";
        buffInfo.description = "화염지대에 의해 입는 피해를 무효화한다";
    }

    protected override void Effect()
    {
        // 턴마다 1감소되게만하면됨, 스택 0이면 
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            stack--;
            SetStack();
            if(stack <= 0)
                Destroy(gameObject);
        }

        // 피해 무효화는 함정지형 관리하는애가 알아서할거임
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
