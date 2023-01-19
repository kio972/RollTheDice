using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyBattle : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 2;
        buffInfo.name = "전투준비";
        buffInfo.description = "내 턴이 시작 될 때 1의 행동력을 얻습니다.";
    }

    protected override void Effect()
    {
        if(GameManager.instance != null && GameManager.instance.phase == 1)
        {
            GameManager.instance.player.stack += stack;
            if (GameManager.instance.uiManager.stackUpdater != null)
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
