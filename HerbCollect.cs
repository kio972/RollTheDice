using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbCollect : Buff
{

    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 1;
        buffInfo.name = "���ʼ���";
        buffInfo.description = "���� ��Ⱑ ���ϴ�.";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
