using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 11;
        buffInfo.name = "��������";
        buffInfo.description = "���ظ� ���� ������ ȹ���մϴ�. ��ø�� ���ݷ��� 5 ����մϴ�. 1���� �ϼ����� �����ϸ� ������ϴ�.";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
