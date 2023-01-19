using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 5;
        buffInfo.name = "거미줄";
        buffInfo.description = "내 턴이 시작될때 중첩당 행동력을 1 잃는다.";
    }

    protected override void Effect()
    {
        // 턴 시작시 스택계산해서 데미지
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            GameManager.instance.player.stack -= stack;
            GameManager.instance.uiManager.stackUpdater.UpdateStack();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
