using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorne : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 3;
        buffInfo.name = "����";
        buffInfo.description = "���ݹ��� ��� �����ڿ��� 5�� ���ظ� �����ϴ�.";
    }

    protected override void Effect()
    {
        // takeDamage�� ȿ������
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
