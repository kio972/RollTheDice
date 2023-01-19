using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 12;
        buffInfo.name = "����";
        buffInfo.description = "���� ���۵ɶ� ��ø����ŭ�� ���ظ� �Դ´�.\n�ϴ� 1��ø ����";
    }

    protected override void Effect()
    {
        // �� ���۽� ���ð���ؼ� ������
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            target.TakeDamage(stack, null);
            
            stack--;
            SetStack();

            if(stack <= 0)
                Destroy(gameObject, 0.1f);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
