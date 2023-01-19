using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 10;
        buffInfo.name = "ȭ��";
        buffInfo.description = "�� ���� ���۵ɶ� ���ÿ� ����Ͽ� 5�� �������� �Դ´�";
    }

    protected override void Effect()
    {
        // �� ���۽� ���ð���ؼ� ������
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
