using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howling : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 8;
        buffInfo.name = "����";
        buffInfo.description = "��ȭ�� ������ ������ ���ݿ��� 1�� �������� �Դ´�.";
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
