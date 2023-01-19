using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceImmune : Buff
{
    public override void Init(Controller target, int stack = 1)
    {
        base.Init(target, stack);
        id = 6;
        buffInfo.name = "������ȣ��";
        buffInfo.description = "�������뿡 ���� �Դ� ���ظ� ��ȿȭ�Ѵ�";
    }

    protected override void Effect()
    {
        // �ϸ��� 1���ҵǰԸ��ϸ��, ���� 0�̸� 
        if (GameManager.instance != null && GameManager.instance.phase == 1)
        {
            stack--;
            SetStack();
            if (stack <= 0)
                Destroy(gameObject);
        }

        // ���� ��ȿȭ�� �������� �����ϴ¾ְ� �˾Ƽ��Ұ���
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}