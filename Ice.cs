using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 9;
        buffInfo.name = "����";
        buffInfo.description = "�� �ϵ��� �̵��� �Ұ�������.";
    }

    protected override void Effect()
    {
        // �� ���۽� ���ð���ؼ� ������
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
